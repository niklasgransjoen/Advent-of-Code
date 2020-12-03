using System;

namespace AOC.Y2019.Day17.P01
{
    public sealed class IntcodeInterpreter : IInterpreter
    {
        private const int ParameterBufferSize = 3;

        private static readonly OpcodeInfo _addInfo = new OpcodeInfo(paramCount: 3);
        private static readonly OpcodeInfo _mulInfo = new OpcodeInfo(paramCount: 3);
        private static readonly OpcodeInfo _inpInfo = new OpcodeInfo(paramCount: 1);
        private static readonly OpcodeInfo _outInfo = new OpcodeInfo(paramCount: 1);
        private static readonly OpcodeInfo _jnzInfo = new OpcodeInfo(paramCount: 2);
        private static readonly OpcodeInfo _jzInfo = new OpcodeInfo(paramCount: 2);
        private static readonly OpcodeInfo _ltInfo = new OpcodeInfo(paramCount: 3);
        private static readonly OpcodeInfo _eqlInfo = new OpcodeInfo(paramCount: 3);
        private static readonly OpcodeInfo _arbInfo = new OpcodeInfo(paramCount: 1);

        private readonly IIOPort _ioPort;

        private readonly long[] _intcode;
        private readonly int[] _parameters = new int[ParameterBufferSize];

        private int _instructionPointer = 0;
        private int _relativeBase = 0;

        public IntcodeInterpreter(Span<long> intcode, IIOPort ioPort)
        {
            // Allocate more memory than needed.
            _intcode = new long[intcode.Length * 100];
            intcode.CopyTo(_intcode);

            _ioPort = ioPort;
        }

        #region Properties

        public bool HasHalted { get; private set; }

        private int CurrentInstruction => (int)_intcode[_instructionPointer];

        #endregion Properties

        /// <summary>
        /// Starts the intcode execution.
        /// </summary>
        /// <returns>True if program halts, otherwise false.</returns>
        public bool Execute()
        {
            while (true)
            {
                Opcode opcode = GetOpcode();
                if (opcode == Opcode.HLT)
                {
                    HasHalted = true;
                    return true;
                }

                OpcodeInfo opcodeInfo = GetOpcodeInfo(opcode);
                for (int i = 0; i < opcodeInfo.ParameterCount; i++)
                {
                    int valueIndex = _instructionPointer + i + 1;
                    ParameterMode parameterMode = GetParameterMode(i);
                    _parameters[i] = ReadValue(parameterMode, valueIndex);
                }

                var result = ExecuteOpcode(opcode);
                if (result.AwaitIOPort)
                    return false;
                else if (result.NextInstruction == -1)
                    _instructionPointer += opcodeInfo.ParameterCount + 1;
                else
                    _instructionPointer = result.NextInstruction;
            }
        }

        /// <summary>
        /// Returns the opcode of the current instruction.
        /// </summary>
        private Opcode GetOpcode()
        {
            int opcode = CurrentInstruction % 100;
            return (Opcode)opcode;
        }

        private static OpcodeInfo GetOpcodeInfo(Opcode opcode)
        {
            return opcode switch
            {
                Opcode.ADD => _addInfo,
                Opcode.MUL => _mulInfo,
                Opcode.INP => _inpInfo,
                Opcode.OUT => _outInfo,
                Opcode.JNZ => _jnzInfo,
                Opcode.JZ => _jzInfo,
                Opcode.LT => _ltInfo,
                Opcode.EQL => _eqlInfo,
                Opcode.ARB => _arbInfo,

                _ => throw new IntcodeEvaluationException($"Opcode '{opcode}' is illegal."),
            };
        }

        /// <summary>
        /// Gets the parameter mode of the parameter of the given index.
        /// </summary>
        private ParameterMode GetParameterMode(int paramIndex)
        {
            int instruction = CurrentInstruction;

            // Divide by 100, then 10 for each extra position
            instruction /= 100;
            for (int i = 0; i < paramIndex; i++)
                instruction /= 10;

            // Remove all higher parameter modes.
            instruction %= 10;

            // Param mode is Position if first two bits are zero, Immediate if they are one, and relative if two.
            byte mode = (byte)(instruction & 0b11);
            return mode switch
            {
                0 => ParameterMode.Position,
                1 => ParameterMode.Immediate,
                2 => ParameterMode.Relative,

                _ => throw new IntcodeEvaluationException($"Parameter mode '{mode}' is illegal"),
            };
        }

        private int ReadValue(ParameterMode parameterMode, int index)
        {
            return parameterMode switch
            {
                ParameterMode.Position => (int)_intcode[index],
                ParameterMode.Immediate => index,
                ParameterMode.Relative => (int)_intcode[index] + _relativeBase,

                _ => throw new IntcodeEvaluationException($"Parameter mode '{parameterMode}' is illegal."),
            };
        }

        /// <summary>
        /// Executes an opcode.
        /// </summary>
        /// <returns>The next position of the instruction pointer. -1 if it should just be incremented as usual.</returns>
        private OpcodeResult ExecuteOpcode(Opcode opcode)
        {
            long[] c = _intcode;
            int[] p = _parameters;

            switch (opcode)
            {
                case Opcode.ADD:
                    c[p[2]] = c[p[0]] + c[p[1]];
                    break;

                case Opcode.MUL:
                    c[p[2]] = c[p[0]] * c[p[1]];
                    break;

                case Opcode.INP:
                    IOReadResult ioResult = _ioPort.Read();
                    if (ioResult.ValueAvailable)
                        c[p[0]] = ioResult.Value;
                    else
                    {
                        _ioPort.RegisterInterpreterForInput(this);
                        return new OpcodeResult
                        {
                            AwaitIOPort = true,
                        };
                    }
                    break;

                case Opcode.OUT:
                    _ioPort.Write(c[p[0]]);
                    break;

                case Opcode.JNZ:
                    if (c[p[0]] != 0)
                        return new OpcodeResult((int)c[p[1]]);
                    break;

                case Opcode.JZ:
                    if (c[p[0]] == 0)
                        return new OpcodeResult((int)c[p[1]]);
                    break;

                case Opcode.LT:
                    c[p[2]] = c[p[0]] < c[p[1]] ? 1 : 0;
                    break;

                case Opcode.EQL:
                    c[p[2]] = c[p[0]] == c[p[1]] ? 1 : 0;
                    break;

                case Opcode.ARB:
                    _relativeBase += (int)c[p[0]];
                    break;

                default:
                    throw new IntcodeEvaluationException($"Opcode '{opcode}' is illegal.");
            }

            return new OpcodeResult(-1);
        }

        private struct OpcodeResult
        {
            public OpcodeResult(int nextInstruction)
            {
                NextInstruction = nextInstruction;
                AwaitIOPort = false;
            }

            /// <summary>
            /// The next value of the instruction pointer. -1 if the current value should just be incremented.
            /// </summary>
            public int NextInstruction { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the program should stop execution to await the IO port recieving data.
            /// </summary>
            public bool AwaitIOPort { get; set; }
        }
    }
}
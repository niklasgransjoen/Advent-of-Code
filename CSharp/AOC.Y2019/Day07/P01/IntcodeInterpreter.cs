using System;

namespace AOC.Y2019.Day07.P01
{
    public sealed class IntcodeInterpreter
    {
        private const int ParameterBufferSize = 3;

        private static readonly OpcodeInfo _addInfo = new OpcodeInfo(new bool[] { false, false, true });
        private static readonly OpcodeInfo _mulInfo = new OpcodeInfo(new bool[] { false, false, true });
        private static readonly OpcodeInfo _inpInfo = new OpcodeInfo(new bool[] { true });
        private static readonly OpcodeInfo _outInfo = new OpcodeInfo(new bool[] { false });
        private static readonly OpcodeInfo _jnzInfo = new OpcodeInfo(new bool[] { false, false });
        private static readonly OpcodeInfo _jzInfo = new OpcodeInfo(new bool[] { false, false });
        private static readonly OpcodeInfo _ltInfo = new OpcodeInfo(new bool[] { false, false, true });
        private static readonly OpcodeInfo _eqlInfo = new OpcodeInfo(new bool[] { false, false, true });

        private int[] _intcode = Array.Empty<int>();
        private int[] _input = Array.Empty<int>();

        private bool _hasOutput;
        private int _output;

        public IntcodeInterpreter()
        {
        }

        public int Execute(int[] intcode, int[] input)
        {
            ResetState();
            _intcode = intcode ?? throw new ArgumentNullException(nameof(intcode));
            _input = input ?? throw new ArgumentNullException(nameof(input));

            int index = 0;
            int[] parameters = new int[ParameterBufferSize];

            while (true)
            {
                Opcode opcode = GetOpcode(intcode[index]);
                if (opcode == Opcode.HLT)
                {
                    if (!_hasOutput)
                        throw new IntcodeEvaluationException("Interpreter halted with no output");

                    return _output;
                }

                OpcodeInfo opcodeInfo = GetOpcodeInfo(opcode);
                for (int i = 0; i < opcodeInfo.ParameterCount; i++)
                {
                    int valueIndex = index + i + 1;
                    if (opcodeInfo.IsWriteDestination[i])
                    {
                        // Write destination values are always position mode.
                        parameters[i] = ReadValue(ParameterMode.Position, valueIndex);
                    }
                    else
                    {
                        ParameterMode parameterMode = GetParameterMode(intcode[index], i);
                        parameters[i] = ReadValue(parameterMode, valueIndex);
                    }
                }

                int nextInstruction = ExecuteOpcode(opcode, parameters);
                if (nextInstruction == -1)
                    index += opcodeInfo.ParameterCount + 1;
                else
                    index = nextInstruction;
            }
        }

        private static Opcode GetOpcode(int rawInstaction)
        {
            int opcode = rawInstaction % 100;
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

                _ => throw new IntcodeEvaluationException($"Opcode '{opcode}' is illegal."),
            };
        }

        private static ParameterMode GetParameterMode(int instruction, int paramModeIndex)
        {
            // Divide by 100, then 10 for each extra position
            instruction /= 100;
            for (int i = 0; i < paramModeIndex; i++)
                instruction /= 10;

            // Param mode is Position if first bit is zero, Immediate if it's one.
            bool modeIsPosition = (instruction & 0b1) == 0;
            return modeIsPosition ? ParameterMode.Position : ParameterMode.Immediate;
        }

        private int ReadValue(ParameterMode parameterMode, int index)
        {
            return parameterMode switch
            {
                ParameterMode.Position => _intcode[index],
                ParameterMode.Immediate => index,

                _ => throw new IntcodeEvaluationException($"Parameter mode '{parameterMode}' is illegal."),
            };
        }

        /// <summary>
        /// Executes an opcode.
        /// </summary>
        /// <returns>The next position of the instruction pointer. -1 if it should just be incremented as usual.</returns>
        private int ExecuteOpcode(Opcode opcode, ReadOnlySpan<int> p)
        {
            int[] c = _intcode;
            switch (opcode)
            {
                case Opcode.ADD:
                    c[p[2]] = c[p[0]] + c[p[1]];
                    break;

                case Opcode.MUL:
                    c[p[2]] = c[p[0]] * c[p[1]];
                    break;

                case Opcode.INP:
                    c[p[0]] = ReadInput();
                    break;

                case Opcode.OUT:
                    WriteOutput(c[p[0]]);
                    break;

                case Opcode.JNZ:
                    if (c[p[0]] != 0)
                        return c[p[1]];
                    break;

                case Opcode.JZ:
                    if (c[p[0]] == 0)
                        return c[p[1]];
                    break;

                case Opcode.LT:
                    c[p[2]] = c[p[0]] < c[p[1]] ? 1 : 0;
                    break;

                case Opcode.EQL:
                    c[p[2]] = c[p[0]] == c[p[1]] ? 1 : 0;
                    break;

                default:
                    throw new IntcodeEvaluationException($"Opcode '{opcode}' is illegal.");
            }

            return -1;
        }

        #region Utility

        private void ResetState()
        {
            _currentInput = 0;
            _output = 0;
            _hasOutput = false;
        }

        private int _currentInput;

        private int ReadInput()
        {
            if (_currentInput >= _input.Length)
                throw new IntcodeEvaluationException($"Intcode execution attempted to read input #{_currentInput + 1}, but there are only {_currentInput} inputs available.");

            int input = _input[_currentInput];
            _currentInput++;

            return input;
        }

        private void WriteOutput(int output)
        {
            _hasOutput = true;
            _output = output;
        }

        #endregion Utility
    }
}
using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2019.Day05
{
    /**
     * https://adventofcode.com/2019/day/5
     */

    public static class Part02
    {
        private enum Opcode
        {
            ADD = 1,
            MUL = 2,
            INP = 3,
            OUT = 4,
            JNZ = 5,
            JZ = 6,
            LT = 7,
            EQL = 8,
            HLT = 99,
        }

        private enum ParameterMode
        {
            Position,
            Immediate
        }

        private static readonly OpcodeInfo _addInfo = new OpcodeInfo(parameterCount: 3, new bool[] { false, false, true });
        private static readonly OpcodeInfo _mulInfo = new OpcodeInfo(parameterCount: 3, new bool[] { false, false, true });
        private static readonly OpcodeInfo _inpInfo = new OpcodeInfo(parameterCount: 1, new bool[] { true });
        private static readonly OpcodeInfo _outInfo = new OpcodeInfo(parameterCount: 1, new bool[] { false });
        private static readonly OpcodeInfo _jnzInfo = new OpcodeInfo(parameterCount: 2, new bool[] { false, false });
        private static readonly OpcodeInfo _jzInfo = new OpcodeInfo(parameterCount: 2, new bool[] { false, false });
        private static readonly OpcodeInfo _ltInfo = new OpcodeInfo(parameterCount: 3, new bool[] { false, false, true });
        private static readonly OpcodeInfo _eqlInfo = new OpcodeInfo(parameterCount: 3, new bool[] { false, false, true });

        private const int ParameterBufferSize = 3;
        private const int Input = 5;

        public static void Exec()
        {
            string[] input = General.ReadCSVInput(Day.Day05);
            int[] intcode = General.ParseStrings(input);

            ExecuteIntcode(intcode);
            Console.ReadKey();
        }

        private static void ExecuteIntcode(Span<int> intcode)
        {
            Console.WriteLine("Intcode execution initiated");

            int index = 0;
            int[] parameters = new int[ParameterBufferSize];

            while (true)
            {
                Opcode opcode = GetOpcode(intcode[index]);
                if (opcode == Opcode.HLT)
                {
                    Console.WriteLine("The program has halted.");
                    break;
                }

                OpcodeInfo opcodeInfo = GetOpcodeInfo(opcode);
                for (int i = 0; i < opcodeInfo.ParameterCount; i++)
                {
                    int valueIndex = index + i + 1;
                    if (opcodeInfo.IsWriteDestination[i])
                    {
                        // Write destination values are position mode, but we use immediate to read *address* of write destination.
                        parameters[i] = ReadValue(ParameterMode.Immediate, intcode, valueIndex);
                    }
                    else
                    {
                        ParameterMode parameterMode = GetParameterMode(intcode[index], i);
                        parameters[i] = ReadValue(parameterMode, intcode, valueIndex);
                    }
                }

                int nextInstruction = ExecuteOpcode(intcode, opcode, parameters);
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

                _ => throw new ArgumentException("Invalid opcode"),
            };
        }

        private static ParameterMode GetParameterMode(int rawInstruction, int paramModeIndex)
        {
            // Divide by 100, then 10 for each extra position
            for (int i = 0; i < paramModeIndex + 2; i++)
                rawInstruction /= 10;

            // Param mode is Position if first bit is zero, Immediate if it's one.
            bool modeIsPosition = (rawInstruction & 0b1) == 0;
            return modeIsPosition ? ParameterMode.Position : ParameterMode.Immediate;
        }

        private static int ReadValue(ParameterMode parameterMode, Span<int> intcode, int index)
        {
            return parameterMode switch
            {
                ParameterMode.Position => intcode[intcode[index]],
                ParameterMode.Immediate => intcode[index],

                _ => throw new ArgumentException("Invalid parameter mode"),
            };
        }

        /// <summary>
        /// Executes an opcode.
        /// </summary>
        /// <returns>The next position of the instruction pointer. -1 if it should just be incremented as usual.</returns>
        private static int ExecuteOpcode(Span<int> intcode, Opcode opcode, ReadOnlySpan<int> parameters)
        {
            switch (opcode)
            {
                case Opcode.ADD:
                    intcode[parameters[2]] = parameters[0] + parameters[1];
                    break;

                case Opcode.MUL:
                    intcode[parameters[2]] = parameters[0] * parameters[1];
                    break;

                case Opcode.INP:
                    intcode[parameters[0]] = Input;
                    break;

                case Opcode.OUT:
                    Console.WriteLine(parameters[0]);
                    break;

                case Opcode.JNZ:
                    if (parameters[0] != 0)
                        return parameters[1];
                    break;

                case Opcode.JZ:
                    if (parameters[0] == 0)
                        return parameters[1];
                    break;

                case Opcode.LT:
                    intcode[parameters[2]] = parameters[0] < parameters[1] ? 1 : 0;
                    break;

                case Opcode.EQL:
                    intcode[parameters[2]] = parameters[0] == parameters[1] ? 1 : 0;
                    break;

                default:
                    throw new ArgumentException("Invalid opcode");
            }

            return -1;
        }

        private readonly struct OpcodeInfo
        {
            public OpcodeInfo(int parameterCount, IEnumerable<bool> isWriteDestination)
            {
                ParameterCount = parameterCount;
                IsWriteDestination = isWriteDestination.ToList().AsReadOnly();
            }

            public int ParameterCount { get; }

            public IReadOnlyList<bool> IsWriteDestination { get; }
        }
    }
}
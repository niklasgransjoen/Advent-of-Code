using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day14
{
    /**
      * https://adventofcode.com/2020/day/14
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var instructions = ParseInstructions(input);
            var memory = ComputeMemory(instructions);

            var result = memory.Values.Aggregate(0uL, (sum, val) => checked(sum + val));
            AOCUtils.PrintResult(result);
        }

        private static Instruction[] ParseInstructions(string[] input)
        {
            var instructions = new Instruction[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                var line = input[i];
                if (line.StartsWith("mask", StringComparison.Ordinal))
                {
                    var match = Regex.Match(line, @"^mask = ([X10]+)$");
                    if (!match.Success)
                        throw new Exception($"Invalid mask instruction '{line}'.");

                    instructions[i] = new MaskInstruction(match.Groups[1].Value);
                }
                else
                {
                    var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)$");
                    if (!match.Success)
                        throw new Exception($"Invalid memory instruction '{line}'.");

                    var address = ulong.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var value = ulong.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    instructions[i] = new MemoryInstruction(address, value);
                }
            }

            return instructions;
        }

        private static Dictionary<ulong, ulong> ComputeMemory(Instruction[] instructions)
        {
            if (instructions.Length == 0)
                return new();
            if (instructions[0] is not MaskInstruction initialMask)
                throw new Exception("First instruction must be a mask instruction.");

            var memory = new Dictionary<ulong, ulong>();
            var mask = initialMask.Mask;

            foreach (var instruction in instructions.AsSpan(1))
            {
                switch (instruction)
                {
                    case MaskInstruction maskInstruction:
                        mask = maskInstruction.Mask;
                        break;

                    case MemoryInstruction memoryInstruction:
                        memory[memoryInstruction.Address] = applyMask(memoryInstruction.Value);
                        break;

                    default:
                        throw new Exception($"Illegal instruction '{instruction?.GetType().ToString() ?? "null"}'.");
                }
            }

            return memory;

            ulong applyMask(ulong value)
            {
                var result = value;
                for (int i = 0; i < mask.Length; i++)
                {
                    var bit = mask.Length - i - 1;

                    var c = mask[i];
                    switch (c)
                    {
                        case 'X':
                            break;

                        case '1':
                            result |= 1uL << bit;
                            break;

                        case '0':
                            result &= ~(1uL << bit);
                            break;

                        default:
                            throw new Exception($"Illegal mask char '{c}'.");
                    }
                }

                return result;
            }
        }

        abstract record Instruction();
        record MaskInstruction(string Mask) : Instruction;
        record MemoryInstruction(ulong Address, ulong Value) : Instruction;
    }
}
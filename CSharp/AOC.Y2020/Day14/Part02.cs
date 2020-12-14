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

    public static class Part02
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
                        ExecuteMemoryInstruction(memoryInstruction, mask, memory);
                        break;

                    default:
                        throw new Exception($"Illegal instruction '{instruction?.GetType().ToString() ?? "null"}'.");
                }
            }

            return memory;
        }

        private static void ExecuteMemoryInstruction(MemoryInstruction instruction, string mask, Dictionary<ulong, ulong> memory)
        {
            var floatingBits = (int)Math.Pow(2, mask.Count(c => c == 'X'));
            Span<ulong> addresses = stackalloc ulong[floatingBits];
            CalculateAddresses(instruction.Address, mask, addresses);

            foreach (var address in addresses)
            {
                memory[address] = instruction.Value;
            }
        }

        private static void CalculateAddresses(ulong address, string mask, Span<ulong> addresses)
        {
            // Assume addresses.Length == 2^mask.Count('X');

            for (int i = addresses.Length - 1; i >= 0; i--)
            {
                addresses[i] = address;

                int floatingIndex = 0;
                for (int j = mask.Length - 1; j >= 0; j--)
                {
                    var c = mask[j];
                    var bit = mask.Length - j - 1;

                    switch (c)
                    {
                        case 'X':
                            var floatingVal = i & (1 << floatingIndex);
                            if (floatingVal == 0)
                                addresses[i] &= ~(1uL << bit);
                            else
                                addresses[i] |= 1uL << bit;

                            floatingIndex++;
                            break;

                        case '1':
                            addresses[i] |= 1uL << bit;
                            break;

                        case '0':
                            break;

                        default:
                            throw new Exception($"Illegal mask char '{c}'.");
                    }
                }
            }
        }

        abstract record Instruction();
        record MaskInstruction(string Mask) : Instruction;
        record MemoryInstruction(ulong Address, ulong Value) : Instruction;
    }
}
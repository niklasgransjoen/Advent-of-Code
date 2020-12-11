using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day08
{
    /**
      * https://adventofcode.com/2020/day/8
      */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var instructions = ParseInstructions(input);

            var result = RepairAndExecute(instructions);
            AOCUtils.PrintResult(result);
        }

        private static Instruction[] ParseInstructions(string[] input)
        {
            var result = new Instruction[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = ParseInstruction(input[i]);
            }

            return result;
        }

        private static Instruction ParseInstruction(string rawInstruction)
        {
            var match = Regex.Match(rawInstruction, @"^(\w+) ([\+\-])(\d+)$");
            if (!match.Success)
                throw new Exception($"Failed to parse instruction '{rawInstruction}'.");

            var opcode = Enum.Parse<OpCode>(match.Groups[1].Value, ignoreCase: true);
            var sign = match.Groups[2].Value == "+" ? +1 : -1;
            var unsignedArgument = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            return new Instruction(opcode, sign * unsignedArgument);
        }

        private static int RepairAndExecute(Instruction[] instructions)
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                var current = instructions[i];
                if (current.OpCode == OpCode.ACC)
                    continue;

                instructions[i] = new Instruction(current.OpCode == OpCode.NOP ? OpCode.JMP : OpCode.NOP, current.Argument);
                if (TryExecute(instructions, out var acc))
                    return acc;

                instructions[i] = current;
            }

            throw new Exception($"Failed to repair the boot code.");
        }

        private static bool TryExecute(Instruction[] instructions, out int acc)
        {
            acc = 0;
            var index = 0;
            var executedInstructions = new HashSet<int>();

            while (index < instructions.Length)
            {
                if (!executedInstructions.Add(index))
                {
                    return false;
                }

                var instruction = instructions[index];
                switch (instruction.OpCode)
                {
                    case OpCode.ACC:
                        acc += instruction.Argument;
                        index++;
                        break;

                    case OpCode.JMP:
                        index += instruction.Argument;
                        break;

                    case OpCode.NOP:
                        index++;
                        break;

                    default:
                        throw new Exception($"Invalid opcode '{instruction.OpCode}'.");
                }
            }

            return true;
        }

        #region Util classes

        private enum OpCode
        {
            ACC,
            JMP,
            NOP,
        }

        private readonly struct Instruction
        {
            public Instruction(OpCode opCode, int argument)
            {
                OpCode = opCode;
                Argument = argument;
            }

            public OpCode OpCode { get; }
            public int Argument { get; }
        }

        #endregion Util classes
    }
}
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day12
{
    /**
      * https://adventofcode.com/2020/day/12
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var instructions = ParseInstructions(input);
            var pos = CalculatePosition(instructions);

            var result = Math.Abs(pos.X) + Math.Abs(pos.Y);
            AOCUtils.PrintResult(result);
        }

        private static Instruction[] ParseInstructions(string[] input)
        {
            var result = new Instruction[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                var line = input[i];

                var match = Regex.Match(line, @"^(\w)(\d+)$");
                if (!match.Success)
                    throw new Exception($"Failed to parse input '{line}'.");

                var rawAction = match.Groups[1].Value[0];
                var action = rawAction switch
                {
                    'N' => InstructionAction.MoveNorth,
                    'S' => InstructionAction.MoveSouth,
                    'E' => InstructionAction.MoveEast,
                    'W' => InstructionAction.MoveWest,
                    'F' => InstructionAction.MoveForward,
                    'R' => InstructionAction.RotateRight,
                    'L' => InstructionAction.RotateLeft,
                    _ => throw new Exception($"Invalid action character '{rawAction}'."),
                };

                var value = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                result[i] = new Instruction(action, value);
            }

            return result;
        }

        private static Point CalculatePosition(Instruction[] instructions)
        {
            const int DirEast = 0;
            const int DirSouth = 1 * 90;
            const int DirWest = 2 * 90;
            const int DirNorth = 3 * 90;

            int x = 0;
            int y = 0;
            var dir = 0;

            foreach (var instruction in instructions)
            {
                switch (instruction.Action)
                {
                    case InstructionAction.MoveNorth:
                        moveDirection(DirNorth, instruction.Value);
                        break;

                    case InstructionAction.MoveSouth:
                        moveDirection(DirSouth, instruction.Value);
                        break;

                    case InstructionAction.MoveEast:
                        moveDirection(DirEast, instruction.Value);
                        break;

                    case InstructionAction.MoveWest:
                        moveDirection(DirWest, instruction.Value);
                        break;

                    case InstructionAction.MoveForward:
                        moveDirection(dir, instruction.Value);
                        break;

                    case InstructionAction.RotateRight:
                        dir = (dir + instruction.Value) % 360;
                        break;

                    case InstructionAction.RotateLeft:
                        dir = (dir - instruction.Value + 360) % 360;
                        break;

                    default:
                        throw new Exception($"Illegal action '{instruction.Action}'.");
                }
            }

            return new(x, y);

            void moveDirection(int dir, int distance)
            {
                switch (dir)
                {
                    case DirNorth:
                        y += distance;
                        break;

                    case DirSouth:
                        y -= distance;
                        break;

                    case DirEast:
                        x += distance;
                        break;

                    case DirWest:
                        x -= distance;
                        break;

                    default:
                        throw new Exception($"Invalid direction '{dir}'.");
                }
            }
        }

        private enum InstructionAction
        {
            MoveNorth,
            MoveSouth,
            MoveEast,
            MoveWest,
            MoveForward,
            RotateRight,
            RotateLeft,
        }

        private readonly struct Instruction
        {
            public Instruction(InstructionAction action, int value)
            {
                Action = action;
                Value = value;
            }

            public InstructionAction Action { get; }
            public int Value { get; }
        }

        private readonly struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }
    }
}
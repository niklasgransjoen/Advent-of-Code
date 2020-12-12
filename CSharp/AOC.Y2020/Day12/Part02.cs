using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day12
{
    /**
      * https://adventofcode.com/2020/day/12
      */

    public static class Part02
    {
        private static readonly Point _initialWaypoint = new Point(10, 1);

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
            int x = 0;
            int y = 0;
            var waypoint = _initialWaypoint;

            foreach (var instruction in instructions)
            {
                switch (instruction.Action)
                {
                    case InstructionAction.MoveNorth:
                        waypoint = new Point(waypoint.X, waypoint.Y + instruction.Value);
                        break;

                    case InstructionAction.MoveSouth:
                        waypoint = new Point(waypoint.X, waypoint.Y - instruction.Value);
                        break;

                    case InstructionAction.MoveEast:
                        waypoint = new Point(waypoint.X + instruction.Value, waypoint.Y);
                        break;

                    case InstructionAction.MoveWest:
                        waypoint = new Point(waypoint.X - instruction.Value, waypoint.Y);
                        break;

                    case InstructionAction.MoveForward:
                        x += waypoint.X * instruction.Value;
                        y += waypoint.Y * instruction.Value;
                        break;

                    case InstructionAction.RotateRight:
                        rotateWaypoint(instruction.Value / 90);
                        break;

                    case InstructionAction.RotateLeft:
                        rotateWaypoint((360 - instruction.Value) / 90);
                        break;

                    default:
                        throw new Exception($"Illegal action '{instruction.Action}'.");
                }
            }

            return new(x, y);

            // Rotate the waypoint 90 degrees clockwise a given amount of times.
            void rotateWaypoint(int steps)
            {
                for (int i = 0; i < steps; i++)
                {
                    waypoint = new Point(waypoint.Y, -waypoint.X);
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
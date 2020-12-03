using AOC.Resources;
using System;

namespace AOC.Y2015.Day06
{
    /**
     * https://adventofcode.com/2015/day/6
     */

    public static class Part02
    {
        private enum LightAction
        {
            On,
            Off,
            Toggle,
        }

        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day06);
            Instruction[] instructions = ParseInput(input);

            var lights = InitializeLights(instructions);
            int totalBrightness = CalculateTotalBrightness(lights);

            General.PrintResult(totalBrightness);
        }

        private static Instruction[] ParseInput(string[] input)
        {
            Instruction[] instructions = new Instruction[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                ParseInputLine(input[i], out var corner1, out var corner2, out var actionStr);

                ParseCommaSeparatedPoint(corner1, out int x1, out int y1);
                ParseCommaSeparatedPoint(corner2, out int x2, out int y2);
                LightAction action = actionStr.Contains("turn off", StringComparison.InvariantCulture) ? LightAction.Off :
                    actionStr.Contains("turn on", StringComparison.InvariantCulture) ? LightAction.On : LightAction.Toggle;

                instructions[i] = new Instruction(x1, y1, x2 - x1 + 1, y2 - y1 + 1, action);
            }

            return instructions;
        }

        private static void ParseInputLine(ReadOnlySpan<char> input, out ReadOnlySpan<char> corner1, out ReadOnlySpan<char> corner2, out ReadOnlySpan<char> actionStr)
        {
            corner1 = string.Empty;
            corner2 = string.Empty;
            actionStr = string.Empty;

            byte state = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (state == 0 && char.IsNumber(input[i]))
                {
                    actionStr = input.Slice(0, i - 1);
                    state++;
                }
                else if (state == 1 && input[i] == ' ')
                {
                    corner1 = input.Slice(actionStr.Length, i - actionStr.Length);
                    state++;
                }
                else if (state == 2 && char.IsNumber(input[i]))
                {
                    corner2 = input.Slice(i);
                    return;
                }
            }
        }

        private static void ParseCommaSeparatedPoint(ReadOnlySpan<char> point, out int x, out int y)
        {
            int commaPos = point.IndexOf(',');
            var xStr = point.Slice(0, commaPos);
            var yStr = point.Slice(commaPos + 1);

            x = int.Parse(xStr);
            y = int.Parse(yStr);
        }

        private static int[,] InitializeLights(Instruction[] instructions)
        {
            int[,] lights = new int[1000, 1000];

            foreach (var instruction in instructions)
            {
                int increment = instruction.LightAction == LightAction.Off ? -1 : instruction.LightAction == LightAction.On ? 1 : 2;
                for (int x = instruction.X; x < instruction.X + instruction.Width; x++)
                {
                    for (int y = instruction.Y; y < instruction.Y + instruction.Height; y++)
                    {
                        lights[x, y] = Math.Max(0, lights[x, y] + increment);
                    }
                }
            }

            return lights;
        }

        private static int CalculateTotalBrightness(int[,] lights)
        {
            int totalBrightness = 0;
            for (int i = 0; i < lights.GetLength(0); i++)
            {
                for (int j = 0; j < lights.GetLength(1); j++)
                {
                    totalBrightness += lights[i, j];
                }
            }

            return totalBrightness;
        }

        private readonly struct Instruction
        {
            public Instruction(int x, int y, int width, int height, LightAction lightAction)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                LightAction = lightAction;
            }

            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }
            public LightAction LightAction { get; }
        }
    }
}
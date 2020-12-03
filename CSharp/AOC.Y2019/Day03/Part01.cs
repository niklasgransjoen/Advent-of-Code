using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AOC.Y2019.Day03
{
    /**
     * https://adventofcode.com/2019/day/3
     */

    public static class Part01
    {
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left,
        }

        public static void Exec(AOCContext context)
        {
            string[] input = context.GetInputLines();

            // Create the line definitions.
            var line1 = ParseInput(input[0]);
            var line2 = ParseInput(input[1]);

            // Find extreme values.
            CalculatePathMaxValues(line1, out int line1MinX, out int line1MinY, out int line1MaxX, out int line1MaxY);
            CalculatePathMaxValues(line2, out int line2MinX, out int line2MinY, out int line2MaxX, out int line2MaxY);

            int minX = Math.Min(line1MinX, line2MinX);
            int minY = Math.Min(line1MinY, line2MinY);
            int maxX = Math.Max(line1MaxX, line2MaxX) + 1;
            int maxY = Math.Max(line1MaxY, line2MaxY) + 1;

            // Find size of area, offset to zero.
            int width = maxX - minX;
            int height = maxY - minY;

            int originX = -minX;
            int originY = -minY;

            /**
             * Use byte to be able to represent at least three values:
             * - No path
             * - First path
             * - Second path
             * - Crossing
             */
            byte[,] map = new byte[width, height];

            const byte signature1 = 1;
            const byte signature2 = 2;
            const byte originSignature = 9;

            map[originX, originY] = originSignature;
            WritePath(map, line1, originX, originY, signature1);
            WritePath(map, line2, originX, originY, signature2);

            //PrintMap(map);
            //Console.ReadKey();

            int result = FindIntersectionDistance(map, originX, originY, signature1 + signature2);
            AOCUtils.PrintResult(result);
        }

        /// <summary>
        /// Parses the input into an array of path components. Format D[Length], where D is direction.
        /// </summary>
        private static PathComponent[] ParseInput(string input)
        {
            string[] componentStrings = input.Split(',');
            PathComponent[] components = new PathComponent[componentStrings.Length];
            for (int i = 0; i < componentStrings.Length; i++)
            {
                string componentString = componentStrings[i];

                Direction direction = ParseDirection(componentString[0]);
                int length = int.Parse(componentString.AsSpan().Slice(1));
                components[i] = new PathComponent(direction, length);
            }

            return components;
        }

        private static Direction ParseDirection(char c)
        {
            return c switch
            {
                'U' => Direction.Up,
                'L' => Direction.Left,
                'D' => Direction.Down,
                'R' => Direction.Right,

                _ => throw new Exception("Invalid direction"),
            };
        }

        private static void CalculatePathMaxValues(PathComponent[] path, out int minX, out int minY, out int maxX, out int maxY)
        {
            minX = 0;
            minY = 0;
            maxX = 0;
            maxY = 0;

            int x = 0, y = 0;

            foreach (var component in path)
            {
                switch (component.Direction)
                {
                    case Direction.Up:
                        y += component.Length;
                        if (y > maxY) maxY = y;
                        break;

                    case Direction.Right:
                        x += component.Length;
                        if (x > maxX) maxX = x;
                        break;

                    case Direction.Down:
                        y -= component.Length;
                        if (y < minY) minY = y;
                        break;

                    case Direction.Left:
                        x -= component.Length;
                        if (x < minX) minX = x;
                        break;
                }
            }
        }

        private static void WritePath(byte[,] map, PathComponent[] path, int originX, int originY, byte signature)
        {
            int x = originX;
            int y = originY;
            foreach (var component in path)
            {
                switch (component.Direction)
                {
                    case Direction.Up:
                        for (int i = 0; i < component.Length; i++)
                            writeSignature(map, x, ++y, signature);
                        break;

                    case Direction.Right:
                        for (int i = 0; i < component.Length; i++)
                            writeSignature(map, ++x, y, signature);
                        break;

                    case Direction.Down:
                        for (int i = 0; i < component.Length; i++)
                            writeSignature(map, x, --y, signature);
                        break;

                    case Direction.Left:
                        for (int i = 0; i < component.Length; i++)
                            writeSignature(map, --x, y, signature);
                        break;

                    default:
                        break;
                }
            }

            static void writeSignature(byte[,] map, int x, int y, byte signature)
            {
                byte currentValue = map[x, y];
                if (currentValue == signature)
                    return;

                if (currentValue == 0)
                    map[x, y] = signature;
                else
                    map[x, y] += signature;
            }
        }

        private static void PrintMap(byte[,] map)
        {
            for (int y = map.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    char c = map[x, y] switch
                    {
                        1 => '1',
                        2 => '2',
                        3 => '@',
                        9 => 'O',
                        _ => '.',
                    };
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        private static int FindIntersectionDistance(byte[,] map, int originX, int originY, int signature)
        {
            Direction direction = Direction.Up;
            int length = 1;

            int x = originX;
            int y = originY;

            int width = map.GetLength(0);
            int height = map.GetLength(1);

            /**
             * Very dirty way of finding the intersection closest to the origin:
             * Find all intersections, and then return the smallest one.
             */
            List<int> intersectionDistances = new List<int>();
            while (true)
            {
                if (length > width + 1 &&
                    length > height + 1)
                    break;

                for (int i = 0; i <= length; i++)
                {
                    x += getXIncrement(direction);
                    y += getYIncrement(direction);

                    if (x < 0 || x >= width || y < 0 || y >= height)
                        continue;

                    if (map[x, y] == signature)
                    {
                        // All distances are positive values.
                        int result = Math.Abs(x - originX) + Math.Abs(y - originY);
                        intersectionDistances.Add(result);
                    }
                }

                direction = getNextDirection(direction);
                if (direction == Direction.Down || direction == Direction.Up)
                {
                    length++;
                }
            }

            return intersectionDistances.Min();

            static int getXIncrement(Direction direction)
            {
                return direction switch
                {
                    Direction.Right => 1,
                    Direction.Left => -1,

                    _ => 0,
                };
            }

            static int getYIncrement(Direction direction)
            {
                return direction switch
                {
                    Direction.Up => 1,
                    Direction.Down => -1,

                    _ => 0,
                };
            }

            static Direction getNextDirection(Direction direction)
            {
                if (direction == Direction.Left)
                    return Direction.Up;

                return direction + 1;
            }
        }

        /// <summary>
        /// Represents a component of a bigger path.
        /// </summary>
        [DebuggerDisplay("{Direction} {Length}")]
        private struct PathComponent
        {
            public PathComponent(Direction direction, int length)
            {
                Direction = direction;
                Length = length;
            }

            public Direction Direction { get; }
            public int Length { get; }
        }
    }
}
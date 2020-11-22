using AOC.Resources;
using AOC2019.Day17.P01;
using System;
using System.Collections.Generic;

namespace AOC2019.Day17
{
    public static class Part02
    {
        private enum Tile
        {
            Scaffold,
            Space,
            Robot,
        }

        public static void Exec()
        {
            string[] input = General.ReadCSVInput(Day.Day17);
            long[] intcode = General.StringToLong(input);

            Communicator communicator = new Communicator();
            IntcodeInterpreter interpreter = new IntcodeInterpreter(intcode, communicator);
            interpreter.Execute();

            Tile[][] tiles = ParseCommunicatorOutput(communicator.Output);
            int result = CalculateResult(tiles);
            General.PrintResult(result);
        }

        private static Tile[][] ParseCommunicatorOutput(List<char> output)
        {
            int width = output.IndexOf('\n');
            output.RemoveAll(c => c == '\n');

            int height = output.Count / width;

            Tile[][] tiles = ArrayUtility.Create2D<Tile>(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y][x] = CharToTile(output[y * width + x]);
                }
            }

            return tiles;
        }

        private static Tile CharToTile(char c)
        {
            return c switch
            {
                '#' => Tile.Scaffold,
                '.' => Tile.Space,

                _ => Tile.Robot,
            };
        }

        private static int CalculateResult(Tile[][] tiles)
        {
            (int robotX, int robotY) = LocateRobot(tiles);

            byte[][] visitationMap = ArrayUtility.Create2D<byte>(tiles[0].Length, tiles.Length);
            visitationMap[robotY][robotX]++;

            // Initialize visitation map by following scaffolds.
            int x = robotX;
            int y = robotY;
            while (true)
            {
                // Locate next scaffold
                var (deltaX, deltaY) = FindDirection(tiles, visitationMap, x, y);
                if (deltaX == 0 && deltaY == 0)
                    break;

                // Mark off scaffold
                do
                {
                    x += deltaX;
                    y += deltaY;

                    visitationMap[y][x]++;

                    // check bounds
                    if (y == 0 || y == visitationMap.Length - 1 || x == 0 || x == visitationMap[0].Length - 1)
                        break;

                    // check tile type
                    if (tiles[y + deltaY][x + deltaX] != Tile.Scaffold)
                        break;
                } while (true);
            }

            // Find the actual result from the crossings.
            int result = 0;
            for (y = 0; y < visitationMap.Length; y++)
            {
                for (x = 0; x < visitationMap[0].Length; x++)
                {
                    if (visitationMap[y][x] != 2)
                        continue;

                    result += x * y;
                }
            }

            return result;
        }

        private static (int x, int y) LocateRobot(Tile[][] tiles)
        {
            for (int y = 0; y < tiles.Length; y++)
            {
                for (int x = 0; x < tiles[y].Length; x++)
                {
                    if (tiles[y][x] == Tile.Robot)
                        return (x, y);
                }
            }

            throw new Exception("Failed to locate robot");
        }

        private static (int deltaX, int deltaY) FindDirection(Tile[][] tiles, byte[][] visitationMap, int x, int y)
        {
            for (int deltaY = -1; deltaY <= 1; deltaY++)
            {
                for (int deltaX = -1; deltaX <= 1; deltaX++)
                {
                    // Don't allow diagonal directions.
                    if (!(deltaY == 0 ^ deltaX == 0))
                        continue;

                    // Check bounds - y
                    if ((y == 0 && deltaY == -1) || (y == tiles.Length - 1 && deltaY == 1))
                        continue;

                    // Check bounds - x
                    if ((x == 0 && deltaX == -1) || (x == tiles[0].Length - 1 && deltaX == 1))
                        continue;

                    int x1 = x + deltaX;
                    int y1 = y + deltaY;

                    if (tiles[y1][x1] == Tile.Scaffold && visitationMap[y1][x1] == 0)
                    {
                        return (deltaX, deltaY);
                    }
                }
            }

            return default;
        }

        private sealed class Communicator : IIOPort
        {
            public List<char> Output { get; } = new List<char>();

            public IOReadResult Read()
            {
                throw new NotImplementedException();
            }

            public void RegisterInterpreterForInput(IInterpreter interpreter)
            {
                throw new NotImplementedException();
            }

            public void Write(long value)
            {
                Output.Add((char)value);
            }
        }
    }
}
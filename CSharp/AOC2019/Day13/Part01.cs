using AOC.Resources;
using AOC2019.Day13.P01;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2019.Day13
{
    /**
     * https://adventofcode.com/2019/day/13
     */

    public static class Part01
    {
        public static void Exec()
        {
            string[] input = General.ReadCSVInput(Day.Day13);
            long[] intcode = General.StringToLong(input);

            ArcadeCabinet cabinet = new ArcadeCabinet();
            IntcodeInterpreter interpreter = new IntcodeInterpreter(intcode, cabinet);
            if (!interpreter.Execute())
                General.PrintError("Intcode interpreter returned before halting.");
            else
            {
                const int blockID = 2;
                long result = cabinet.Screen.GetTileCount(blockID);
                General.PrintResult(result);
            }
        }

        private sealed class ArcadeCabinet : IIOPort
        {
            private readonly List<long> _outputBuffer = new List<long>();

            public ArcadeCabinet()
            {
            }

            public ArcadeScreen Screen { get; } = new ArcadeScreen();

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
                const int paramCount = 3;
                if (_outputBuffer.Count < paramCount - 1)
                {
                    _outputBuffer.Add(value);
                }
                else
                {
                    long x = _outputBuffer[0];
                    long y = _outputBuffer[1];
                    int tileID = (int)value;

                    Screen.DrawTile(x, y, tileID);

                    _outputBuffer.Clear();
                }
            }
        }

        private sealed class ArcadeScreen
        {
            private int[][] _buffer = Array.Empty<int[]>();

            public void DrawTile(long x, long y, int tileID)
            {
                // Expand direction y if needed.
                if (y >= _buffer.Length)
                {
                    int[][] oldBuffer = _buffer;

                    _buffer = new int[y + 1][];
                    oldBuffer.CopyTo(_buffer, 0);
                }

                // Create, expand direction x if needed.
                if (_buffer[y] is null)
                    _buffer[y] = new int[x + 1];
                else if (x >= _buffer[y].Length)
                {
                    int[] oldBuffer = _buffer[y];

                    _buffer[y] = new int[x + 1];
                    oldBuffer.CopyTo(_buffer[y], 0);
                }

                _buffer[y][x] = tileID;
            }

            public long GetTileCount(int tileID)
            {
                return _buffer.Sum(rowBuffer => rowBuffer is null ? 0 : rowBuffer.Count(id => id == tileID));
            }
        }
    }
}
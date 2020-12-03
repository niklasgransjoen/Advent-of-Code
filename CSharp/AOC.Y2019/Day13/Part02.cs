using AOC.Y2019.Day13.P02;
using System;
using System.Collections.Generic;

namespace AOC.Y2019.Day13
{
    /**
     * https://adventofcode.com/2019/day/13
     */

    public static class Part02
    {
        private const int EmptyID = 0;
        private const int WallID = 1;
        private const int BlockID = 2;
        private const int PaddleID = 3;
        private const int BallID = 4;

        public static void Exec(AOCContext context)
        {
            string[] input = context.GetCSVInput();
            long[] intcode = AOCUtils.StringToLong(input);

            // Trick game into believing it got two quarters.
            intcode[0] = 2;

            while (true)
            {
                Console.ResetColor();
                Console.Clear();

                ArcadeCabinet cabinet = new ArcadeCabinet();
                IntcodeInterpreter interpreter = new IntcodeInterpreter(intcode, cabinet);
                if (!interpreter.Execute())
                {
                    AOCUtils.PrintError("Intcode interpreter returned before halting.");
                    return;
                }

                Console.ResetColor();
                Console.Clear();

                AOCUtils.WriteLine("Game over", ConsoleColor.DarkCyan);
                AOCUtils.WriteLine($"Final Player Score: {cabinet.PlayerScore:N0}", ConsoleColor.Magenta);
                AOCUtils.WriteLine("Press ENTER to play again", ConsoleColor.Green);
                Console.ReadLine();
            }
        }

        private sealed class ArcadeCabinet : IIOPort
        {
            private readonly List<int> _outputBuffer = new List<int>();
            private int _paddlePosition;
            private int _ballPosition;

            public ArcadeCabinet()
            {
                Console.CursorVisible = false;
            }

            public ArcadeScreen Screen { get; } = new ArcadeScreen();

            public long PlayerScore { get; private set; }

            public IOReadResult Read()
            {
                //int keyID = ReadUserKey();
                int keyID = CalculateKey();

                return new IOReadResult(keyID);
            }

            private int ReadUserKey()
            {
                var keyInfo = Console.ReadKey(intercept: true);
                return keyInfo.Key switch
                {
                    ConsoleKey.A => -1,
                    ConsoleKey.S => 0,
                    ConsoleKey.D => 1,
                    ConsoleKey.Spacebar => CalculateKey(),

                    _ => ReadUserKey(), // there's probably no chance for stackoverflow
                };
            }

            private int CalculateKey()
            {
                return Math.Sign(_ballPosition - _paddlePosition);
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
                    _outputBuffer.Add((int)value);
                    return;
                }

                int x = _outputBuffer[0];
                int y = _outputBuffer[1];

                // Check if output is player score, and not write to screen.
                if (x == -1 && y == 0)
                {
                    PlayerScore = value;
                    Screen.DrawPlayerScore(value);
                }
                else
                {
                    int tileID = (int)value;
                    Screen.DrawPixel(x, y, tileID);

                    // We want to store some values.
                    if (tileID == PaddleID)
                        _paddlePosition = x;
                    else if (tileID == BallID)
                        _ballPosition = x;
                }

                _outputBuffer.Clear();
            }
        }

        private sealed class ArcadeScreen
        {
            private const int ScreenOffset = 2;

            public void DrawPlayerScore(long value)
            {
                Console.SetCursorPosition(0, 0);
                AOCUtils.Write($"Player score: {value:N0}", ConsoleColor.Magenta);
            }

            public void DrawPixel(int x, int y, int tileID)
            {
                Console.SetCursorPosition(x, y + ScreenOffset);

                switch (tileID)
                {
                    case EmptyID:
                        DrawPixel(' ', ConsoleColor.Black, ConsoleColor.Black);
                        break;

                    case WallID:
                        DrawPixel('#', ConsoleColor.Black, ConsoleColor.Cyan);
                        break;

                    case BlockID:
                        DrawPixel('*', ConsoleColor.Black, ConsoleColor.Green);
                        break;

                    case PaddleID:
                        DrawPixel('Y', ConsoleColor.Yellow, ConsoleColor.Red);
                        break;

                    case BallID:
                        DrawPixel('o', ConsoleColor.Yellow, ConsoleColor.Red);
                        break;

                    default:
                        DrawPixel('X', ConsoleColor.DarkRed, ConsoleColor.Yellow);
                        break;
                }
            }

            private static void DrawPixel(char c, ConsoleColor background, ConsoleColor foreground)
            {
                Console.BackgroundColor = background;
                Console.ForegroundColor = foreground;

                Console.Write(c);
            }
        }
    }
}
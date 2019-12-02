using AOC.Resources;
using System;
using System.IO;

namespace AOC2019
{
    internal class Program
    {
        private static int Main()
        {
            AOCEngine engine = new AOCEngine();
            return engine.Run(Day.Day01);
        }
    }

    public sealed class AOCEngine : AOCFramework
    {
        public AOCEngine()
        {
            General.SetInput(Path.GetFullPath(@"..\..\..\Input"));
        }

        public override int Run(Day day)
        {
            switch (day)
            {
                case Day.Day01:
                    Day01.Part01.Exec();
                    Day01.Part02.Exec();
                    break;

                case Day.Day02:
                    break;

                case Day.Day03:
                    break;

                case Day.Day04:
                    break;

                case Day.Day05:
                    break;

                case Day.Day06:
                    break;

                case Day.Day07:
                    break;

                case Day.Day08:
                    break;

                case Day.Day09:
                    break;

                case Day.Day10:
                    break;

                case Day.Day11:
                    break;

                case Day.Day12:
                    break;

                case Day.Day13:
                    break;

                case Day.Day14:
                    break;

                case Day.Day15:
                    break;

                case Day.Day16:
                    break;

                case Day.Day17:
                    break;

                case Day.Day18:
                    break;

                case Day.Day19:
                    break;

                case Day.Day20:
                    break;

                case Day.Day21:
                    break;

                case Day.Day22:
                    break;

                case Day.Day23:
                    break;

                case Day.Day24:
                    break;
            }

            Console.WriteLine("Day not available.");
            return -1;
        }
    }
}
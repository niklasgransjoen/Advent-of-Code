using System;
using System.IO;
using System.Reflection;

namespace AOC.Resources
{
    public enum Days
    {
        Day01,
        Day02,
        Day03,
        Day04,
        Day05,
        Day06,
        Day07,
        Day08,
        Day09,
        Day10,
        Day11,
        Day12,
        Day13,
        Day14,
        Day15,
        Day16,
        Day17,
        Day18,
        Day19,
        Day20,
        Day21,
        Day22,
        Day23,
        Day24,
    }

    public static class General
    {
        public static string[] ReadInput(Days day)
        {
            var assembly = Assembly.GetCallingAssembly();
            string path = assembly.Location + @"\..\..\..\..\..\input\" + day.ToString() + ".txt";
            return File.ReadAllLines(path);
        }

        public static int[] ReadIntegerInput(Days day)
        {
            string[] input = ReadInput(day);
            int[] intInput = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
                intInput[i] = int.Parse(input[i]);

            return intInput;
        }

        public static string ReadSingleLineInput(Days day)
        {
            string[] lines = ReadInput(day);
            if (lines.Length == 0)
                throw new Exception("Input file had no lines!");
            if (lines.Length > 1)
                throw new Exception("Input file had more than one line!");

            return lines[0];
        }

        public static void PrintResult<T>(T result)
        {
            PrintResult("The answer is", result);
        }

        public static void PrintResult<T>(string caption, T result)
        {
            Console.Write(caption);
            Console.WriteLine(": {0}", result);

            Console.ReadKey();
        }
    }
}
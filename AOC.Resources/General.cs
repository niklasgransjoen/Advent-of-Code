using System;
using System.IO;

namespace AOC.Resources
{
    public static class General
    {
        private static string Filepath { get; set; }

        public static void SetInput(string filepath)
        {
            Filepath = filepath;
        }

        public static string[] ReadInput(Day day)
        {
            string path = Filepath + "\\" + day.ToString() + ".txt";
            return File.ReadAllLines(path);
        }

        public static int[] ReadIntegerInput(Day day)
        {
            string[] input = ReadInput(day);
            return ParseStrings(input);
        }

        public static string ReadSingleLineInput(Day day)
        {
            string[] lines = ReadInput(day);
            if (lines.Length == 0)
                throw new Exception("Input file had no lines!");
            if (lines.Length > 1)
                throw new Exception("Input file had more than one line!");

            return lines[0];
        }

        public static string[] ReadCSVInput(Day day)
        {
            string input = ReadSingleLineInput(day);
            return input.Split(',');
        }

        public static int[] ParseStrings(string[] values)
        {
            int[] intInput = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
                intInput[i] = int.Parse(values[i]);

            return intInput;
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
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
            string path = GetPath(day);
            return File.ReadAllLines(path);
        }

        public static int[] ReadIntegerInput(Day day)
        {
            string[] input = ReadInput(day);
            return StringToInt(input);
        }

        public static string ReadSingleLineInput(Day day)
        {
            string path = GetPath(day);
            return File.ReadAllText(path);
        }

        public static string[] ReadCSVInput(Day day)
        {
            string input = ReadSingleLineInput(day);
            return input.Split(',');
        }

        public static int[] StringToInt(Span<string> values)
        {
            int[] intInput = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
                intInput[i] = int.Parse(values[i]);

            return intInput;
        }

        public static long[] StringToLong(Span<string> values)
        {
            long[] intInput = new long[values.Length];
            for (int i = 0; i < values.Length; i++)
                intInput[i] = long.Parse(values[i]);

            return intInput;
        }

        public static void PrintResult<T>(T result)
        {
            PrintResult("The answer is", result);
        }

        public static void PrintResult<T>(ReadOnlySpan<char> caption, T result)
        {
            for (int i = 0; i < caption.Length; i++)
                Console.Write(caption[i]);

            Console.WriteLine(": {0}", result);

            Console.ReadKey();
        }

        #region Helpers

        private static string GetPath(Day day)
        {
            return Filepath + "\\" + day.ToString() + ".txt";
        }

        #endregion Helpers
    }
}
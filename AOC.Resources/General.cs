using System;
using System.Diagnostics;
using System.IO;

namespace AOC.Resources
{
    public static class General
    {
        #region IO

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

        #endregion IO

        #region Type conversion

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

        #endregion Type conversion

        #region Console

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

        public static void PrintResult<T>(ReadOnlySpan<char> caption, T result)
        {
            Console.Out.Write(caption);
            Console.WriteLine(": {0}", result);
            Console.ReadKey();
        }

        public static void PrintError(string errorMessage)
        {
            WriteLine(errorMessage, ConsoleColor.DarkRed);
            Console.ReadKey();
        }

        public static void PrintError(ReadOnlySpan<char> errorMessage)
        {
            WriteLine(errorMessage, ConsoleColor.DarkRed);
            Console.ReadKey();
        }

        public static void Write(string value, ConsoleColor foreground)
        {
            ConsoleColor oldForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;

            Console.Write(value);

            Console.ForegroundColor = oldForeground;
        }

        public static void Write(ReadOnlySpan<char> value, ConsoleColor foreground)
        {
            ConsoleColor oldForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;

            Console.Out.Write(value);

            Console.ForegroundColor = oldForeground;
        }

        public static void Write<T>(T value, ConsoleColor foreground)
        {
            ConsoleColor oldForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;

            Console.Write(value);

            Console.ForegroundColor = oldForeground;
        }

        public static void WriteLine(string value, ConsoleColor foreground)
        {
            Write(value, foreground);
            Console.WriteLine();
        }

        public static void WriteLine(ReadOnlySpan<char> value, ConsoleColor foreground)
        {
            Write(value, foreground);
            Console.WriteLine();
        }

        public static void WriteLine<T>(T value, ConsoleColor foreground)
        {
            Write(value, foreground);
            Console.WriteLine();
        }

        #endregion Console

        #region Debug

        private static readonly Lazy<Stopwatch> _stopwatch = new Lazy<Stopwatch>(() => new Stopwatch());

        public static void StartTimer()
        {
            _stopwatch.Value.Start();
            WriteLine("Timer started", ConsoleColor.DarkCyan);
        }

        public static void StopTimer()
        {
            Stopwatch sw = _stopwatch.Value;

            sw.Stop();
            WriteLine("Timer stopped", ConsoleColor.DarkCyan);
            WriteLine($"Time measured: {sw.ElapsedMilliseconds} ms", ConsoleColor.DarkCyan);
        }

        #endregion Debug

        #region Helpers

        private static string GetPath(Day day)
        {
            return Filepath + "\\" + day.ToString() + ".txt";
        }

        #endregion Helpers
    }
}
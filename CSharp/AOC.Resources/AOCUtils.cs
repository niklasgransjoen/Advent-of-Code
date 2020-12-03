using System;
using System.Diagnostics;

namespace AOC
{
    public static class AOCUtils
    {
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
        }

        public static void PrintResult<T>(ReadOnlySpan<char> caption, T result)
        {
            Console.Out.Write(caption);
            Console.WriteLine(": {0}", result);
        }

        public static void PrintError(string errorMessage)
        {
            WriteLine(errorMessage, ConsoleColor.DarkRed);
        }

        public static void PrintError(ReadOnlySpan<char> errorMessage)
        {
            WriteLine(errorMessage, ConsoleColor.DarkRed);
        }

        public static void Write(string value, ConsoleColor foreground)
        {
            var oldForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;

            Console.Write(value);

            Console.ForegroundColor = oldForeground;
        }

        public static void Write(ReadOnlySpan<char> value, ConsoleColor foreground)
        {
            var oldForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;

            Console.Out.Write(value);

            Console.ForegroundColor = oldForeground;
        }

        public static void Write<T>(T value, ConsoleColor foreground)
        {
            var oldForeground = Console.ForegroundColor;
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
            var sw = _stopwatch.Value;

            sw.Stop();
            WriteLine("Timer stopped", ConsoleColor.DarkCyan);
            WriteLine($"Time measured: {sw.ElapsedMilliseconds} ms", ConsoleColor.DarkCyan);
        }

        #endregion Debug
    }
}
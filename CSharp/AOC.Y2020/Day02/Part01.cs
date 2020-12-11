using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day02
{
    /**
      * https://adventofcode.com/2020/day/2
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var rawInput = context.GetInputLines();
            var input = Parse(rawInput);
            var validPasswords = GetValidPasswords(input);

            AOCUtils.PrintResult("The number of valid passwords are", validPasswords.Count());
        }

        private static PasswordLine[] Parse(string[] lines)
        {
            var regex = new Regex(@"(\d+)\-(\d+) (\w): (\w+)");

            var result = new PasswordLine[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var match = regex.Match(line);
                if (!match.Success)
                {
                    throw new Exception($"Failed to parse expression '{line}'.");
                }

                var rawMin = match.Groups[1].Value;
                var rawMax = match.Groups[2].Value;
                var rawLetter = match.Groups[3].Value;
                var password = match.Groups[4].Value;

                var min = int.Parse(rawMin, CultureInfo.InvariantCulture);
                var max = int.Parse(rawMax, CultureInfo.InvariantCulture);
                var letter = rawLetter.Single();

                result[i] = new PasswordLine(min, max, letter, password);
            }

            return result;
        }

        private static IEnumerable<PasswordLine> GetValidPasswords(PasswordLine[] lines)
        {
            foreach (var line in lines)
            {
                int charCount = line.Password.Count(c => c == line.Letter);
                if (charCount >= line.Min && charCount <= line.Max)
                {
                    yield return line;
                }
            }
        }

        private readonly struct PasswordLine
        {
            public PasswordLine(int min, int max, char letter, string password)
            {
                Min = min;
                Max = max;
                Letter = letter;
                Password = password;
            }

            public int Min { get; }
            public int Max { get; }
            public char Letter { get; }
            public string Password { get; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day02
{
    public static class Part02
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

                var rawPos1 = match.Groups[1].Value;
                var rawPos2 = match.Groups[2].Value;
                var rawLetter = match.Groups[3].Value;
                var password = match.Groups[4].Value;

                var pos1 = int.Parse(rawPos1, CultureInfo.InvariantCulture);
                var pos2 = int.Parse(rawPos2, CultureInfo.InvariantCulture);
                var letter = rawLetter.Single();

                result[i] = new PasswordLine(pos1, pos2, letter, password);
            }

            return result;
        }

        private static IEnumerable<PasswordLine> GetValidPasswords(PasswordLine[] lines)
        {
            foreach (var line in lines)
            {
                var password = line.Password;
                var letter = line.Letter;

                // Indexing starts at 1.
                var p1 = line.Pos1 - 1;
                var p2 = line.Pos2 - 1;

                if (password[p1] == letter ^ password[p2] == letter)
                    yield return line;
            }
        }

        private readonly struct PasswordLine
        {
            public PasswordLine(int pos1, int pos2, char letter, string password)
            {
                Pos1 = pos1;
                Pos2 = pos2;
                Letter = letter;
                Password = password;
            }

            public int Pos1 { get; }
            public int Pos2 { get; }
            public char Letter { get; }
            public string Password { get; }
        }
    }
}
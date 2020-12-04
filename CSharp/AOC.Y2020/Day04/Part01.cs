using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day04
{
    public static class Part01
    {
        private static readonly ImmutableArray<string> _requiredFields = new[] {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid",
        }.ToImmutableArray();

        public static void Exec(AOCContext context)
        {
            var passports = ParseData(context.Input);
            var validPassports = GetValid(passports);

            AOCUtils.PrintResult("Number of valid passports", validPassports.Count());
        }

        private static Passport[] ParseData(string input)
        {
            var regex = new Regex(@"(\w+):(.*)");

            var result = new List<Passport>
            {
                new Passport(),
            };

            var entries = input.Split(new[] { "\r\n", "\r", "\n", " " }, StringSplitOptions.None);
            foreach (var entry in entries)
            {
                if (string.IsNullOrWhiteSpace(entry))
                {
                    result.Add(new Passport());
                }
                else
                {
                    var match = regex.Match(entry);
                    if (!match.Success)
                        throw new Exception($"Input '{entry}' could not be parsed.");

                    var current = result[^1];
                    current[match.Groups[1].Value] = match.Groups[2].Value;
                }
            }

            return result.ToArray();
        }

        private static IEnumerable<Passport> GetValid(Passport[] passports)
        {
            foreach (var passport in passports)
            {
                if (_requiredFields.All(rf => passport.HasField(rf)))
                {
                    yield return passport;
                }
            }
        }

        private sealed class Passport
        {
            private readonly Dictionary<string, string> _fields = new();

            public string this[string field]
            {
                get => _fields[field];
                set => _fields[field] = value;
            }

            public bool HasField(string field) => _fields.ContainsKey(field);
        }
    }
}
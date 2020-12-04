using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day04
{
    public static class Part02
    {
        private static readonly ImmutableDictionary<string, Func<string, bool>> _requiredFields = new Dictionary<string, Func<string, bool>> {
            { "byr", PassportValidation.ValidateBirthYear },
            { "iyr", PassportValidation.ValidateIssueYear },
            { "eyr", PassportValidation.ValidateExpirationYear },
            { "hgt", PassportValidation.ValidateHeight },
            { "hcl", PassportValidation.ValidateHairColor},
            { "ecl", PassportValidation.ValidateEyeColor },
            { "pid", PassportValidation.ValidatePassportID },
        }.ToImmutableDictionary();

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

        private static IEnumerable<Passport> GetValid(Passport[] passports) => passports
            .Where(p => _requiredFields.Keys.All(field => p.HasField(field)))
            .Where(p => _requiredFields.All(rf =>
            {
                var field = rf.Key;
                var fieldValue = p[field];
                var validator = rf.Value;

                return validator(fieldValue);
            }));

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

        private static class PassportValidation
        {
            public static bool ValidateBirthYear(string value) => ValidateYear(value, 1920, 2002);

            public static bool ValidateIssueYear(string value) => ValidateYear(value, 2010, 2020);

            public static bool ValidateExpirationYear(string value) => ValidateYear(value, 2020, 2030);

            public static bool ValidateHeight(string value)
            {
                var match = Regex.Match(value, @"^(\d+)(cm|in)$");
                if (!match.Success)
                    return false;

                int height = int.Parse(match.Groups[1].Value);
                return match.Groups[2].Value switch
                {
                    "cm" => height >= 150 && height <= 193,
                    _ => height >= 59 && height <= 76,
                };
            }

            public static bool ValidateHairColor(string value)
            {
                return Regex.IsMatch(value, @"^#[0-9a-f]{6}$");
            }

            public static bool ValidateEyeColor(string value)
            {
                return value is "amb" or
                                "blu" or
                                "brn" or
                                "gry" or
                                "grn" or
                                "hzl" or
                                "oth";
            }

            public static bool ValidatePassportID(string value)
            {
                return Regex.IsMatch(value, @"^\d{9}$");
            }

            #region Utilities

            private static bool ValidateYear(string year, int min, int max)
            {
                if (Regex.IsMatch(year, @"^\d{4}$"))
                {
                    var yearInt = int.Parse(year);
                    return yearInt >= min && yearInt <= max;
                }
                else
                    return false;
            }

            #endregion Utilities
        }
    }
}
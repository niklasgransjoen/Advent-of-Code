using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day16
{
    /**
      * https://adventofcode.com/2020/day/16
      */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            var rawInput = context.GetInputLines();
            var input = ParseInput(rawInput);
            var orderedRules = FindOrderedRules(input);

            var result = orderedRules
                .Zip(input.Ticket.Fields, (rule, field) => (rule, field)).Where(pair => pair.rule.StartsWith("departure", StringComparison.Ordinal))
                .Aggregate(1L, (product, pair) => product * pair.field);
            AOCUtils.PrintResult(result);
        }

        private static PuzzleInput ParseInput(string[] input)
        {
            var rules = new List<Rule>();
            var otherTickets = new List<Ticket>();

            // Rules
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                    break;

                var match = Regex.Match(line, @"^([\w ]+): (\d+)-(\d+) or (\d+).(\d+)$");
                if (!match.Success)
                    throw new Exception($"Invalid input '{line}'.");

                var ruleName = match.Groups[1].Value;
                var min1 = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var max1 = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var min2 = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);
                var max2 = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                var range = new Range(min1, max1, min2, max2);
                rules.Add(new(ruleName, range));
            }

            // Ticket
            var ticketStart = input.TakeWhile(line => line != "your ticket:").Count() + 1;
            var ticket = parseTicket(input[ticketStart]);

            // Other tickets
            var otherTicketsStart = input.TakeWhile(line => line != "nearby tickets:").Count() + 1;
            foreach (var line in input.Skip(otherTicketsStart))
            {
                otherTickets.Add(parseTicket(line));
            }

            return new PuzzleInput(rules.ToArray(), ticket, otherTickets.ToArray());

            static Ticket parseTicket(string line)
            {
                var rawFields = line.Split(',');
                var fields = AOCUtils.StringToInt(rawFields);
                return new(fields);
            }
        }

        private static string[] FindOrderedRules(PuzzleInput input)
        {
            var validInput = FilterOutInvalidTickets(input);

            var unresolvedRules = validInput.Rules.ToList();
            var unresolvedFields = Enumerable.Range(0, unresolvedRules.Count).ToList();

            var matchedRules = new List<Rule>();

            var result = new string[unresolvedFields.Count];

            while (unresolvedRules.Count > 0)
            {
                for (int i = 0; i < unresolvedFields.Count; i++)
                {
                    var row = unresolvedFields[i];

                    MatchRules(validInput.OtherTickets.Select(ot => ot.Fields[row]), unresolvedRules, matchedRules);

                    if (matchedRules.Count == 0)
                        throw new Exception($"Row {row} of fields was not matched by any rules!");
                    else if (matchedRules.Count == 1)
                    {
                        // This row only matched a single rule, so that must be the correct one.
                        var matchedRule = matchedRules[0];

                        result[row] = matchedRule.Name;
                        unresolvedFields.Remove(row);
                        unresolvedRules.Remove(matchedRule);

                        // We removed an item, so decrement i to compensate.
                        i--;
                    }
                }
            }

            return result;
        }

        private static PuzzleInput FilterOutInvalidTickets(PuzzleInput input)
        {
            var invalidTickets = new List<Ticket>();
            foreach (var ticket in input.OtherTickets)
            {
                foreach (var field in ticket.Fields)
                {
                    if (!ValidateField(field, input.Rules.Select(r => r.Range)))
                    {
                        invalidTickets.Add(ticket);
                        break;
                    }
                }
            }

            return input with
            {
                OtherTickets = input.OtherTickets.Except(invalidTickets).ToArray(),
            };

            static bool ValidateField(int field, IEnumerable<Range> ranges)
            {
                foreach (var range in ranges)
                {
                    if (field >= range.Min1 && field <= range.Max1 ||
                        field >= range.Min2 && field <= range.Max2)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static void MatchRules(IEnumerable<int> values, IEnumerable<Rule> rules, List<Rule> matches)
        {
            matches.Clear();

            foreach (var rule in rules)
            {
                bool isValid = true;
                foreach (var value in values)
                {
                    isValid &= ValidateField(value, rule.Range);
                    if (!isValid)
                        break;
                }

                if (isValid)
                    matches.Add(rule);
            }
        }

        private static bool ValidateField(int field, Range range)
        {
            if (field >= range.Min1 && field <= range.Max1 ||
                field >= range.Min2 && field <= range.Max2)
            {
                return true;
            }

            return false;
        }

        record Range(int Min1, int Max1, int Min2, int Max2);
        record Rule(string Name, Range Range);
        record Ticket(int[] Fields);
        record PuzzleInput(Rule[] Rules, Ticket Ticket, Ticket[] OtherTickets);
    }
}
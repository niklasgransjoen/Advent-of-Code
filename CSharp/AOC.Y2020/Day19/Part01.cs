using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AOC.Y2020.Day19
{
    /**
      * https://adventofcode.com/2020/day/19
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var rawInput = context.GetInputLines();
            var input = ParseInput(rawInput);
            var result = FindMatches(input);

            AOCUtils.PrintResult("The number of messages matching rule 0 is", result);
        }

        private static PuzzleInput ParseInput(string[] input)
        {
            var rules = new List<Rule>();
            var messages = new List<string>();

            // Rules
            foreach (var line in input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)))
            {
                Rule rule;

                var idIndexEnd = line.IndexOf(':');
                var id = int.Parse(line[0..idIndexEnd], CultureInfo.InvariantCulture);
                if (line.Contains('a'))
                    rule = new LiteralRule(id, 'a');
                else if (line.Contains('b'))
                    rule = new LiteralRule(id, 'b');
                else
                {
                    var referencePairs = line[(idIndexEnd + 1)..^0].Split('|');
                    var referencedRules = referencePairs[0]
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(r => int.Parse(r, CultureInfo.InvariantCulture))
                        .ToArray();
                    var optionalAlternativeRules = referencePairs.Length == 1 ?
                        null :
                        referencePairs[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                         .Select(r => int.Parse(r, CultureInfo.InvariantCulture))
                                         .ToArray();

                    rule = new CompositeRule(id, referencedRules, optionalAlternativeRules);
                }

                rules.Add(rule);
            }

            // Data
            foreach (var message in input.Reverse().TakeWhile(line => !string.IsNullOrWhiteSpace(line)))
            {
                messages.Add(message);
            }

            return new(rules.ToArray(), messages.ToArray());
        }

        private static ulong FindMatches(PuzzleInput input)
        {
            var count = 0uL;

            var ruleMap = input.Rules.ToDictionary(r => r.ID);
            var ruleZero = ruleMap[0];

            foreach (var message in input.Message)
            {
                if (tryMatch(message, ruleMap) == message.Length)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Returns the number of consumed characters.
        /// </summary>
        private static int tryMatch(ReadOnlySpan<char> message, Dictionary<int, Rule> rules, int currentRule = 0)
        {
            var r = rules[currentRule];
            if (r is LiteralRule literalRule)
                return message[0] == literalRule.C ? 1 : 0;
            if (r is not CompositeRule compositeRule)
                throw new Exception($"Invalid type of rule");

            var consumedCharacters = MatchComposite(message, rules, compositeRule.Rules);
            if (consumedCharacters == 0)
                consumedCharacters = MatchComposite(message, rules, compositeRule.OptionalAlternativeRules);

            return consumedCharacters;

            static int MatchComposite(ReadOnlySpan<char> message, Dictionary<int, Rule> rules, int[]? matchingRules)
            {
                if (matchingRules is null)
                    return 0;

                var messageIndex = 0;
                foreach (var matchingRule in matchingRules)
                {
                    if (messageIndex >= message.Length)
                    {
                        return 0;
                    }

                    var consumedChars = tryMatch(message[messageIndex..^0], rules, matchingRule);
                    if (consumedChars == 0)
                        return 0;

                    messageIndex += consumedChars;
                }

                return messageIndex;
            }
        }

        record PuzzleInput(Rule[] Rules, string[] Message);

        abstract record Rule(int ID);
        record LiteralRule(int ID, char C) : Rule(ID);
        record CompositeRule(int ID, int[] Rules, int[]? OptionalAlternativeRules = null) : Rule(ID);
    }
}
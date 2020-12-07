using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day07
{
    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var rules = ParseInput(input);

            var result = new HashSet<BagRule>();
            GetAncestors(rules, "shiny gold", result);

            AOCUtils.PrintResult(result.Count);
        }

        private static BagRule[] ParseInput(string[] input)
        {
            var parsedBags = new (string color, string rawContents)[input.Length];

            // Parse colors.
            for (int i = 0; i < input.Length; i++)
            {
                var match = Regex.Match(input[i], @"(^.*) bags contain (.*)$");
                if (!match.Success)
                    throw new Exception($"Failed to parse color from input '{input}'.");

                var color = match.Groups[1].Value;
                var rawContents = match.Groups[2].Value;

                parsedBags[i] = (color, rawContents);
            }

            // Parse contents.
            var bagsByColors = parsedBags.ToDictionary(pair => pair.color, pair => new BagRule(pair.color));
            foreach (var (color, rawContents) in parsedBags)
            {
                if (!bagsByColors.TryGetValue(color, out var bag))
                    throw new Exception($"Bag color '{color}' could not be resolved.");

                parseBagContents(bag, rawContents);
            }

            return bagsByColors.Values.ToArray();

            void parseBagContents(BagRule bag, string rawContents)
            {
                if (rawContents == "no other bags.")
                    return;

                var splitRawContents = rawContents.Split(',').Select(content => content.Trim());
                foreach (var rawContent in splitRawContents)
                {
                    var match = Regex.Match(rawContent, @"^(\d+) (.*) bag");
                    if (!match.Success)
                        throw new Exception($"Failed to parse bag contents '{rawContent}'.");

                    var contentColor = match.Groups[2].Value;

                    if (!bagsByColors.TryGetValue(contentColor, out var contentBag))
                        throw new Exception($"Bag color '{contentColor}' could not be resolved.");

                    bag.Contents.Add(contentBag);
                }
            }
        }

        private static bool GetAncestors(IEnumerable<BagRule> rules, string color, HashSet<BagRule> result)
        {
            var isAncestor = false;
            foreach (var rule in rules)
            {
                if (rule.Color == color)
                    isAncestor |= true;
                else if (GetAncestors(rule.Contents, color, result))
                {
                    result.Add(rule);
                    isAncestor |= true;
                }
            }

            return isAncestor;
        }

        private sealed class BagRule
        {
            public BagRule(string color)
            {
                Color = color;
            }

            public string Color { get; }
            public HashSet<BagRule> Contents { get; } = new();
        }
    }
}
using AOC.Y2019.Day12.P02;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2019.Day12
{
    /**
     * https://adventofcode.com/2019/day/12
     *
     * Solved with a hint from the stream of https://twitter.com/lizthegrey
     */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            string[] input = context.GetInputLines();
            Moon[] moons = ParseInput(input);
            InitializeMoons(moons);

            // Returns one array per moon containing three elements representing (x, y, z)
            int[] orbitSteps = CalculateOrbits(moons);

            long result = CalculateBackToOrigin(orbitSteps);
            AOCUtils.PrintResult(result);
        }

        private static Moon[] ParseInput(string[] input)
        {
            Moon[] moons = new Moon[input.Length];
            for (int i = 0; i < moons.Length; i++)
            {
                string line = input[i];

                Regex regex = new Regex(@"(?<=\=)\-?[0-9]+");
                var matches = regex.Matches(line);

                int x = int.Parse(matches[0].Value);
                int y = int.Parse(matches[1].Value);
                int z = int.Parse(matches[2].Value);

                Vector moonPosition = new Vector(x, y, z);
                moons[i] = new Moon(moonPosition);
            }

            return moons;
        }

        private static void InitializeMoons(Moon[] moons)
        {
            // Create lists of neighbors.
            List<Moon>[] neighborMoons = new List<Moon>[moons.Length];
            for (int i = 0; i < neighborMoons.Length; i++) neighborMoons[i] = new List<Moon>();

            // Init lists of neighbors.
            for (int i = 0; i < moons.Length - 1; i++)
            {
                for (int j = i + 1; j < moons.Length; j++)
                {
                    neighborMoons[i].Add(moons[j]);
                    neighborMoons[j].Add(moons[i]);
                }
            }

            // Init moons.
            for (int i = 0; i < moons.Length; i++)
            {
                moons[i].Initialize(neighborMoons[i]);
            }
        }

        private static int[] CalculateOrbits(Moon[] moons)
        {
            Vector[] positions = moons.Select(m => m.Position).ToArray();

            int[] result = new int[3];
            int steps = 0;
            while (true)
            {
                for (int i = 0; i < moons.Length; i++)
                    moons[i].Interact();

                for (int i = 0; i < moons.Length; i++)
                    moons[i].Step();

                steps++;
                if (result[0] == 0 &&
                    moons.All(m => m.Velocity.X == 0) &&
                    moons.Select(m => m.Position.X).SequenceEqual(positions.Select(p => p.X)))
                {
                    result[0] = steps;
                }

                if (result[1] == 0 &&
                    moons.All(m => m.Velocity.Y == 0) &&
                    moons.Select(m => m.Position.Y).SequenceEqual(positions.Select(p => p.Y)))
                {
                    result[1] = steps;
                }

                if (result[2] == 0 &&
                    moons.All(m => m.Velocity.Z == 0) &&
                    moons.Select(m => m.Position.Z).SequenceEqual(positions.Select(p => p.Z)))
                {
                    result[2] = steps;
                }

                bool complete = result.All(r => r > 0);
                if (complete)
                    return result;
            }
        }

        private static long CalculateBackToOrigin(int[] orbitSteps)
        {
            Dictionary<int, int> divisors = new Dictionary<int, int>();

            for (int i = 0; i < orbitSteps.Length; i++)
            {
                List<int> divs = FindDivisors(orbitSteps[i]);
                var groupedDivisors = divs.GroupBy(div => div, (div, values) => new KeyValuePair(div, values.Count()));

                foreach (var div in groupedDivisors)
                {
                    if (!divisors.TryGetValue(div.Key, out int divisorCount) || divisorCount < div.Value)
                        divisors[div.Key] = div.Value;
                }
            }

            long result = 1;
            foreach (var divisor in divisors)
            {
                result *= (int)Math.Pow(divisor.Key, divisor.Value);
            }

            return result;
        }

        private static List<int> FindDivisors(int number)
        {
            List<int> divisors = new List<int>();

            int divisor = 2;
            while (number > 1)
            {
                if (number % divisor == 0)
                {
                    number /= divisor;
                    divisors.Add(divisor);
                }
                else
                {
                    divisor++;
                }
            }

            return divisors;
        }

        private readonly struct KeyValuePair
        {
            public KeyValuePair(int key, int value)
            {
                Key = key;
                Value = value;
            }

            public int Key { get; }
            public int Value { get; }
        }
    }
}
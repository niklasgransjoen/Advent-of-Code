using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dim1 = System.Collections.Generic.Dictionary<int, bool>;
using Dim2 = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, bool>>;
using Dim3 = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, bool>>>;

namespace AOC.Y2020.Day17
{
    /**
      * https://adventofcode.com/2020/day/17
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var state = ParseInput(input);
            Simulate(state, cycles: 6);

            var activeCells = state
                .SelectMany(s => s.Value)
                .SelectMany(s => s.Value)
                .Where(s => s.Value)
                .Count();

            AOCUtils.PrintResult("Active cells", activeCells);
        }

        private static Dim3 ParseInput(string[] input)
        {
            var dim2 = new Dim2();
            for (int lineIndex = 0; lineIndex < input.Length; lineIndex++)
            {
                var line = input[lineIndex];

                var dim1 = new Dim1();
                dim2.Add(lineIndex, dim1);

                for (int charIndex = 0; charIndex < line.Length; charIndex++)
                {
                    var c = line[charIndex];
                    dim1[charIndex] = c switch
                    {
                        '#' => true,
                        '.' => false,
                        _ => throw new Exception($"Invalid character input '{c}' (line {lineIndex + 1}, character {charIndex + 1}."),
                    };
                }
            }

            return new Dim3()
            {
                { 0, dim2 },
            };
        }

        private static void Simulate(Dim3 state, int cycles)
        {
            var pool = new DimPool();
            for (int i = 0; i < cycles; i++)
            {
                Simulate(state, pool);

                //Console.WriteLine($"Generation {i + 1}");
                //PrettyPrint(state);
            }
        }

        private static void Simulate(Dim3 state, DimPool pool)
        {
            pool.ResetAll();

            // Simulate next state

            /*
             * We iterate over all the active states,
             * and then consider the inactive neigbours of these as well.
             */

            var nextState = pool.RentDim3();
            foreach (var dim2s in state)
            {
                if (!nextState.TryGetValue(dim2s.Key, out var nextDim2))
                {
                    nextDim2 = pool.RentDim2();
                    nextState.Add(dim2s.Key, nextDim2);
                }

                foreach (var dim1s in dim2s.Value)
                {
                    if (!nextDim2.TryGetValue(dim1s.Key, out var nextDim1))
                    {
                        nextDim1 = pool.RentDim1();
                        nextDim2.Add(dim1s.Key, nextDim1);
                    }

                    foreach (var valuePair in dim1s.Value)
                    {
                        if (!valuePair.Value)
                        {
                            // Cell is inactive.
                            continue;
                        }

                        // Cell is active. Consider it's state.
                        var activeNeighbors = CountActiveNeighbors(state, valuePair.Key, dim1s.Key, dim2s.Key);
                        nextDim1.Add(valuePair.Key, activeNeighbors is 2 or 3);

                        // Then consider its inactive neighbors states.
                        foreach (var (x, y, z) in EnumerateNeighbors(valuePair.Key, dim1s.Key, dim2s.Key)
                                                                    .Where(addr => Lookup(state, addr.x, addr.y, addr.z).isActive is not true))
                        {
                            var (dim2, dim1, isActive) = Lookup(nextState, x, y, z);

                            // Skip neighbors that have already been calculated.
                            if (isActive.HasValue)
                                continue;

                            if (dim2 is null)
                            {
                                dim2 = pool.RentDim2();
                                nextState.Add(z, dim2);
                            }

                            if (dim1 is null)
                            {
                                dim1 = pool.RentDim1();
                                dim2.Add(y, dim1);
                            }

                            activeNeighbors = CountActiveNeighbors(state, x, y, z);
                            dim1.Add(x, activeNeighbors == 3);
                        }
                    }
                }
            }

            // Copy next state over to current state
            CopyState(state, nextState);
        }

        private static int CountActiveNeighbors(Dim3 state, int x, int y, int z)
        {
            var neighbors = 0;
            foreach (var (xi, yi, zi) in EnumerateNeighbors(x, y, z))
            {
                if (Lookup(state, xi, yi, zi).isActive is true)
                    neighbors++;
            }

            return neighbors;
        }

        private static (Dim2? dim2, Dim1? dim1, bool? isActive) Lookup(Dim3 state, int x, int y, int z)
        {
            if (!state.TryGetValue(z, out var dim2))
                return default;

            if (!dim2.TryGetValue(y, out var dim1))
                return (dim2, null, null);

            if (!dim1.TryGetValue(x, out var isActive))
                return (dim2, dim1, null);

            return (dim2, dim1, isActive);
        }

        private static IEnumerable<(int x, int y, int z)> EnumerateNeighbors(int x, int y, int z)
        {
            for (int zi = z - 1; zi <= z + 1; zi++)
            {
                for (int yi = y - 1; yi <= y + 1; yi++)
                {
                    for (int xi = x - 1; xi <= x + 1; xi++)
                    {
                        if (zi == z && yi == y && xi == x)
                            continue;

                        yield return (xi, yi, zi);
                    }
                }
            }
        }

        private static void CopyState(Dim3 state, Dim3 nextState)
        {
            foreach (var dim2s in nextState)
            {
                if (!state.TryGetValue(dim2s.Key, out var currentDim2))
                {
                    currentDim2 = new();
                    state.Add(dim2s.Key, currentDim2);
                }

                foreach (var dim1s in dim2s.Value)
                {
                    if (!currentDim2.TryGetValue(dim1s.Key, out var currentDim1))
                    {
                        currentDim1 = new();
                        currentDim2.Add(dim1s.Key, currentDim1);
                    }

                    currentDim1.Clear();
                    foreach (var valuePair in dim1s.Value)
                    {
                        currentDim1.Add(valuePair.Key, valuePair.Value);
                    }
                }
            }
        }

        private static void PrettyPrint(Dim3 state)
        {
            var str = ToPrettyString(state);
            Console.WriteLine(str);
        }

        private static string ToPrettyString(Dim3 state)
        {
            if (!state.Any() || !state.SelectMany(s => s.Value).Any() || !state.SelectMany(s => s.Value).SelectMany(s => s.Value).Any())
            {
                return "No alive cells";
            }

            var minZ = state.Min(s => s.Key);
            var maxZ = state.Max(s => s.Key);

            var minY = state.SelectMany(s => s.Value).Min(s => s.Key);
            var maxY = state.SelectMany(s => s.Value).Max(s => s.Key);

            var minX = state.SelectMany(s => s.Value).SelectMany(s => s.Value).Min(s => s.Key);
            var maxX = state.SelectMany(s => s.Value).SelectMany(s => s.Value).Max(s => s.Key);

            var sb = new StringBuilder();
            for (int z = minZ; z <= maxZ; z++)
            {
                sb.AppendLine($"z={z}");

                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        if (Lookup(state, x, y, z).isActive is true)
                            sb.Append('#');
                        else
                            sb.Append('.');
                    }
                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private class DimPool
        {
            private readonly List<Dim1> _dim1s = new();
            private int _dim1Index = 0;

            private readonly List<Dim2> _dim2s = new();
            private int _dim2Index = 0;

            private readonly List<Dim3> _dim3s = new();
            private int _dim3Index = 0;

            public Dim1 RentDim1()
            {
                if (_dim1s.Count == _dim1Index)
                {
                    _dim1s.Add(new());
                }

                var next = _dim1s[_dim1Index];
                _dim1Index++;

                next.Clear();

                return next;
            }

            public Dim2 RentDim2()
            {
                if (_dim2s.Count == _dim2Index)
                {
                    _dim2s.Add(new());
                }

                var next = _dim2s[_dim2Index];
                _dim2Index++;

                next.Clear();

                return next;
            }

            public Dim3 RentDim3()
            {
                if (_dim3s.Count == _dim3Index)
                {
                    _dim3s.Add(new());
                }

                var next = _dim3s[_dim3Index];
                _dim3Index++;

                next.Clear();

                return next;
            }

            public void ResetAll()
            {
                _dim1Index = 0;
                _dim2Index = 0;
                _dim3Index = 0;
            }
        }
    }
}
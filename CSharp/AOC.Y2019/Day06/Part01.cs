using AOC.Resources;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AOC.Y2019.Day06
{
    /**
     * https://adventofcode.com/2019/day/6
     */

    public static class Part01
    {
        private const char Separator = ')';
        private const string RootName = "COM";

        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day06);
            Dictionary<string, string> childParentMap = CreateChildParentMap(input);
            Satellite satellite = CreateSatteliteMap(childParentMap, RootName);

            int result = satellite.GetOrbitCount(parents: 0);
            General.PrintResult(result);
        }

        /// <summary>
        /// Creates a mapping between children and parents.
        /// </summary>
        private static Dictionary<string, string> CreateChildParentMap(string[] input)
        {
            Dictionary<string, string> childParentMap = new Dictionary<string, string>();
            foreach (var line in input)
            {
                string[] pair = line.Split(Separator);
                childParentMap[pair[1]] = pair[0];
            }

            return childParentMap;
        }

        private static Satellite CreateSatteliteMap(Dictionary<string, string> childParentMap, string parentName)
        {
            var childNames = childParentMap.Where(m => m.Value == parentName).Select(m => m.Key).ToArray();

            // Satellite has no children.
            if (childNames.Length == 0)
                return new Satellite(parentName, Enumerable.Empty<Satellite>());

            Satellite[] children = new Satellite[childNames.Length];
            for (int i = 0; i < childNames.Length; i++)
            {
                children[i] = CreateSatteliteMap(childParentMap, childNames[i]);
            }

            Satellite satellite = new Satellite(parentName, children);
            foreach (var child in children)
            {
                child.Parent = satellite;
            }

            return satellite;
        }

        [DebuggerDisplay("{Name} (Children: {ChildCount})")]
        private sealed class Satellite
        {
            private readonly IReadOnlyList<Satellite> _children;

            public Satellite(string name, IEnumerable<Satellite> children)
            {
                Name = name;
                _children = children.ToList().AsReadOnly();
            }

            public string Name { get; }
            public Satellite? Parent { get; set; }
            public IEnumerable<Satellite> Children => _children;
            public int ChildCount => _children.Count;

            /// <summary>
            /// Returns the number of orbits of this satellite and all of its children.
            /// </summary>
            /// <param name="parents">The number of parents (direct and indirect) over this satellite.</param>
            public int GetOrbitCount(int parents)
            {
                int orbits = parents;
                foreach (var child in Children)
                    orbits += child.GetOrbitCount(parents + 1);

                return orbits;
            }
        }
    }
}
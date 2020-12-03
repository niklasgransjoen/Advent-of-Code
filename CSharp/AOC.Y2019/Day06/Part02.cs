using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AOC.Y2019.Day06
{
    /**
     * https://adventofcode.com/2019/day/6
     */

    public static class Part02
    {
        private const char Separator = ')';
        private const string RootName = "COM";
        private const string CurrentPosition = "YOU";
        private const string Santa = "SAN";

        public static void Exec(AOCContext context)
        {
            string[] input = context.GetInputLines();
            Dictionary<string, string> childParentMap = CreateChildParentMap(input);
            Satellite satellite = CreateSatteliteMap(childParentMap, RootName);

            var currentPath = satellite.FindSatellitePath(CurrentPosition);
            var santaPath = satellite.FindSatellitePath(Santa);

            if (currentPath is null) Console.WriteLine("Failed to locate " + CurrentPosition);
            if (santaPath is null) Console.WriteLine("Failed to locate " + Santa);
            if (currentPath is null || santaPath is null)
            {
                Console.ReadKey();
                return;
            }

            var shortestPath = FindShortestPath(currentPath.ToArray(), santaPath.ToArray());
            int result = shortestPath.Length;
            AOCUtils.PrintResult(result);
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

        private static string[] FindShortestPath(string[] currentPosition, string[] targetPosition)
        {
            int index;
            int maxCount = Math.Min(currentPosition.Length, targetPosition.Length);
            for (index = 0; index < maxCount; index++)
            {
                if (currentPosition[index] != targetPosition[index])
                    break;
            }

            string[] result = new string[currentPosition.Length + targetPosition.Length - index * 2];

            int i = 0;
            foreach (var node in currentPosition.Skip(index).Reverse())
            {
                result[i] = node;
                i++;
            }

            foreach (var node in targetPosition.Skip(index))
            {
                result[i] = node;
                i++;
            }

            return result;
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

            public IEnumerable<string>? FindSatellitePath(string name)
            {
                List<string> paths = new List<string>();
                FindSatellitePath(name, paths);

                return paths.Count > 0 ? Enumerable.Reverse(paths) : null;
            }

            private bool FindSatellitePath(string name, List<string> paths)
            {
                if (name == Name)
                    return true;

                foreach (var child in Children)
                {
                    var subPath = child.FindSatellitePath(name, paths);
                    if (subPath)
                    {
                        paths.Add(Name);
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
using AOC.Resources;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2018.Day08
{
    /*
   * https://adventofcode.com/2018/day/8
   */

    public static class Part02
    {
        public static void Exec()
        {
            string input = General.ReadSingleLineInput(Day.Day08);
            //string input = General.GetLineInput();
            List<int> parsedInput = ParseInput(input);
            Node node = BuildTree(parsedInput);
            int result = node.GetValue();

            General.PrintResult("The value of the root is", result);
        }

        private static List<int> ParseInput(string input)
        {
            List<int> parsedInput = new List<int>();
            foreach (string entry in input.Split(' '))
                parsedInput.Add(int.Parse(entry));

            return parsedInput;
        }

        /// <summary>
        /// Builds the tree recursively.
        /// </summary>
        private static Node BuildTree(List<int> input)
        {
            int index = 0;
            return BuildTree(input, ref index);
        }

        /// <summary>
        /// Builds the tree recursively.
        /// </summary>
        private static Node BuildTree(List<int> input, ref int index)
        {
            Node node = new Node();

            int childrenCount = input[index++];
            int metadataCount = input[index++];

            for (int i = 0; i < childrenCount; i++)
                node.Children.Add(BuildTree(input, ref index));

            for (int i = 0; i < metadataCount; i++)
                node.Metadata.Add(input[index++]);

            return node;
        }

        private sealed class Node
        {
            public List<Node> Children { get; } = new List<Node>();
            public List<int> Metadata { get; } = new List<int>();

            /// <summary>
            /// Returns the value of this node.
            /// </summary>
            public int GetValue()
            {
                if (Children.Any())
                {
                    int value = 0;
                    foreach (int data in Metadata)
                    {
                        if (data > 0 && data <= Children.Count)
                            value += Children[data - 1].GetValue();
                    }

                    return value;
                }
                else return SumMetadata();
            }

            /// <summary>
            /// Returns the sum of the metadata of this node and all of its children.
            /// </summary>
            private int SumMetadata()
            {
                int data = Metadata.Sum();
                foreach (Node node in Children)
                    data += node.SumMetadata();

                return data;
            }
        }
    }
}
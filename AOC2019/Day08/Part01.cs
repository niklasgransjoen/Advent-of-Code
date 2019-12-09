using AOC.Resources;
using System.Collections.Generic;
using System.Linq;

namespace AOC2019.Day08
{
    /**
     * https://adventofcode.com/2019/day/8
     */

    public static class Part01
    {
        private const int ImageWidth = 25;
        private const int ImageHeight = 6;

        public static void Exec()
        {
            string input = General.ReadSingleLineInput(Day.Day08);
            Layer[] layers = ParseInput(input);

            Layer minLayer = LocateLayerWithFewestZeroes(layers);

            int ones = minLayer.GetCharCount('1');
            int twos = minLayer.GetCharCount('2');

            int result = ones * twos;
            General.PrintResult(result);
        }

        private static Layer[] ParseInput(string input)
        {
            int layerCount = input.Length / (ImageWidth * ImageHeight);
            Layer[] layers = new Layer[layerCount];

            for (int i = 0; i < layerCount; i++)
            {
                string[] rows = new string[ImageHeight];
                for (int j = 0; j < ImageHeight; j++)
                {
                    int offset = i * ImageWidth * ImageHeight + j * ImageWidth;
                    rows[j] = input.Substring(offset, ImageWidth);
                }

                layers[i] = new Layer(rows);
            }

            return layers;
        }

        private static Layer LocateLayerWithFewestZeroes(Layer[] layers)
        {
            Layer minLayer = layers[0];
            int minZeroCount = minLayer.GetCharCount('0');
            for (int i = 1; i < layers.Length; i++)
            {
                int currentZeroCount = layers[i].GetCharCount('0');
                if (currentZeroCount < minZeroCount)
                {
                    minLayer = layers[i];
                    minZeroCount = currentZeroCount;
                }
            }

            return minLayer;
        }

        private sealed class Layer
        {
            public Layer(IReadOnlyList<string> rows)
            {
                Rows = rows;
            }

            public IReadOnlyList<string> Rows { get; }

            /// <summary>
            /// Returns the number of zeros in this layer.
            /// </summary>
            public int GetCharCount(char character)
            {
                return Rows.Sum(r => r.Count(c => c == character));
            }
        }
    }
}
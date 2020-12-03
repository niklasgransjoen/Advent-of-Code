using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2019.Day08
{
    /**
     * https://adventofcode.com/2019/day/8
     */

    public static class Part02
    {
        private const int ImageWidth = 25;
        private const int ImageHeight = 6;

        public static void Exec()
        {
            string input = General.ReadSingleLineInput(Day.Day08);
            Layer[] layers = ParseInput(input);
            byte[][] image = MergeLayers(layers);

            DrawImage(image);
            Console.ReadKey();
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

        private static byte[][] MergeLayers(Layer[] layers)
        {
            // Create 2D array of pixels.
            byte[][] pixels = new byte[ImageHeight][];
            for (int i = 0; i < ImageHeight; i++) pixels[i] = new byte[ImageWidth];

            for (int row = 0; row < ImageHeight; row++)
            {
                for (int column = 0; column < ImageWidth; column++)
                {
                    foreach (var layer in layers.Reverse())
                    {
                        char pixel = layer[row, column];

                        /**
                         * We use different values than the encoding for our colors.
                         * - Transparent pixels are 0.
                         * - Black pixels are 1.
                         * - White pixels are 2.
                         */

                        if (pixel == '0')
                            pixels[row][column] = 1;
                        else if (pixel == '1')
                            pixels[row][column] = 2;
                        else
                        {
                            // Ignore transparent pixels.
                        }
                    }
                }
            }

            return pixels;
        }

        private static void DrawImage(byte[][] pixels)
        {
            for (int row = 0; row < pixels.Length; row++)
            {
                for (int column = 0; column < pixels[row].Length; column++)
                {
                    Console.BackgroundColor = (pixels[row][column]) switch
                    {
                        0 => ConsoleColor.Cyan,
                        1 => ConsoleColor.Black,
                        2 => ConsoleColor.White,
                        _ => throw new Exception("Invalid byte value in pixels"),
                    };
                    Console.Write(' ');
                }

                Console.WriteLine();
            }
        }

        private sealed class Layer
        {
            public Layer(IReadOnlyList<string> rows)
            {
                Rows = rows;
            }

            public char this[int row, int column] => Rows[row][column];

            public IReadOnlyList<string> Rows { get; }
        }
    }
}
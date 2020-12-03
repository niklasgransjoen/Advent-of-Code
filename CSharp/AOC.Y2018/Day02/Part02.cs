using AOC.Resources;
using System;
using System.Text;

namespace AOC.Y2018.Day02
{
    /*
     * The boxes will have IDs which differ by exactly one character at the same position in both strings.
     * What letters are common between the two correct box IDs?
     *
     * https://adventofcode.com/2018/day/2
     */

    public static class Part02
    {
        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day02);
            string[] correctIDs = FindCorrectIDs(input);
            string commonLetters = GetCommonLetters(correctIDs[0], correctIDs[1]);

            General.PrintResult("The common letters of your correct IDs are", commonLetters);
        }

        /// <summary>
        /// Returns an array containing the two correct IDs.
        /// </summary>
        private static string[] FindCorrectIDs(string[] input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                string comp1 = input[i];
                for (int j = i + 1; j < input.Length; j++)
                {
                    string comp2 = input[j];
                    bool hasOneDifferentLetter = false;
                    bool hasMultipleDifferentLetters = false;

                    for (int k = 0; k < comp1.Length; k++)
                    {
                        if (comp1[k] != comp2[k])
                        {
                            if (!hasOneDifferentLetter)
                                hasOneDifferentLetter = true;
                            else
                            {
                                hasMultipleDifferentLetters = true;
                                break;
                            }
                        }
                    }

                    if (!hasMultipleDifferentLetters)
                        return new string[] { comp1, comp2 };
                }
            }

            throw new Exception("Failed to find correct IDs");
        }

        /// <summary>
        /// Returns all the shared letters in the two given IDs.
        /// </summary>
        /// <param name="correctIDs"></param>
        /// <returns></returns>
        private static string GetCommonLetters(string str1, string str2)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str1.Length; i++)
            {
                char c1 = str1[i];
                char c2 = str2[i];

                if (c1 == c2)
                    sb.Append(c1);
            }

            return sb.ToString();
        }
    }
}
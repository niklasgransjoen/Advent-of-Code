using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2018.Day09
{
    /*
     * https://adventofcode.com/2018/day/9
     */

    /// <summary>
    /// My solution to Part One of Day 9. Should work with Part Two as well, but is way too slow.
    /// See day_9b for a solution that works better (for both parts).
    /// </summary>
    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            string input = context.Input;
            ParseInput(input, out int players, out int maxPoints);

            int result = GetGameResult(players, maxPoints);

            AOCUtils.PrintResult("The winning Elf's score is", result);
        }

        private static void ParseInput(string input, out int players, out int maxPoints)
        {
            string[] inputArr = input.Split(';');
            if (inputArr.Length == 2)
            {
                bool playerResult = int.TryParse(inputArr[0], out players);
                bool maxPointsResult = int.TryParse(inputArr[1], out maxPoints);

                if (playerResult && maxPointsResult)
                    return;
            }

            throw new ArgumentException("Input was invalid.");
        }

        /// <summary>
        /// Calculates the score of the winning elf in a game with known inputs.
        /// </summary>
        private static int GetGameResult(int playerCount, int maxPoints)
        {
            int[] playerScores = new int[playerCount];
            int playerIndex = 0;

            Circle marbleCircle = new Circle(maxPoints);

            for (int i = 1; i <= maxPoints; i++)
            {
                playerScores[playerIndex] += marbleCircle.AddMarble();

                playerIndex++;
                if (playerIndex >= playerCount)
                    playerIndex = 0;
            }

            return playerScores.Max();
        }
    }

    /// <summary>
    /// A circle of marbles.
    /// </summary>
    internal class Circle
    {
        private int previousMarble = 0;
        private int currentIndex = 0;

        private readonly IList<int> marbleChain;

        public Circle(int marbleCount)
        {
            marbleChain = new List<int>(marbleCount) { 0 };
        }

        /// <summary>
        /// Adds the next marble. Returns the score.
        /// </summary>
        public int AddMarble()
        {
            int marble = ++previousMarble;

            if (marble % 23 != 0)
            {
                currentIndex += 2;
                ConstrainIndex();

                marbleChain.Insert(currentIndex, marble);
                return 0;
            }
            else
            {
                int points = marble;
                currentIndex -= 7;
                ConstrainIndex();

                points += marbleChain[currentIndex];
                marbleChain.RemoveAt(currentIndex);

                return points;
            }
        }

        private void ConstrainIndex()
        {
            if (currentIndex > 0)
            {
                while (currentIndex >= marbleChain.Count)
                    currentIndex -= marbleChain.Count;
            }
            else
            {
                while (currentIndex < 0)
                    currentIndex += marbleChain.Count;
            }
        }
    }
}
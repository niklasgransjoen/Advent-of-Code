using System;
using System.Linq;
using System.Threading.Tasks;

namespace day_9b
{
    /// <summary>
    /// Works with both Part One and Part Two of Day 9. Faster than my original solution.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            long result = Task.Run(() =>
            {
                string input = "486;7083300";
                ParseInput(input, out int players, out int maxPoints);

                return GetGameResult(players, maxPoints);
            }).Result;

            Console.WriteLine("The winning Elf's score is: {0}", result);
            Console.ReadKey();
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
        private static long GetGameResult(int playerCount, int maxPoints)
        {
            long[] playerScores = new long[playerCount];
            int playerIndex = 0;

            MarbleChain marbleCircle = new MarbleChain(maxPoints);

            for (int i = 0; i < maxPoints; i++)
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
    internal class MarbleChain
    {
        private int currentIndex = 0;
        private readonly int[] removedMarbles;

        /// <summary>
        /// Creates a new marble chain.
        /// </summary>
        /// <param name="marbleCount">The total number of marbles in the chain/the value of the last marble.</param>
        public MarbleChain(int marbleCount)
        {
            int capacity = (int)Math.Ceiling(marbleCount / 23d);
            removedMarbles = new int[capacity];

            FindRemovedMarbles(marbleCount);
        }

        #region Initialization

        /// <summary>
        /// Finds the marbles to remove.
        /// </summary>
        /// <remarks>
        ///
        /// Takes base is the following sequence: https://oeis.org/A025480
        ///
        /// The special rules for moving backwards and removing marbles makes the result differ from this sequence.
        ///
        /// </remarks>
        private void FindRemovedMarbles(int marbleCount)
        {
            int chainSize = marbleCount;
            if (chainSize < 18)
                chainSize = 18;

            // Allocate way too much memory here:
            // (this is the amount needed if the pointer wasn't going backwards at times)
            chainSize = chainSize * 2 - 1;
            int[] chain = new int[chainSize];

            // First 18 numbers follow the standard pattern
            for (int i = 0; i < 19; i++)
            {
                chain[i * 2] = i;
                chain[i * 2 + 1] = chain[i];
            }

            // Main loop
            // Calculates in two parts: The overlappings and the normal sequence.
            int marbleIndex = 0;
            int nextIndex = 37;
            int copyIndex = 38;
            int copySource = 19;

            for (int i = 19; i < chainSize / 2; i += 23)
            {
                removedMarbles[marbleIndex] = chain[copyIndex - 1];
                marbleIndex++;

                // Calculate the 12 numbers where there's an overlap manually
                {
                    // Overlapping (next)
                    chain[nextIndex] = i;
                    chain[nextIndex + 3] = i + 1;
                    chain[nextIndex + 7] = i + 2;
                    chain[nextIndex + 11] = i + 3;

                    // Overlapping (copy)
                    chain[copyIndex] = chain[copySource];
                    chain[copyIndex + 4] = chain[copySource + 1];
                    chain[copyIndex + 8] = chain[copySource + 2];

                    // Overlapping (next, offset)
                    chain[nextIndex + 2] = i + 5;
                    chain[nextIndex + 4] = i + 6;
                    chain[nextIndex + 6] = i + 7;
                    chain[nextIndex + 8] = i + 8;
                    chain[nextIndex + 10] = i + 9;
                }

                // Sequence.
                for (int j = 0; j < 13; j++)
                {
                    chain[nextIndex + 12 + j * 2] = i + 10 + j;
                    chain[copyIndex + 12 + j * 2] = chain[copySource + 3 + j];
                }

                // Increment indexes
                nextIndex += 37;
                copyIndex += 37;
                copySource += 16;
            }
        }

        #endregion Initialization

        /// <summary>
        /// Adds the next marble. Returns the score.
        /// </summary>
        public int AddMarble()
        {
            currentIndex++;
            if (currentIndex % 23 != 0)
                return 0;

            int points = removedMarbles[currentIndex / 23 - 1] + currentIndex;

            return points;
        }
    }
}
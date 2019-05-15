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
            int result = Task.Run(() =>
            {
                //string input = General.GetLineInput();
                string input = "10;46";
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
        private static int GetGameResult(int playerCount, int maxPoints)
        {
            int[] playerScores = new int[playerCount];
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
        private int previousMarble = 0;
        private int currentIndex = 0;

        private readonly Marble[] marbleChain;

        public MarbleChain(int marbleCount)
        {
            int chainSize = 1;
            while (chainSize < marbleCount)
                chainSize *= 2;

            chainSize *= 2;

            // Construct the complete marble chain.
            Marble[] completeChain = new Marble[chainSize];
            for (int i = 0; i < chainSize / 2; i++)
                completeChain[i * 2] = i;

            for (int i = 0; i < chainSize / 2; i++)
                completeChain[i * 2 + 1] = completeChain[i];

            // Extract only the needed part of the chain, remove marbles dividable by 23
            marbleChain = new Marble[chainSize / 2];
            int offset = chainSize / 2 - 1;

            for (int i = 0; i < marbleChain.Length; i++)
            {
                marbleChain[i] = completeChain[i + offset];
                if (marbleChain[i].Value > marbleCount)
                    marbleChain[i].IsSet = false;

                if (marbleChain[i].IsSet && marbleChain[i].Value != 0 && marbleChain[i].Value % 23 == 0)
                    marbleChain[i].IsSet = false;
            }
        }

        /// <summary>
        /// Adds the next marble. Returns the score.
        /// </summary>
        public int AddMarble()
        {
            previousMarble++;
            if (previousMarble % 23 != 0)
                return 0;

            int marble = previousMarble;

            GoToMarble(marble - 1);
            StepBack(steps: 7);

            int points = marble;
            points += marbleChain[currentIndex];
            marbleChain[currentIndex].IsSet = false;
            Console.WriteLine("Marble {0} resulted in {1} points, by removing a marble of value {2}", marble, points, marbleChain[currentIndex].Value);

            return points;
        }

        /// <summary>
        /// Steps forward to the next marble.
        /// </summary>
        private void GoToMarble(int currentMarble)
        {
            while (!marbleChain[currentIndex].IsSet || marbleChain[currentIndex].Value != currentMarble)
            {
                currentIndex++;
                ConstrainIndex();
            }
        }

        /// <summary>
        /// Steps back a set number of marbles.
        /// </summary>
        private void StepBack(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                do
                {
                    currentIndex--;
                    ConstrainIndex();
                } while (!marbleChain[currentIndex].IsSet || marbleChain[currentIndex].Value > previousMarble);
            }
        }

        private void ConstrainIndex()
        {
            while (currentIndex >= marbleChain.Length)
                currentIndex -= marbleChain.Length;

            while (currentIndex < 0)
                currentIndex += marbleChain.Length;
        }
    }

    internal struct Marble
    {
        /// <summary>
        /// Gets or sets whether this marble is set/in use.
        /// </summary>
        public bool IsSet { get; set; }

        /// <summary>
        /// Gets or sets the value of this marble.
        /// </summary>
        public int Value { get; set; }

        #region Operators

        public static int operator +(int value, Marble marble)
        {
            return value + marble.Value;
        }

        public static implicit operator Marble(int value)
        {
            return new Marble
            {
                IsSet = true,
                Value = value,
            };
        }

        #endregion Operators

        public override string ToString()
        {
            if (!IsSet)
                return "false";

            return $"Val: {Value}";
        }
    }
}
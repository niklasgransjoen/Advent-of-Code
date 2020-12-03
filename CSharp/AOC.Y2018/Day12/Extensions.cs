namespace AOC.Y2018.Day12
{
    internal static class Extensions
    {
        /// <summary>
        /// Turns an array of bools into an int.
        /// </summary>
        /// <remarks>The LSB is the last value in the array.</remarks>
        public static int ToInt(this bool[] values)
        {
            int result = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[values.Length - i - 1])
                    result |= 1 << i;
            }

            return result;
        }
    }
}
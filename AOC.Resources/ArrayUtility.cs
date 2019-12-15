namespace AOC.Resources
{
    public static class ArrayUtility
    {
        /// <summary>
        /// Creates a (jagged) 2D array.
        /// </summary>
        public static T[][] Create2D<T>(int width, int height)
        {
            T[][] array = new T[height][];
            for (int i = 0; i < height; i++)
            {
                array[i] = new T[width];
            }

            return array;
        }
    }
}
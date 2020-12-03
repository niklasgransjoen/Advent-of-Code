namespace AOC
{
    public static class ArrayUtility
    {
        /// <summary>
        /// Creates a (jagged) 2D array.
        /// </summary>
        public static T[][] Create2D<T>(int width, int height)
        {
            T[][] array = new T[height][];
            for (int y = 0; y < height; y++)
            {
                array[y] = new T[width];
            }

            return array;
        }

        /// <summary>
        /// Creates a (jagged) 2D array.
        /// </summary>
        public static T[][] Create2D<T>(int width, int height, T initialValue)
        {
            T[][] array = new T[height][];
            for (int y = 0; y < height; y++)
            {
                array[y] = new T[width];
                for (int x = 0; x < width; x++)
                {
                    array[y][x] = initialValue;
                }
            }

            return array;
        }
    }
}
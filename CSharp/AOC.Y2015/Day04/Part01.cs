using System.Security.Cryptography;
using System.Text;

namespace AOC.Y2015.Day04
{
    /**
     * https://adventofcode.com/2015/day/4
     */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            string key = context.Input;
            int result = CalculateResult(key);

            AOCUtils.PrintResult(result);
        }

        private static int CalculateResult(string key)
        {
            using var md5 = new MD5CryptoServiceProvider();
            md5.Initialize();

            int index = 1;
            while (true)
            {
                string input = key + index;
                byte[] bInput = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(bInput);

                // Detect an hash starting with 0x00000... (five zeroes, first 2.5 bytes are zero).
                if (hash[0] == 0 &&
                    hash[1] == 0 &&
                    (hash[2] & 0xF0) == 0)
                {
                    return index;
                }

                index++;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AOC.Resources
{
    public class General
    {
        /// <summary>
        /// Gets input from console.
        /// <para>Escapes on /x.</para>
        /// </summary>
        public static string[] GetInput()
        {
            Console.WriteLine("Please enter your input:");

            List<string> input = new List<string>();
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "/x")
                    break;

                input.Add(line);
            }

            return input.ToArray();
        }

        public static string GetLineInput()
        {
            Console.WriteLine("Please enter your input:");
            return Console.ReadLine();
        }

        public static async Task<string[]> GetInputFromPath(string path)
        {
            string result;
            using (StreamReader reader = new StreamReader(path))
                result = await reader.ReadToEndAsync();

            return result.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static async Task<string> GetLineFromPath(string path)
        {
            using (StreamReader reader = new StreamReader(path))
                return await reader.ReadToEndAsync();
        }
    }
}
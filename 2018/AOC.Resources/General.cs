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

        /// <summary>
        /// Reads the input of a file to a string array.
        /// </summary>
        public static async Task<string[]> GetInputFromPath(string path)
        {
            string result;
            using (StreamReader reader = new StreamReader(path))
                result = await reader.ReadToEndAsync();

            return result.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Reads the input of a relative file to a string array.
        /// </summary>
        public static string[] GetInputFromRelativePath(string path)
        {
            return File.ReadAllLines(@"..\..\..\input\" + path);
        }

        /// <summary>
        /// Returns the contents of a file.
        /// </summary>
        public static async Task<string> GetLineFromPath(string path)
        {
            using (StreamReader reader = new StreamReader(path))
                return await reader.ReadToEndAsync();
        }
    }
}
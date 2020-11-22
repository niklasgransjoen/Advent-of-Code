using AOC.Resources;
using System.IO;

namespace AOC2018
{
    internal class Program
    {
        private static void Main()
        {
            General.SetInput(Path.GetFullPath("Input"));

            Day01.Part01.Exec();
        }
    }
}
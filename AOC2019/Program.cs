using AOC.Resources;
using System.IO;

namespace AOC2019
{
    internal class Program
    {
        private static void Main()
        {
            General.SetInput(Path.GetFullPath("Input"));

            Day06.Part02.Exec();
        }
    }
}
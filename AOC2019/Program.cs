using AOC.Resources;
using System.IO;

namespace AOC2019
{
    internal class Program
    {
        private static void Main()
        {
            General.SetInput(Path.GetFullPath("Input"));

            Day16.Part02.Exec();
        }
    }
}
using AOC.Resources;
using System.IO;

namespace AOC2015
{
    internal class Program
    {
        private static void Main()
        {
            General.SetInput(Path.GetFullPath("Input"));

            Day02.Part02.Exec();
        }
    }
}
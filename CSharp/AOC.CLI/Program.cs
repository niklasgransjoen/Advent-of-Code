using AOC.Resources;
using CommandLine;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace AOC.CLI
{
    public sealed class Options
    {
        [Option('y', "year", HelpText = "The year of the event.", Required = true)]
        public int Year { get; set; }

        [Option('d', "day", HelpText = "The day of the solution to run.", Required = true)]
        public int Day { get; set; }

        [Option('2', "part2", HelpText = "Flag for indicating that it's the second part that should be run.", Default = false)]
        public bool Part2 { get; set; }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run);
        }

        private static void Run(Options o)
        {
            if (!TryGetAssembly(o, out var a))
            {
                Console.WriteLine("Unsupported year.");
                return;
            }

            var targetNamespace = $"AOC.Y{o.Year}.Day{o.Day:00}";
            var targetType = o.Part2 ? "Part02" : "Part01";

            var solution = a.GetTypes()
                .Where(t => t.Namespace == targetNamespace)
                .FirstOrDefault(t => t.Name == targetType);

            if (solution is null)
            {
                Console.WriteLine($"No solution class '{targetType}' in namespace '{targetNamespace}' could be found for the specified year.");
                return;
            }

            var executeMethod = solution.GetMethod("Exec");
            if (executeMethod is null)
            {
                Console.WriteLine("Invalid solution - no execute method.");
                return;
            }
            else if (!executeMethod.IsStatic)
            {
                Console.WriteLine("Invalid solution - execute method must be static.");
                return;
            }
            else if (executeMethod.GetParameters().Length > 0)
            {
                Console.WriteLine("Invalid solution - execute method must be parametherless.");
                return;
            }

            General.SetInput($"../../../../../Input/{o.Year}");
            executeMethod.Invoke(null, null);
        }

        private static bool TryGetAssembly(Options o, [NotNullWhen(true)] out Assembly? assembly)
        {
            assembly = o.Year switch
            {
                2015 => typeof(Y2015.Proxy).Assembly,
                2018 => typeof(Y2018.Proxy).Assembly,
                2019 => typeof(Y2019.Proxy).Assembly,
                _ => null,
            };

            return assembly is not null;
        }
    }
}
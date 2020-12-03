using CommandLine;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

        [Option("part2", HelpText = "Flag for indicating that it's the second part that should be run.", Default = false)]
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
            if (!TryGetSolution(o, out var solution))
                return;

            var executeMethod = solution.GetMethod("Exec");
            if (executeMethod is null)
            {
                AOCUtils.PrintError("Invalid solution - no execute method.");
                return;
            }
            else if (!executeMethod.IsStatic)
            {
                AOCUtils.PrintError("Invalid solution - execute method must be a static method.");
                return;
            }

            if (!TryCreateParameters(o, executeMethod, out var parameters))
                return;

            executeMethod.Invoke(null, parameters);
        }

        private static bool TryGetSolution(Options o, [NotNullWhen(true)] out Type? solution)
        {
            if (!TryGetAssembly(o, out var a))
            {
                solution = null;
                return false;
            }

            var targetNamespace = $"AOC.Y{o.Year}.Day{o.Day:00}";
            var targetType = o.Part2 ? "Part02" : "Part01";

            solution = a.GetTypes()
                .Where(t => t.Namespace == targetNamespace)
                .FirstOrDefault(t => t.Name == targetType);

            if (solution is null)
            {
                AOCUtils.PrintError($"No solution class '{targetType}' in namespace '{targetNamespace}' could be found for the specified year.");
                return false;
            }

            return true;
        }

        private static bool TryGetAssembly(Options o, [NotNullWhen(true)] out Assembly? assembly)
        {
            var proxy = o.Year switch
            {
                2015 => typeof(Y2015.Proxy),
                2018 => typeof(Y2018.Proxy),
                2019 => typeof(Y2019.Proxy),
                2020 => typeof(Y2020.Proxy),
                _ => null,
            };

            if (proxy is null)
            {
                AOCUtils.PrintError("Unsupported year.");

                assembly = null;
                return false;
            }

            assembly = proxy.Assembly;
            return true;
        }

        private static bool TryCreateParameters(Options o, MethodInfo executeMethod, [NotNullWhen(true)] out object[]? parameters)
        {
            var paramInfo = executeMethod.GetParameters();
            parameters = new object[paramInfo.Length];
            for (int i = 0; i < paramInfo.Length; i++)
            {
                var param = paramInfo[i];
                if (param.ParameterType == typeof(AOCContext))
                {
                    var currentDir = Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? string.Empty;
                    var rawInputPath = $"../../../../../Input/{o.Year}/Day{o.Day:00}.txt";
                    var inputPath = Path.Combine(currentDir, rawInputPath);

                    if (!File.Exists(inputPath))
                    {
                        AOCUtils.PrintError($"Input path '{inputPath}' does not exist.");
                        parameters = null;
                        return false;
                    }

                    parameters[i] = new AOCContext
                    {
                        Input = File.ReadAllText(inputPath),
                    };
                }
                else
                {
                    AOCUtils.PrintError($"Invalid solution - Invalid parameter type '{param.ParameterType}'.");
                    parameters = null;
                    return false;
                }
            }

            return true;
        }
    }
}
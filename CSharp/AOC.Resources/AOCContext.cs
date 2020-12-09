using System;

namespace AOC
{
    public record AOCContext
    {
        /// <summary>
        /// The input of the puzzle.
        /// </summary>
        public string Input { get; init; } = string.Empty;
    }

    public static class AOCContextExtensions
    {
        /// <summary>
        /// Returns the input of this context split by lines.
        /// </summary>
        public static string[] GetInputLines(this AOCContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (context.Input is null)
                return Array.Empty<string>();

            return context.Input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static int[] GetIntegerInput(this AOCContext context)
        {
            var input = context.GetInputLines();
            return AOCUtils.StringToInt(input);
        }

        public static long[] GetBigIntegerInput(this AOCContext context)
        {
            var input = context.GetInputLines();
            return AOCUtils.StringToLong(input);
        }

        public static string[] GetCSVInput(this AOCContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (context.Input is null)
                return Array.Empty<string>();

            return context.Input.Split(',');
        }
    }
}
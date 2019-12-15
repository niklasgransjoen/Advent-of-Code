using System;

namespace AOC2019.Day15.P01
{
    public sealed class IntcodeEvaluationException : Exception
    {
        public IntcodeEvaluationException(string message) : base(message)
        {
        }
    }
}
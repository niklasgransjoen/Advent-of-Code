namespace AOC.Y2020.Day18.P02.Syntax
{
    internal class LiteralExpressionSyntax : ExpressionSyntax
    {
        public LiteralExpressionSyntax(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
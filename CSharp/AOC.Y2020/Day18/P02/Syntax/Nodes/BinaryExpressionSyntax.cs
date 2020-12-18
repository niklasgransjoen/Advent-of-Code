namespace AOC.Y2020.Day18.P02.Syntax
{
    internal class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, ExpressionSyntax right, bool isMultiply)
        {
            Left = left;
            Right = right;
            IsMultiply = isMultiply;
        }

        public ExpressionSyntax Left { get; }
        public ExpressionSyntax Right { get; }
        public bool IsMultiply { get; }
    }
}
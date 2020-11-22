namespace AOC2015.Day07.P02.Syntax
{
    public sealed class BinaryExpression : Expression
    {
        public BinaryExpression(
            SyntaxToken leftInput,
            SyntaxToken operatorToken,
            SyntaxToken rightInput,
            SyntaxToken arrowToken,
            SyntaxToken output)
        {
            LeftInput = leftInput;
            OperatorToken = operatorToken;
            RightInput = rightInput;
            ArrowToken = arrowToken;
            Output = output;
        }

        public SyntaxToken LeftInput { get; }
        public SyntaxToken OperatorToken { get; }
        public SyntaxToken RightInput { get; }
        public SyntaxToken ArrowToken { get; }
        public override SyntaxToken Output { get; }

    }
}
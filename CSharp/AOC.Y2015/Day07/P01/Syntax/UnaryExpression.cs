namespace AOC.Y2015.Day07.P01.Syntax
{
    public sealed class UnaryExpression : Expression
    {
        public UnaryExpression(SyntaxToken notToken, SyntaxToken input, SyntaxToken arrowToken, SyntaxToken output)
        {
            NotToken = notToken;
            Input = input;
            ArrowToken = arrowToken;
            Output = output;
        }

        public SyntaxToken NotToken { get; }
        public SyntaxToken Input { get; }
        public SyntaxToken ArrowToken { get; }
        public override SyntaxToken Output { get; }
    }
}
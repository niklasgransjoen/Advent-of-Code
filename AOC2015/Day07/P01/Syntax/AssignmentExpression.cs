namespace AOC2015.Day07.P01.Syntax
{
    public sealed class AssignmentExpression : Expression
    {
        public AssignmentExpression(SyntaxToken leftToken, SyntaxToken arrowToken, SyntaxToken output)
        {
            Input = leftToken;
            ArrowToken = arrowToken;
            Output = output;
        }

        public SyntaxToken Input { get; }
        public SyntaxToken ArrowToken { get; }

        public override SyntaxToken Output { get; }
    }
}
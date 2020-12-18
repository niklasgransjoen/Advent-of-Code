namespace AOC.Y2020.Day18.P02.Syntax
{
    internal class SyntaxToken
    {
        public SyntaxToken(TokenKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }

        public TokenKind Kind { get; }
        public string Text { get; }
    }
}
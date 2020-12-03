namespace AOC.Y2015.Day07.P02.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        LiteralToken,
        IdentifierToken,
        ArrowToken,
        WhiteSpaceToken,
        EndOfFileToken,

        // Keywords
        NotKeyword,
        OrKeyword,
        AndKeyword,
        RShiftKeyword,
        LShiftKeyword,
    }
}
namespace AOC2015.Day07.P01.Syntax
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
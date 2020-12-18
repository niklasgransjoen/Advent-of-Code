namespace AOC.Y2020.Day18.P01.Syntax
{
    internal enum TokenKind
    {
        EndOfFileToken,
        WhitespaceToken,

        NumberToken,
        PlusToken,
        StarToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
    }
}
namespace AOC.Y2020.Day18.P02.Syntax
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
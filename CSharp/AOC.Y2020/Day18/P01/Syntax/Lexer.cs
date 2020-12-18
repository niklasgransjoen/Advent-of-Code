using System;

namespace AOC.Y2020.Day18.P01.Syntax
{
    internal class Lexer
    {
        private readonly string _source;
        private int _currentIndex = 0;

        public Lexer(string source)
        {
            _source = source;
        }

        public SyntaxToken Lex()
        {
            var token = InternalLex();
            _currentIndex++;

            return token;
        }

        private SyntaxToken InternalLex()
        {
            switch (Current)
            {
                case '\0':
                    return new(TokenKind.EndOfFileToken, "\0");

                case '+':
                    return new(TokenKind.PlusToken, "+");

                case '*':
                    return new(TokenKind.StarToken, "*");

                case '(':
                    return new(TokenKind.OpenParenthesisToken, "(");

                case ')':
                    return new(TokenKind.CloseParenthesisToken, ")");
            }

            if (char.IsNumber(Current))
                return new(TokenKind.NumberToken, Current.ToString());

            if (char.IsWhiteSpace(Current))
                return new SyntaxToken(TokenKind.WhitespaceToken, Current.ToString());

            throw new Exception($"Bad character '{Current}'.");
        }

        private char Current => Lookup(0);

        private char Lookup(int offset)
        {
            var index = _currentIndex + offset;
            if (index >= _source.Length)
                return '\0';

            return _source[index];
        }
    }
}
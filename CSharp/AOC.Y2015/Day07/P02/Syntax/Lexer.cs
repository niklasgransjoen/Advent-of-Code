using System;

namespace AOC.Y2015.Day07.P02.Syntax
{
    public sealed class Lexer
    {
        private readonly string _sourceText;

        private int _start;
        private int _position;

        public Lexer(string sourceText)
        {
            _sourceText = sourceText;
        }

        #region Properties

        private char Current => Peek(0);
        private char LookAhead => Peek(1);

        #endregion Properties

        public SyntaxToken Lex()
        {
            _start = _position;

            switch (Current)
            {
                case '\0':
                    _position++;
                    return new SyntaxToken(SyntaxKind.EndOfFileToken, "\0");

                case '-' when LookAhead == '>':
                    _position += 2;
                    return new SyntaxToken(SyntaxKind.ArrowToken, "->");

                case '\r':
                case '\n':
                case ' ':
                    return ReadWhiteSpace();

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadNumberLiteral();

                default:
                    if (char.IsLetter(Current))
                        return ReadIdentifierOrKeyword();
                    else
                        throw new Exception("Illegal character");
            }
        }

        private SyntaxToken ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                _position++;

            return new SyntaxToken(SyntaxKind.WhiteSpaceToken, _sourceText[_start.._position]);
        }

        private SyntaxToken ReadNumberLiteral()
        {
            while (char.IsDigit(Current))
                _position++;

            return new SyntaxToken(SyntaxKind.LiteralToken, _sourceText[_start.._position]);
        }

        private SyntaxToken ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current))
                _position++;

            string word = _sourceText[_start.._position];
            if (TryGetKeywordKind(word, out SyntaxKind keyword))
                return new SyntaxToken(keyword, word);

            return new SyntaxToken(SyntaxKind.IdentifierToken, word);
        }

        private bool TryGetKeywordKind(string word, out SyntaxKind keyword)
        {
            switch (word)
            {
                case "NOT":
                    keyword = SyntaxKind.NotKeyword;
                    return true;

                case "OR":
                    keyword = SyntaxKind.OrKeyword;
                    return true;

                case "AND":
                    keyword = SyntaxKind.AndKeyword;
                    return true;

                case "RSHIFT":
                    keyword = SyntaxKind.RShiftKeyword;
                    return true;

                case "LSHIFT":
                    keyword = SyntaxKind.LShiftKeyword;
                    return true;

                default:
                    keyword = default;
                    return false;
            }
        }

        #region Utilities

        private char Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _sourceText.Length)
                return '\0';

            return _sourceText[index];
        }

        #endregion Utilities
    }
}
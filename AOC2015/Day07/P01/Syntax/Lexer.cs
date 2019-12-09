using System;
using System.Diagnostics.CodeAnalysis;

namespace AOC2015.Day07.P01.Syntax
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

                case '-':
                    if (LookAhead == '>')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.ArrowToken, "->");
                    }
                    break;

                case '\r':
                case '\n':
                case ' ':
                    _position++;
                    return Lex();
            }

            if (char.IsDigit(Current))
                return ReadNumberLiteral();

            if (TryReadKeyword(out SyntaxKind keyword, out string? text))
                return new SyntaxToken(keyword, text);

            return ReadIdentifier();
        }

        private bool TryReadKeyword(out SyntaxKind keyword, [NotNullWhen(true)] out string? text)
        {
            int offset = 0;
            while (char.IsLetter(Peek(offset)))
                offset++;

            text = _sourceText[_start..(_position + offset)];
            switch (text)
            {
                case "NOT":
                    keyword = SyntaxKind.NotKeyword;
                    _position += offset + 1;
                    return true;

                case "OR":
                    keyword = SyntaxKind.OrKeyword;
                    _position += offset + 1;
                    return true;

                case "AND":
                    keyword = SyntaxKind.AndKeyword;
                    _position += offset + 1;
                    return true;

                case "RSHIFT":
                    keyword = SyntaxKind.RShiftKeyword;
                    _position += offset + 1;
                    return true;

                case "LSHIFT":
                    keyword = SyntaxKind.LShiftKeyword;
                    _position += offset + 1;
                    return true;

                default:
                    keyword = default;
                    text = null;
                    return false;
            }
        }

        private SyntaxToken ReadNumberLiteral()
        {
            while (char.IsDigit(Current))
                _position++;

            return new SyntaxToken(SyntaxKind.LiteralToken, _sourceText[_start.._position]);
        }

        private SyntaxToken ReadIdentifier()
        {
            if (!char.IsLetter(Current))
                throw new Exception("Expected letter");

            while (char.IsLetter(Current))
                _position++;

            return new SyntaxToken(SyntaxKind.IdentifierToken, _sourceText[_start.._position]);
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
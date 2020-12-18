using System;
using System.Collections.Generic;
using System.Globalization;

namespace AOC.Y2020.Day18.P01.Syntax
{
    internal class Parser
    {
        private readonly List<SyntaxToken> _tokens;
        private int _currentIndex = 0;

        private Parser(List<SyntaxToken> tokens)
        {
            _tokens = tokens;
        }

        public static ExpressionSyntax Parse(string source)
        {
            var lexer = new Lexer(source);
            var tokens = new List<SyntaxToken>();
            while (true)
            {
                var token = lexer.Lex();
                if (token.Kind == TokenKind.WhitespaceToken)
                    continue;

                if (token.Kind == TokenKind.EndOfFileToken)
                    break;

                tokens.Add(token);
            }

            var parser = new Parser(tokens);
            return parser.Parse();
        }

        private ExpressionSyntax Parse(Func<bool>? escapePredicate = null)
        {
            var left = ParsePrimitive();
            while (true)
            {
                if (_currentIndex >= _tokens.Count || (escapePredicate?.Invoke() ?? false))
                    return left;

                left = ParseBinaryExpression(left);
            }
        }

        private ExpressionSyntax ParsePrimitive()
        {
            return Current.Kind == TokenKind.NumberToken ? ParseLiteralExpression() : ParseParenthesisedExpression();
        }

        private ExpressionSyntax ParseLiteralExpression()
        {
            var numberToken = MatchToken(TokenKind.NumberToken);
            var value = int.Parse(numberToken.Text, CultureInfo.InvariantCulture);
            return new LiteralExpressionSyntax(value);
        }

        private ExpressionSyntax ParseBinaryExpression(ExpressionSyntax left)
        {
            var sign = Next();
            if (sign.Kind is not TokenKind.PlusToken and not TokenKind.StarToken)
                throw new Exception($"Invalid binary operator '{sign.Kind}'.");

            var isMultiply = sign.Kind == TokenKind.StarToken;
            var right = ParsePrimitive();
            return new BinaryExpressionSyntax(left, right, isMultiply);
        }

        private ExpressionSyntax ParseParenthesisedExpression()
        {
            MatchToken(TokenKind.OpenParenthesisToken);
            var parenthesisedExpression = Parse(escapePredicate: () => Current.Kind == TokenKind.CloseParenthesisToken);
            MatchToken(TokenKind.CloseParenthesisToken);
            return parenthesisedExpression;
        }

        private SyntaxToken Current => Lookup(0);

        private SyntaxToken Lookup(int offset)
        {
            var index = _currentIndex + offset;
            if (index >= _tokens.Count)
                return _tokens[^1];

            return _tokens[index];
        }

        private SyntaxToken MatchToken(TokenKind kind)
        {
            if (Current.Kind != kind)
                throw new Exception($"The next token is of kind '{Current.Kind}', but '{kind}' was expected.");

            return Next();
        }

        private SyntaxToken Next()
        {
            var next = Current;
            _currentIndex++;
            return next;
        }
    }
}
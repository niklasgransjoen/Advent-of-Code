using System;
using System.Collections.Generic;

namespace AOC2015.Day07.P01.Syntax
{
    public sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;

        private int _position;

        private Parser(string sourceText)
        {
            Lexer lexer = new Lexer(sourceText);
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            SyntaxToken current;
            do
            {
                current = lexer.Lex();
                tokens.Add(current);
            }
            while (current.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToArray();
        }

        #region Properties

        public SyntaxToken Current => Peek(0);
        public SyntaxToken LookAhead => Peek(1);

        #endregion Properties

        public static Expression[] Parse(string sourceText)
        {
            Parser parser = new Parser(sourceText);

            List<Expression> expressions = new List<Expression>();
            while (parser.Current.Kind != SyntaxKind.EndOfFileToken)
            {
                expressions.Add(parser.Parse());
            }

            return expressions.ToArray();
        }

        private Expression Parse()
        {
            if (Current.Kind == SyntaxKind.NotKeyword)
                return ParseUnaryExpression();
            else if (LookAhead.Kind == SyntaxKind.ArrowToken)
                return ParseAssignmentExpression();
            else
                return ParseBinaryExpression();
        }

        private Expression ParseUnaryExpression()
        {
            SyntaxToken notToken = MatchToken(SyntaxKind.NotKeyword);
            SyntaxToken input = Next();
            SyntaxToken arrowToken = MatchToken(SyntaxKind.ArrowToken);
            SyntaxToken output = MatchToken(SyntaxKind.IdentifierToken);

            return new UnaryExpression(notToken, input, arrowToken, output);
        }

        private Expression ParseAssignmentExpression()
        {
            SyntaxToken input = Next();
            SyntaxToken arrowToken = MatchToken(SyntaxKind.ArrowToken);
            SyntaxToken output = MatchToken(SyntaxKind.IdentifierToken);

            return new AssignmentExpression(input, arrowToken, output);
        }

        private Expression ParseBinaryExpression()
        {
            SyntaxToken leftInput = Next();
            SyntaxToken operatorToken = Next(); // better hope this is an operator 😓
            SyntaxToken rightInput = Next();
            SyntaxToken arrowToken = MatchToken(SyntaxKind.ArrowToken);
            SyntaxToken output = MatchToken(SyntaxKind.IdentifierToken);

            return new BinaryExpression(
                leftInput,
                operatorToken,
                rightInput,
                arrowToken,
                output);
        }

        #region Utilities

        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[^1];

            return _tokens[index];
        }

        private SyntaxToken Next()
        {
            SyntaxToken current = Current;
            _position++;

            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return Next();

            throw new Exception($"Expected token of kind '{kind}', but got '{Current.Kind}' instead.");
        }

        #endregion Utilities
    }
}
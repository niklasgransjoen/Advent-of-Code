using AOC.Y2015.Day07.P01.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2015.Day07.P01.Binding
{
    public sealed class Binder
    {
        private readonly Dictionary<string, Expression> _expressions;
        private readonly Dictionary<string, BoundExpression> _boundExpressions = new Dictionary<string, BoundExpression>();

        private Binder(Expression[] expressions)
        {
            _expressions = expressions.ToDictionary(e => e.Output.Text);
        }

        public static BoundExpression[] Bind(Expression[] expressions)
        {
            Binder binder = new Binder(expressions);
            return binder.Bind();
        }

        private BoundExpression[] Bind()
        {
            // We assume no feedback loops.
            List<BoundExpression> boundExpressions = new List<BoundExpression>();

            foreach (var expression in _expressions.Values)
            {
                var boundExpression = Bind(expression);
                boundExpressions.Add(boundExpression);
            }

            return boundExpressions.ToArray();
        }

        private BoundExpression Bind(Expression expression)
        {
            if (expression is BinaryExpression b)
            {
                BoundExpression left = BindSignal(b.LeftInput);
                BoundBinaryOperatorKind op = BindBinaryOperator(b.OperatorToken);
                BoundExpression right = BindSignal(b.RightInput);

                return new BoundBinaryExpression(left, op, right, expression.Output.Text);
            }

            if (expression is UnaryExpression u)
            {
                BoundExpression input = BindSignal(u.Input);
                return new BoundUnaryExpression(input, expression.Output.Text);
            }

            if (expression is AssignmentExpression a)
            {
                BoundExpression input = BindSignal(a.Input);
                return new BoundAssignmentExpression(input, expression.Output.Text);
            }

            throw new Exception("Illegal expression type");
        }

        private BoundExpression BindSignal(SyntaxToken token)
        {
            if (_boundExpressions.TryGetValue(token.Text, out var boundExpression))
                return boundExpression;

            // Input is number.
            if (token.Kind == SyntaxKind.LiteralToken)
            {
                ushort value = ushort.Parse(token.Text);

                var boundLiteral = new BoundLiteralExpression(value);
                _boundExpressions[token.Text] = boundLiteral;

                return boundLiteral;
            }

            // Input is identifier.
            if (!_expressions.TryGetValue(token.Text, out Expression? signal))
                throw new Exception("Failed to locate signal");

            boundExpression = Bind(signal);
            _boundExpressions[token.Text] = boundExpression;

            return boundExpression;
        }

        private BoundBinaryOperatorKind BindBinaryOperator(SyntaxToken token)
        {
            return token.Kind switch
            {
                SyntaxKind.OrKeyword => BoundBinaryOperatorKind.OR,
                SyntaxKind.AndKeyword => BoundBinaryOperatorKind.AND,
                SyntaxKind.RShiftKeyword => BoundBinaryOperatorKind.RSHIFT,
                SyntaxKind.LShiftKeyword => BoundBinaryOperatorKind.LSHIFT,

                _ => throw new Exception("Illegal token type"),
            };
        }
    }
}
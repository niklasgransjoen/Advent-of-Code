using AOC.Y2015.Day07.P02.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2015.Day07.P02.Binding
{
    public sealed class Binder
    {
        private readonly Dictionary<string, Expression> _expressions;
        private readonly Dictionary<string, BoundExpression> _boundExpressions = new Dictionary<string, BoundExpression>();

        private Binder(IEnumerable<Expression> expressions)
        {
            _expressions = expressions.ToDictionary(e => e.Output.Text);
        }

        public static IEnumerable<BoundExpression> Bind(IEnumerable<Expression> expressions)
        {
            Binder binder = new Binder(expressions);
            return binder.Bind();
        }

        private IEnumerable<BoundExpression> Bind()
        {
            // We assume no feedback loops.
            foreach (var expression in _expressions.Values)
            {
                if (!_boundExpressions.TryGetValue(expression.Output.Text, out var boundExpression))
                {
                    boundExpression = Bind(expression);
                    _boundExpressions[expression.Output.Text] = boundExpression;
                }

                yield return boundExpression;
            }
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

            if (token.Kind == SyntaxKind.LiteralToken)
            {
                // Input is number.
                ushort value = ushort.Parse(token.Text);
                boundExpression = new BoundLiteralExpression(value);
            }
            else
            {
                // Input is identifier.
                Expression signal = _expressions[token.Text];
                boundExpression = Bind(signal);
            }

            _boundExpressions[token.Text] = boundExpression;
            return boundExpression;
        }

        private static BoundBinaryOperatorKind BindBinaryOperator(SyntaxToken token)
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
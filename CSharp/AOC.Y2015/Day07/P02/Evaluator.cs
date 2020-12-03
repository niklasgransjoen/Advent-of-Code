using AOC.Y2015.Day07.P02.Binding;
using System;
using System.Collections.Generic;

namespace AOC.Y2015.Day07.P02
{
    public sealed class Evaluator
    {
        private readonly Dictionary<BoundExpression, ushort> _evaluatedValues = new Dictionary<BoundExpression, ushort>();

        public ushort Evaluate(BoundExpression expression)
        {
            if (_evaluatedValues.TryGetValue(expression, out ushort value))
                return value;

            value = EvaluateInternal(expression);
            _evaluatedValues[expression] = value;

            return value;
        }

        public void ForceSignalValue(BoundExpression expression, ushort value)
        {
            _evaluatedValues[expression] = value;
        }

        private ushort EvaluateInternal(BoundExpression expression)
        {
            return expression switch
            {
                BoundBinaryExpression b => EvaluateBinaryExpression(b),
                BoundUnaryExpression u => EvaluateUnaryExpression(u),
                BoundAssignmentExpression a => EvaluateAssignmentExpression(a),
                BoundLiteralExpression l => EvaluateLiteralExpression(l),

                _ => throw new Exception("Illegal expression type"),
            };
        }

        private ushort EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            ushort left = Evaluate(b.Left);
            ushort right = Evaluate(b.Right);

            ushort result = b.Op switch
            {
                BoundBinaryOperatorKind.AND => (ushort)(left & right),
                BoundBinaryOperatorKind.OR => (ushort)(left | right),
                BoundBinaryOperatorKind.RSHIFT => (ushort)(left >> right),
                BoundBinaryOperatorKind.LSHIFT => (ushort)(left << right),

                _ => throw new Exception("Illegal binary operator"),
            };

            return result;
        }

        private ushort EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            return (ushort)~Evaluate(u.Input);
        }

        private ushort EvaluateAssignmentExpression(BoundAssignmentExpression a)
        {
            return Evaluate(a.Input);
        }

        private static ushort EvaluateLiteralExpression(BoundLiteralExpression l)
        {
            return l.Value;
        }
    }
}
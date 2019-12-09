using System;

namespace AOC2015.Day07.P01.Binding
{
    public sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind op, BoundExpression right, string signalName)
        {
            Left = left;
            Op = op;
            Right = right;
            SignalName = signalName;
        }

        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind Op { get; }
        public BoundExpression Right { get; }
        public override string SignalName { get; }

        protected override ushort EvaluateSignal()
        {
            ushort left = Left.Evaluate();
            ushort right = Right.Evaluate();

            int result = Op switch
            {
                BoundBinaryOperatorKind.AND => left & right,
                BoundBinaryOperatorKind.OR => left | right,
                BoundBinaryOperatorKind.RSHIFT => left >> right,
                BoundBinaryOperatorKind.LSHIFT => left << right,
                _ => throw new Exception("Illegal value"),
            };

            return (ushort)result;
        }
    }
}
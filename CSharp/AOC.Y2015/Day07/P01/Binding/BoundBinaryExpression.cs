namespace AOC.Y2015.Day07.P01.Binding
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
    }
}
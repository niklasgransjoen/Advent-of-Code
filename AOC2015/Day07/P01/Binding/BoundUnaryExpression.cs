namespace AOC2015.Day07.P01.Binding
{
    public sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundExpression input, string signalName)
        {
            Input = input;
            SignalName = signalName;
        }

        public BoundExpression Input { get; }
        public override string SignalName { get; }
    }
}
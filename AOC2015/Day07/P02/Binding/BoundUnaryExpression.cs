namespace AOC2015.Day07.P02.Binding
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
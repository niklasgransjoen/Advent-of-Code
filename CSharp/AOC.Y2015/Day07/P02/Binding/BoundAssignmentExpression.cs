namespace AOC.Y2015.Day07.P02.Binding
{
    public sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(BoundExpression input, string signalName)
        {
            Input = input;
            SignalName = signalName;
        }

        public BoundExpression Input { get; }

        public override string SignalName { get; }
    }
}
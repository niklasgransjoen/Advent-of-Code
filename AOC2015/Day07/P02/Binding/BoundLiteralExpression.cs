namespace AOC2015.Day07.P02.Binding
{
    public sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(ushort value)
        {
            Value = value;
        }

        public ushort Value { get; }
        public override string SignalName => "Literal value";
    }
}
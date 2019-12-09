namespace AOC2015.Day07.P01.Binding
{
    public sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(ushort value)
        {
            Value = value;
        }

        public ushort Value { get; }
        public override string SignalName => "Literal value";

        protected override ushort EvaluateSignal() => Value;
    }
}
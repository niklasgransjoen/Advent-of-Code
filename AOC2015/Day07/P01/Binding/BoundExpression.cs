namespace AOC2015.Day07.P01.Binding
{
    public abstract class BoundExpression
    {
        public abstract string SignalName { get; }

        protected abstract ushort EvaluateSignal();

        private ushort? _cachedValue;

        public ushort Evaluate()
        {
            if (_cachedValue.HasValue)
                return _cachedValue.Value;

            _cachedValue = EvaluateSignal();
            return _cachedValue.Value;
        }
    }
}
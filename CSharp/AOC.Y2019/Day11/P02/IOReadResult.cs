using System;

namespace AOC.Y2019.Day11.P02
{
    public readonly struct IOReadResult : IEquatable<IOReadResult>
    {
        public IOReadResult(int value) : this(true, value)
        {
        }

        public IOReadResult(bool valueAvailable, int value)
        {
            ValueAvailable = valueAvailable;
            Value = value;
        }

        public bool ValueAvailable { get; }
        public int Value { get; }

        public override bool Equals(object? obj)
        {
            return obj is IOReadResult readResult && Equals(readResult);
        }

        public override int GetHashCode()
        {
            if (!ValueAvailable)
                return 39;

            int hash = 7;
            hash = (hash * 11) + Value.GetHashCode();

            return hash;
        }

        public static bool operator ==(IOReadResult left, IOReadResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IOReadResult left, IOReadResult right)
        {
            return !(left == right);
        }

        public bool Equals(IOReadResult other)
        {
            if (ValueAvailable ^ other.ValueAvailable)
                return false;

            if (!ValueAvailable && !other.ValueAvailable)
                return true;

            return Value == other.Value;
        }
    }
}
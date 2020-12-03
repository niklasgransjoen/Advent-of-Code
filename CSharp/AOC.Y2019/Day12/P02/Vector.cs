using System;
using System.Diagnostics.CodeAnalysis;

namespace AOC.Y2019.Day12.P02
{
    public readonly struct Vector : IEquatable<Vector>
    {
        private readonly int _hash;

        public Vector(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            
            _hash = CalculateHash(x, y, z);
        }

        private static int CalculateHash(int x, int y, int z)
        {
            int hash = 11;
            hash = (hash * 7) + x.GetHashCode();
            hash = (hash * 7) + y.GetHashCode();
            hash = (hash * 7) + z.GetHashCode();

            return hash;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public override bool Equals(object? obj)
        {
            return obj is Vector vector && Equals(vector);
        }

        public bool Equals([AllowNull] Vector other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !(left == right);
        }

        public static Vector operator +(Vector left, Vector right)
        {
            int x = left.X + right.X;
            int y = left.Y + right.Y;
            int z = left.Z + right.Z;

            return new Vector(x, y, z);
        }
    }
}
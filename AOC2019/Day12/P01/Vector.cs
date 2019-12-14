using System;
using System.Diagnostics.CodeAnalysis;

namespace AOC2019.Day12.P01
{
    public struct Vector : IEquatable<Vector>
    {
        public Vector(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

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
            int hash = 11;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            hash = (hash * 7) + Z.GetHashCode();

            return hash;
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
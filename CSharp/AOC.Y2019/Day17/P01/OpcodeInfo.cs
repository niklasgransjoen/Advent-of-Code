﻿using System;

namespace AOC.Y2019.Day17.P01
{
    public readonly struct OpcodeInfo : IEquatable<OpcodeInfo>
    {
        public OpcodeInfo(int paramCount)
        {
            ParameterCount = paramCount;
        }

        public int ParameterCount { get; }

        #region Operators

        public override bool Equals(object? obj)
        {
            return obj is OpcodeInfo opcodeInfo && Equals(opcodeInfo);
        }

        public bool Equals(OpcodeInfo other)
        {
            return ParameterCount == other.ParameterCount;
        }

        public override int GetHashCode()
        {
            return ParameterCount.GetHashCode();
        }

        public static bool operator ==(OpcodeInfo left, OpcodeInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OpcodeInfo left, OpcodeInfo right)
        {
            return !(left == right);
        }

        #endregion Operators
    }
}
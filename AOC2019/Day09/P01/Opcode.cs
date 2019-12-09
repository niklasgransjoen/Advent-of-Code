﻿namespace AOC2019.Day09.P01
{
    public enum Opcode
    {
        ADD = 1,
        MUL = 2,
        INP = 3,
        OUT = 4,
        JNZ = 5,
        JZ = 6,
        LT = 7,     // less than
        EQL = 8,
        ARB = 9,    // adjust relative base
        HLT = 99,
    }
}
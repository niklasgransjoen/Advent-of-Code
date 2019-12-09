﻿namespace AOC2015.Day07.P02.Syntax
{
    public sealed class SyntaxToken
    {
        public SyntaxToken(SyntaxKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }

        public SyntaxKind Kind { get; }
        public string Text { get; }
    }
}
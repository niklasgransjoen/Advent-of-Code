using AOC.Y2020.Day18.P02.Syntax;
using System;
using System.Linq;
using System.Numerics;

namespace AOC.Y2020.Day18
{
    /**
      * https://adventofcode.com/2020/day/18
      */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            var result = context.GetInputLines()
                                .Select(line => Parser.Parse(line))
                                .Select(expr => EvaluateExpression(expr))
                                .Aggregate(default(BigInteger), (sum, next) => checked(sum + next));

            AOCUtils.PrintResult(result);
        }

        private static BigInteger EvaluateExpression(ExpressionSyntax expression) => expression switch
        {
            LiteralExpressionSyntax literalExpression => literalExpression.Value,
            BinaryExpressionSyntax binaryExpression when binaryExpression.IsMultiply => checked(EvaluateExpression(binaryExpression.Left) * EvaluateExpression(binaryExpression.Right)),
            BinaryExpressionSyntax binaryExpression => checked(EvaluateExpression(binaryExpression.Left) + EvaluateExpression(binaryExpression.Right)),
            _ => throw new Exception($"Invalid expression type '{expression.GetType()}'."),
        };
    }
}
﻿using AOC.Y2019.Day11.P01;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AOC.Y2019.Day11
{
    /**
     * https://adventofcode.com/2019/day/11
     */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            string[] input = context.GetCSVInput();
            long[] intcode = AOCUtils.StringToLong(input);

            Controller controller = new Controller();
            IntcodeInterpreter interpreter = new IntcodeInterpreter(intcode, controller);
            try
            {
                interpreter.Execute();
            }
            catch (IntcodeEvaluationException ex)
            {
                AOCUtils.PrintError(ex.Message);
                return;
            }

            if (!interpreter.HasHalted)
            {
                AOCUtils.PrintError("Interpreter returned before halting.");
                return;
            }

            AOCUtils.PrintResult(controller.Result);
        }

        /// <summary>
        /// The controller for the robot.
        /// </summary>
        private sealed class Controller : IIOPort
        {
            private enum Direction
            {
                Up,
                Right,
                Down,
                Left,
            }

            private readonly Dictionary<Point, bool> _panelColors = new Dictionary<Point, bool>();
            private Point _currentPosition = new Point(0, 0);
            private Direction _direction = Direction.Up;
            private bool _isPainting = true;

            public int Result => _panelColors.Count;

            public IOReadResult Read()
            {
                if (_panelColors.TryGetValue(_currentPosition, out bool isWhite))
                    return new IOReadResult(isWhite ? 1 : 0);

                return new IOReadResult(0);
            }

            public void RegisterInterpreterForInput(IInterpreter interpreter)
            {
                throw new NotImplementedException();
            }

            public void Write(long value)
            {
                if (_isPainting)
                {
                    _panelColors[_currentPosition] = value == 1;
                    _isPainting = false;
                }
                else
                {
                    int positionChange = value == 1 ? 1 : -1;
                    _currentPosition = _direction switch
                    {
                        Direction.Up => new Point(_currentPosition.X + positionChange, _currentPosition.Y),
                        Direction.Right => new Point(_currentPosition.X, _currentPosition.Y + positionChange),
                        Direction.Down => new Point(_currentPosition.X - positionChange, _currentPosition.Y),
                        Direction.Left => new Point(_currentPosition.X, _currentPosition.Y - positionChange),
                        _ => throw new IntcodeEvaluationException("Illegal direction"),
                    };

                    _direction += positionChange;
                    if (_direction < 0)
                        _direction = Direction.Left;
                    else if (_direction > Direction.Left)
                        _direction = Direction.Up;

                    _isPainting = true;
                }
            }
        }

        [DebuggerDisplay("({X}, {Y})")]
        private readonly struct Point : IEquatable<Point>
        {
            private readonly int _hash;

            public Point(int x, int y)
            {
                X = x;
                Y = y;

                int hash = 11;
                hash = (hash * 7) + X.GetHashCode();
                hash = (hash * 7) + Y.GetHashCode();
                _hash = hash;
            }

            public int X { get; }
            public int Y { get; }

            #region Operators

            public override bool Equals(object? obj)
            {
                return obj is Point && Equals((Point)obj);
            }

            public bool Equals([AllowNull] Point other)
            {
                return X == other.X && Y == other.Y;
            }

            public override int GetHashCode()
            {
                return _hash;
            }

            #endregion Operators
        }
    }
}
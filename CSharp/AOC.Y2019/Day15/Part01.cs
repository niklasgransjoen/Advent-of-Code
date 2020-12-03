using AOC.Y2019.Day15.P01;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AOC.Y2019.Day15
{
    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            string[] input = context.GetCSVInput();
            long[] intcode = AOCUtils.StringToLong(input);

            var communicator = new Communicator();
            IntcodeInterpreter interpreter = new IntcodeInterpreter(intcode, communicator);
            interpreter.Execute();
        }

        private sealed class Communicator : IIOPort
        {
            private const int WallID = 0;
            private const int MoveID = 1;
            private const int OxygenID = 2;

            private const int North = 1;
            private const int South = 2;
            private const int West = 3;
            private const int East = 4;

            private byte[][] _map;
            private Point _currentPosition;
            private Memory<Point> _currentPath = Memory<Point>.Empty;

            private bool _locatedOxygen;
            private Point _oxygenSystem;

            public Communicator()
            {
                Console.CursorVisible = false;

                _map = ArrayUtility.Create2D<byte>(15, 15);

                _currentPosition = new Point(7, 7);
                _map[7][7] = 1;
            }

            public IOReadResult Read()
            {
                if (_currentPath.IsEmpty)
                    FindNewDestination();

                Point target = _currentPath.Span[0];

                if (target.X > _currentPosition.X)
                    return new IOReadResult(East);
                else if (target.X < _currentPosition.X)
                    return new IOReadResult(West);
                else if (target.Y > _currentPosition.Y)
                    return new IOReadResult(South);
                else
                    return new IOReadResult(North);
            }

            public void RegisterInterpreterForInput(IInterpreter interpreter)
            {
                throw new NotImplementedException();
            }

            public void Write(long value)
            {
                Point target = _currentPath.Span[0];
                _currentPath = _currentPath.Slice(1);

                Console.SetCursorPosition(target.X, target.Y);
                switch (value)
                {
                    case WallID:
                        _map[target.Y][target.X] = 2;
                        _currentPath = Memory<Point>.Empty;

                        WriteWall(target.X, target.Y);
                        break;

                    case MoveID:
                        WriteDiscoveredSpot(_currentPosition.X, _currentPosition.Y);
                        WriteCurrentSpot(target.X, target.Y);
                        if (_locatedOxygen) WriteOxygenSystem(_oxygenSystem.X, _oxygenSystem.Y);

                        _currentPosition = target;
                        _map[target.Y][target.X] = 1;
                        EnsureMapSize();

                        break;

                    case OxygenID:
                        _currentPosition = target;
                        _oxygenSystem = target;
                        CalculateShortestPathToOxygen();
                        break;
                }
            }

            private void CalculateShortestPathToOxygen()
            {
                if (!_locatedOxygen)
                {
                    AOCUtils.Write("Found oxygen system!", ConsoleColor.Cyan);
                    _currentPath = Memory<Point>.Empty;
                    _locatedOxygen = true;
                }
                else
                {
                    int startPos = (_map.Length - 1) / 2;
                    Point start = new Point(startPos, startPos);

                    Point[] pathStartToEnd = AStar(_map, b => b <= 1, start, _oxygenSystem);
                    int steps = pathStartToEnd.Length - 1;

                    AOCUtils.Write($"Shortest path: {steps:N0} steps", ConsoleColor.Cyan);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }

            private void FindNewDestination()
            {
                if (_locatedOxygen)
                {
                    int startPos = (_map.Length - 1) / 2;
                    Point start = new Point(startPos, startPos);

                    Point[] path;
                    if (_currentPosition != start)
                    {
                        path = AStar(_map, p => p <= 1, _currentPosition, start);
                    }
                    else
                    {
                        path = AStar(_map, p => p <= 1, start, _oxygenSystem);
                    }

                    _currentPath = path.AsMemory().Slice(1); // Remove the first entry, as that's the current position.
                    return;
                }

                for (int y = 0; y < _map.Length; y++)
                {
                    for (int x = 0; x < _map[y].Length; x++)
                    {
                        if (_map[y][x] != 0)
                            continue;

                        if (!hasFreeNeighbor(x, y))
                            continue;

                        var path = AStar(_map, p => p <= 1, _currentPosition, new Point(x, y));
                        _currentPath = path.AsMemory().Slice(1); // Remove the first entry, as that's the current position.
                        return;
                    }
                }

                throw new Exception("No new route found.");

                bool hasFreeNeighbor(int x, int y)
                {
                    for (int deltaX = -1; deltaX <= 1; deltaX++)
                    {
                        for (int deltaY = -1; deltaY <= 1; deltaY++)
                        {
                            if (!(deltaX == 0 ^ deltaY == 0))
                                continue;

                            Point p = new Point(x + deltaX, y + deltaY);
                            if (p.X < 0 || p.Y < 0 || p.Y >= _map.Length || p.X >= _map[p.Y].Length)
                                continue;

                            if (_map[p.Y][p.X] < 2)
                                return true;
                        }
                    }

                    return false;
                }
            }

            /// <summary>
            /// If map edge has been reached, double its size (copying previous map in center of new map).
            /// </summary>
            private void EnsureMapSize()
            {
                // If not currently at an edge, there's no reason to continue.
                if (_currentPosition.X != 0 && _currentPosition.Y != 0 &&
                    _currentPosition.X != _map[0].Length && _currentPosition.Y != _map.Length)
                {
                    return;
                }

                // Resize map.
                int newMapSize = _map.Length * 2 + 1;
                byte[][] newMap = ArrayUtility.Create2D<byte>(newMapSize, newMapSize);

                // Copy old map.
                int oldMapOrigin = (_map.Length + 1) / 2;
                for (int y = 0; y < _map.Length; y++)
                {
                    _map[y].CopyTo(newMap[oldMapOrigin + y], oldMapOrigin);
                }
                _map = newMap;

                // Update current position
                _currentPosition = new Point(_currentPosition.X + oldMapOrigin, _currentPosition.Y + oldMapOrigin);
                _currentPath = Memory<Point>.Empty;

                // Write old map
                Console.Clear();
                for (int y = 0; y < _map.Length; y++)
                {
                    for (int x = 0; x < _map[y].Length; x++)
                    {
                        switch (_map[y][x])
                        {
                            case 1:
                                WriteDiscoveredSpot(x, y);
                                break;

                            case 2:
                                WriteWall(x, y);
                                break;

                            default:
                                WriteEmptySpot(x, y);
                                break;
                        }
                    }
                }
            }

            #region WriteUtilities

            private static void WriteWall(int x, int y)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.SetCursorPosition(x, y);
                Console.Write('#');
                Console.ResetColor();
            }

            private static void WriteDiscoveredSpot(int x, int y)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(x, y);
                Console.Write('*');
                Console.ResetColor();
            }

            private static void WriteCurrentSpot(int x, int y)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.SetCursorPosition(x, y);
                Console.Write('O');
                Console.ResetColor();
            }

            private static void WriteOxygenSystem(int x, int y)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.SetCursorPosition(x, y);
                Console.Write('S');
                Console.ResetColor();
            }

            private static void WriteEmptySpot(int x, int y)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(x, y);
                Console.Write('.');
                Console.ResetColor();
            }

            #endregion WriteUtilities
        }

        /// <summary>
        /// Algorithm for finding shortest path between two points in a given 2D map.
        /// </summary>
        private static Point[] AStar<T>(T[][] map, Predicate<T> positionPredicate, in Point start, in Point end)
        {
            HashSet<Point> relevantPositions = new HashSet<Point> { start };

            // Maps every position p to the position preceding it on the cheapest path to p known.
            Dictionary<Point, Point> pathMap = new Dictionary<Point, Point>();

            Dictionary<Point, int> gScoreMap = new Dictionary<Point, int> { [start] = 0 };

            Dictionary<Point, int> fScoreMap = new Dictionary<Point, int> { [start] = CalculateCheapestPath(start, end) };

            while (relevantPositions.Count > 0)
            {
                int minFScore = fScoreMap.Where(f => relevantPositions.Contains(f.Key)).Min(f => f.Value);
                Point current = fScoreMap.Where(f => relevantPositions.Contains(f.Key)).First(pair => pair.Value == minFScore).Key;
                if (current == end)
                {
                    // We've reached the end. Let's construct the final path from the result.
                    Point target = end;

                    int steps = gScoreMap[target];
                    Point[] path = new Point[steps + 1];
                    path[steps] = target;

                    while (pathMap.TryGetValue(target, out target))
                    {
                        path[steps - 1] = target;
                        steps--;
                    }

                    return path;
                }

                relevantPositions.Remove(current);
                for (int deltaX = -1; deltaX <= 1; deltaX++)
                {
                    for (int deltaY = -1; deltaY <= 1; deltaY++)
                    {
                        // Only allow cells in the Von Neumann neighborhood
                        if (!(deltaX == 0 ^ deltaY == 0))
                            continue;

                        // Check bounds.
                        Point neighbor = new Point(current.X + deltaX, current.Y + deltaY);
                        if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.Y >= map.Length || neighbor.X >= map[neighbor.Y].Length)
                            continue;

                        // Check if route is blocked..
                        bool pathIsFree = positionPredicate(map[neighbor.Y][neighbor.X]);
                        if (!pathIsFree)
                            continue;

                        int gScore = gScoreMap.GetValueOrDefault(current, int.MaxValue - 1) + 1;
                        if (!gScoreMap.TryGetValue(neighbor, out int currentGScore) || gScore < currentGScore)
                        {
                            pathMap[neighbor] = current;
                            gScoreMap[neighbor] = gScore;
                            fScoreMap[neighbor] = gScoreMap[neighbor] + CalculateCheapestPath(neighbor, end);
                            relevantPositions.Add(neighbor);
                        }
                    }
                }
            }

            throw new Exception("Failed to find path");

            /// Calculates the cost of the cheapest direct path between a and b.
            static int CalculateCheapestPath(Point a, Point b)
            {
                return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
            }
        }

        [DebuggerDisplay("({X}, {Y})")]
        private readonly struct Point : IEquatable<Point>
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }

            public override bool Equals(object? obj)
            {
                return obj is Point point && Equals(point);
            }

            public bool Equals(Point other)
            {
                return X == other.X &&
                       Y == other.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            public static bool operator ==(Point left, Point right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Point left, Point right)
            {
                return !left.Equals(right);
            }
        }
    }
}
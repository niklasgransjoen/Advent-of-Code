using AOC.Y2019.Day15.P02;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AOC.Y2019.Day15
{
    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            string[] input = context.GetCSVInput();
            long[] intcode = AOCUtils.StringToLong(input);

            var communicator = new Communicator();
            IntcodeInterpreter interpreter = new IntcodeInterpreter(intcode, communicator);

            Console.WriteLine("Press enter to execute.");
            Console.ReadLine();
            Console.Clear();

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
                _map = ArrayUtility.Create2D<byte>(3, 3);

                WriteCurrentSpot(1, 1);
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

                switch (value)
                {
                    case WallID:
                        _currentPath = Memory<Point>.Empty;
                        WriteWall(target.X, target.Y);
                        break;

                    case MoveID:
                        WriteCurrentSpot(target.X, target.Y);
                        EnsureMapSize();
                        break;

                    case OxygenID:
                        _oxygenSystem = target;
                        _locatedOxygen = true;

                        WriteCurrentSpot(target.X, target.Y);
                        EnsureMapSize();
                        break;
                }
            }

            private void FindNewDestination()
            {
                for (int y = 0; y < _map.Length; y++)
                {
                    for (int x = 0; x < _map[y].Length; x++)
                    {
                        if (_map[y][x] > 0)
                            continue;

                        if (_currentPosition.X == x && _currentPosition.Y == y)
                            continue;

                        Point[]? path = AStar(_map, p => p <= 1, _currentPosition, new Point(x, y));
                        if (path is null)
                        {
                            WriteUnreachable(x, y);
                            continue;
                        }

                        _currentPath = path.AsMemory().Slice(1);
                        return;
                    }
                }

                Console.WriteLine();
                AOCUtils.WriteLine("All areas on the map have been discovered.", ConsoleColor.Magenta);
                LocateDestinationFurthestFromOxygenSystem();
            }

            private void LocateDestinationFurthestFromOxygenSystem()
            {
                int maxSteps = 0;
                for (int y = 0; y < _map.Length; y++)
                {
                    for (int x = 0; x < _map[y].Length; x++)
                    {
                        if (_map[y][x] > 1)
                            continue;

                        Point[]? path = AStar(_map, p => p <= 1, _oxygenSystem, new Point(x, y));
                        if (path is null)
                            continue;

                        int steps = path.Length - 1;
                        if (steps > maxSteps)
                            maxSteps = steps;
                    }
                }

                AOCUtils.WriteLine($"It takes {maxSteps:N0} minutes to refill the area with oxygen.", ConsoleColor.Magenta);
                Console.ReadLine();
                Environment.Exit(0);
            }

            /// <summary>
            /// If map edge has been reached, increase it's size by a factor in that direction.
            /// </summary>
            private void EnsureMapSize()
            {
                const int mapIncreateFactor = 4;

                int mapHeight = _map.Length;
                int mapWidth = _map[0].Length;
                int xOffset = 0;
                int yOffset = 0;

                if (_currentPosition.Y == 0)
                {
                    mapHeight += mapIncreateFactor;
                    yOffset = mapIncreateFactor;
                }
                else if (_currentPosition.X == 0)
                {
                    mapWidth += mapIncreateFactor;
                    xOffset = mapIncreateFactor;
                }
                else if (_currentPosition.Y == mapHeight - 1)
                {
                    mapHeight += mapIncreateFactor;
                }
                else if (_currentPosition.X == mapWidth - 1)
                {
                    mapWidth += mapIncreateFactor;
                }
                else return;

                // Resize map.
                byte[][] newMap = ArrayUtility.Create2D<byte>(mapWidth, mapHeight);

                // Copy old map.
                for (int y = 0; y < _map.Length; y++)
                {
                    // remove unreachable areas, as they might now be reachable.
                    for (int x = 0; x < _map[y].Length; x++)
                    {
                        if (_map[y][x] == 3)
                            _map[y][x] = 0;
                    }

                    _map[y].CopyTo(newMap[y + yOffset], xOffset);
                }
                _map = newMap;

                // Update current position
                _currentPosition = new Point(_currentPosition.X + xOffset, _currentPosition.Y + yOffset);
                _currentPath = Memory<Point>.Empty;

                RefreshVisualMap();
            }

            #region WriteUtilities

            private void RefreshVisualMap()
            {
                Console.Clear();
                for (int y = 0; y < _map.Length; y++)
                {
                    for (int x = 0; x < _map[y].Length; x++)
                    {
                        switch (_map[y][x])
                        {
                            case 1:
                                WriteDiscoveredSpot(x, y, consoleOnly: true);
                                break;

                            case 2:
                                WriteWall(x, y, consoleOnly: true);
                                break;

                            case 3:
                                WriteUnreachable(x, y, consoleOnly: true);
                                break;

                            default:
                                WriteEmptySpot(x, y);
                                break;
                        }
                    }
                }
                WriteCurrentSpot(_currentPosition.X, _currentPosition.Y, consoleOnly: true);
            }

            private static void WriteEmptySpot(int x, int y)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.SetCursorPosition(x, y);
                Console.Write('.');
                Console.ResetColor();
            }

            private void WriteDiscoveredSpot(int x, int y, bool consoleOnly = false)
            {
                if (!consoleOnly)
                {
                    _map[y][x] = 1;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.SetCursorPosition(x, y);
                Console.Write('*');
                Console.ResetColor();
            }

            private void WriteWall(int x, int y, bool consoleOnly = false)
            {
                if (!consoleOnly)
                {
                    _map[y][x] = 2;
                }

                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.SetCursorPosition(x, y);
                Console.Write('#');
                Console.ResetColor();
            }

            private void WriteUnreachable(int x, int y, bool consoleOnly = false)
            {
                if (!consoleOnly)
                {
                    _map[y][x] = 3;
                }

                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.DarkMagenta;

                Console.SetCursorPosition(x, y);
                Console.Write('#');
                Console.ResetColor();
            }

            private void WriteCurrentSpot(int x, int y, bool consoleOnly = false)
            {
                WriteDiscoveredSpot(_currentPosition.X, _currentPosition.Y, consoleOnly: true);
                if (_locatedOxygen)
                    WriteOxygenSystem(_oxygenSystem.X, _oxygenSystem.Y);

                if (!consoleOnly)
                {
                    _map[y][x] = 1;
                    _currentPosition = new Point(x, y);
                }

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

            #endregion WriteUtilities
        }

        /// <summary>
        /// Algorithm for finding shortest path between two points in a given 2D map.
        /// </summary>
        private static Point[]? AStar<T>(T[][] map, Predicate<T> positionPredicate, in Point start, in Point end)
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

            return null;

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
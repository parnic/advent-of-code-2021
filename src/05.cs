using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc2021
{
    internal class Day05
    {
        private static readonly Regex lineRegex = new(@"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)", RegexOptions.Compiled);

        [DebuggerDisplay("{x},{y}")]
        struct Point : IEquatable<Point>
        {
            public int X { get; init; }
            public int Y { get; init; }

            public bool Equals(Point other) => X == other.X && Y == other.Y;

            public override bool Equals(object obj) => obj is Point point && Equals(point);

            public override int GetHashCode() => HashCode.Combine(X, Y);
        }

        [DebuggerDisplay("{start} -> {end}")]
        struct Line
        {
            public Point Start { get; init; }
            public Point End { get; init; }

            public int Length
            {
                get
                {
                    // this is bad and i feel bad
                    if (Start.X == End.X || Start.Y == End.Y)
                    {
                        return Math.Abs((End.X - Start.X) + (End.Y - Start.Y));
                    }

                    return Math.Abs(Start.X - End.X);
                }
            }
        }

        internal static void Go()
        {
            Logger.Log("Day 5");
            Logger.Log("-----");
            var lines = File.ReadAllLines("inputs/05.txt");
            List<Line> segments = new();
            foreach (var line in lines)
            {
                var match = lineRegex.Match(line);
                Line segment = new()
                {
                    Start = new Point()
                    {
                        X = Convert.ToInt32(match.Groups["x1"].Value),
                        Y = Convert.ToInt32(match.Groups["y1"].Value),
                    },
                    End = new Point()
                    {
                        X = Convert.ToInt32(match.Groups["x2"].Value),
                        Y = Convert.ToInt32(match.Groups["y2"].Value)
                    },
                };
                segments.Add(segment);
            }

            Part1(segments);
            Part2(segments);
            Logger.Log("");
        }

        private static void Part1(IEnumerable<Line> lines)
        {
            using var t = new Timer();
            int numPointsGreater = Solve(lines, (line) => !(line.Start.X == line.End.X || line.Start.Y == line.End.Y));
            Logger.Log($"part1: {numPointsGreater}");
        }

        private static void Part2(IEnumerable<Line> lines)
        {
            using var t = new Timer();
            int numPointsGreater = Solve(lines, (line) => false);
            Logger.Log($"part2: {numPointsGreater}");
        }

        private static int Solve(IEnumerable<Line> lines, Func<Line, bool> filter)
        {
            Dictionary<Point, int> coveredPoints = new();
            int numPointsGreater = 0;
            foreach (var line in lines)
            {
                if (filter(line))
                {
                    continue;
                }

                for (int i = 0; i <= line.Length; i++)
                {
                    int x = line.Start.X;
                    int y = line.Start.Y;
                    if (line.Start.X != line.End.X)
                    {
                        x += (line.Start.X > line.End.X ? -1 : 1) * i;
                    }
                    if (line.Start.Y != line.End.Y)
                    {
                        y += (line.Start.Y > line.End.Y ? -1 : 1) * i;
                    }

                    Point point = new() { X = x, Y = y };
                    if (!coveredPoints.TryGetValue(point, out int curr))
                    {
                        coveredPoints.Add(point, curr);
                    }

                    if (curr == 1)
                    {
                        numPointsGreater++;
                    }

                    coveredPoints[point] = curr + 1;
                }
            }

            return numPointsGreater;
        }
    }
}

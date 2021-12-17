using System.Diagnostics;

namespace aoc2021;

internal class Day17 : Day
{
    [DebuggerDisplay("{minX},{minY} -> {maxX},{maxY}")]
    private struct Rectangle
    {
        public int minX = 0;
        public int minY = 0;
        public int maxX = 0;
        public int maxY = 0;

        public bool Contains((int x, int y) pt) => pt.x >= minX && pt.x <= maxX && pt.y >= minY && pt.y <= maxY;
        public bool Beyond((int x, int y) pt) => IsFar(pt) || IsLow(pt);
        public bool IsShort((int x, int y) pt) => pt.x < minX;
        public bool IsFar((int x, int y) pt) => pt.x > maxX;
        public bool IsHigh((int x, int y) pt) => pt.y > maxY;
        public bool IsLow((int x, int y) pt) => pt.y < minY;
        public bool XInRange((int x, int y) pt) => !IsShort(pt) && !IsFar(pt);
        public bool XInRange(int x) => XInRange((x, 0));
        public bool YInRange((int x, int y) pt) => !IsHigh(pt) && !IsLow(pt);
        public bool YInRange(int x) => YInRange((x, 0));
    }

    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/17.txt");
        var instructions = lines.ElementAt(0).Split(", ");

        Rectangle bounds = new();

        var xs = instructions[0].Split("..");
        bounds.minX = Convert.ToInt32(xs[0][(xs[0].IndexOf('=') + 1)..]);
        bounds.maxX = Convert.ToInt32(xs[1]);

        var ys = instructions[1].Split("..");
        bounds.minY = Convert.ToInt32(ys[0][(ys[0].IndexOf('=') + 1)..]);
        bounds.maxY = Convert.ToInt32(ys[1]);

        using var t = new Timer();
        var successes = GetSuccessfulVelocities(bounds);
        t.Stop();

        Part1(successes);
        Part2(successes);
    }

    private static (int x, int y) Step((int x, int y) pt, ref (int x, int y) velocity)
    {
        pt.x += velocity.x;
        pt.y += velocity.y;
        if (velocity.x > 0)
        {
            velocity.x--;
        }
        else if (velocity.x < 0)
        {
            velocity.x++;
        }
        velocity.y--;

        return (pt.x, pt.y);
    }

    private static void Part1(List<((int x, int y) pt, int height)> successes)
    {
        var (pt, height) = successes.MaxBy(vel => vel.height);
        var numOthers = successes.Count(vel => vel.height == height);

        Logger.Log($"<+black>> part1: highest Y was at velocity {pt} (and {numOthers} other{(numOthers == 1 ? "" : "s")}): <+white>{height}<r>");
    }

    private static void Part2(List<((int x, int y) pt, int height)> successes)
    {
        Logger.Log($"<+black>> part2: #successful velocities: <+white>{successes.Count}<r>");
    }

    private static List<((int x, int y) pt, int height)> GetSuccessfulVelocities(Rectangle bounds)
    {
        var (minX, maxX) = GetXVelocityRange(bounds);
        var (minY, maxY) = GetYVelocityRange(bounds);

        (int x, int y) pt;
        List<((int x, int y) pt, int height)> successes = new();
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                pt = (0, 0);
                var startingVelocity = (x, y);
                var velocity = startingVelocity;
                int currHighestY = int.MinValue;
                while (!bounds.Beyond(pt) && !bounds.Contains(pt))
                {
                    pt = Step(pt, ref velocity);
                    if (pt.y > currHighestY)
                    {
                        currHighestY = pt.y;
                    }
                }

                if (bounds.Contains(pt))
                {
                    successes.Add((startingVelocity, currHighestY));
                }
            }
        }

        return successes;
    }

    private static (int min, int max) GetXVelocityRange(Rectangle bounds)
    {
        var minSuccessXVel = int.MaxValue;
        for (var guess = bounds.maxX; guess > 0; guess--)
        {
            var guesspt = (0, 0);
            var testVel = (x: guess, 0);
            while (testVel.x > 0)
            {
                guesspt = Step(guesspt, ref testVel);

                if (bounds.XInRange(guesspt))
                {
                    if (guess < minSuccessXVel)
                    {
                        minSuccessXVel = guess;
                    }
                    break;
                }
            }
        }

        return (minSuccessXVel, bounds.maxX);
    }

    private static (int min, int max) GetYVelocityRange(Rectangle bounds)
    {
        int maxSuccessYVel = int.MinValue;
        int maxVelY = Math.Abs(bounds.minY) - 1;
        int guess = bounds.minY;
        while (guess <= maxVelY)
        {
            var guesspt = (0, 0);
            var testVel = (0, guess);
            while (true)
            {
                guesspt = Step(guesspt, ref testVel);
                if (bounds.YInRange(guesspt))
                {
                    if (guess > maxSuccessYVel)
                    {
                        maxSuccessYVel = guess;
                    }
                    break;
                }
                else if (bounds.IsLow(guesspt))
                {
                    break;
                }
            }

            guess++;
        }

        return (bounds.minY, maxSuccessYVel);
    }
}

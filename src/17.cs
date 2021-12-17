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

        Part1(bounds);
        Part2(bounds);
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

    private static void Part1(Rectangle bounds)
    {
        using var t = new Timer();

        var successes = GetSuccessfulVelocities(bounds);
        var max = successes.MaxBy(vel => vel.Value);
        var numOthers = successes.Count(vel => vel.Value == max.Value);

        Logger.Log($"<+black>> part1: highest Y was at velocity {max.Key} (and {numOthers} other{(numOthers == 1 ? "" : "s")}): <+white>{max.Value}<r>");
    }

    private static void Part2(Rectangle bounds)
    {
        using var t = new Timer();

        var successes = GetSuccessfulVelocities(bounds);

        Logger.Log($"<+black>> part2: #successful velocities: <+white>{successes.Count}<r>");
    }

    private static Dictionary<(int x, int y), int> GetSuccessfulVelocities(Rectangle bounds)
    {
        var (minX, maxX) = GetXVelocityRange(bounds);
        var (minY, maxY) = GetYVelocityRange(bounds);

        (int x, int y) pt;
        Dictionary<(int x, int y), int> successes = new();
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
                    successes.Add(startingVelocity, currHighestY);
                }
            }
        }

        return successes;
    }

    private static (int min, int max) GetXVelocityRange(Rectangle bounds)
    {
        var minSuccessXVel = int.MaxValue;
        for (var guess = bounds.maxX; ; guess--)
        {
            var guesspt = (0, 0);
            var testVel = (x: guess, 0);
            while (testVel.x > 0)
            {
                guesspt = Step(guesspt, ref testVel);
            }

            if (bounds.IsShort(guesspt))
            {
                break;
            }
            else if (!bounds.IsFar(guesspt))
            {
                if (guess < minSuccessXVel)
                {
                    minSuccessXVel = guess;
                }
            }
        }

        return (minSuccessXVel, bounds.maxX);
    }

    private static (int min, int max) GetYVelocityRange(Rectangle bounds)
    {
        int maxSuccessYVel = int.MinValue;
        int guess = bounds.minY;
        bool foundYVals = false;
        while (!foundYVals)
        {
            var guesspt = (0, 0);
            var testVel = (0, guess);
            bool lastWasAbove = false;
            while (true)
            {
                guesspt = Step(guesspt, ref testVel);
                if (bounds.IsHigh(guesspt))
                {
                    lastWasAbove = true;
                }
                else if (!bounds.IsLow(guesspt))
                {
                    if (guess > maxSuccessYVel)
                    {
                        maxSuccessYVel = guess;
                    }
                    break;
                }
                else
                {
                    // need to find a better way to choose a reasonable guess max...
                    if (lastWasAbove && guess > bounds.minY * -4)
                    {
                        foundYVals = true;
                    }

                    break;
                }
            }

            guess++;
        }

        return (bounds.minY, maxSuccessYVel);
    }
}

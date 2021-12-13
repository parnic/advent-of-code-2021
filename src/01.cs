namespace aoc2021;

internal class Day01 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/01.txt");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
        using var t = new Timer();

        int lastDepth = 0;
        int numIncreased = -1;

        foreach (var line in lines)
        {
            var depth = Convert.ToInt32(line);
            if (depth > lastDepth)
            {
                numIncreased++;
            }

            lastDepth = depth;
        }

        Logger.Log($"part1: <blue>{numIncreased}<r>");
    }

    private static void Part2(IEnumerable<string> lines)
    {
        using var t = new Timer();

        int lastTotal = 0;
        int numIncreased = -1;
        int num1 = -1;
        int num2 = -1;
        int num3 = -1;

        foreach (var line in lines)
        {
            var depth = Convert.ToInt32(line);
            num1 = num2;
            num2 = num3;
            num3 = depth;

            if (num1 < 0 || num2 < 0 || num3 < 0)
            {
                continue;
            }

            var total = num1 + num2 + num3;
            if (total > lastTotal)
            {
                numIncreased++;
            }

            lastTotal = total;
        }

        Logger.Log($"part2: <blue>{numIncreased}<r>");
    }
}

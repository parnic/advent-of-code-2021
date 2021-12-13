namespace aoc2021;

internal class DayTemplate : Day
{
    internal override void Go()
    {
        Logger.Log("Day #");
        Logger.Log("-----");
        var lines = Util.ReadAllLines("inputs/##.txt");
        Part1(lines);
        Part2(lines);
        Logger.Log("");
    }

    private static void Part1(IEnumerable<string> lines)
    {
        using var t = new Timer();

        Logger.Log($"part1: <blue><r>");
    }

    private static void Part2(IEnumerable<string> lines)
    {
        using var t = new Timer();

        Logger.Log($"part2: <blue><r>");
    }
}

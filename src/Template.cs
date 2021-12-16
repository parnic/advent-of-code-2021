namespace aoc2021;

internal class DayTemplate : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/##.txt");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
        using var t = new Timer();

        Logger.Log($"<+black>> part1: <+white><r>");
    }

    private static void Part2(IEnumerable<string> lines)
    {
        using var t = new Timer();

        Logger.Log($"<+black>> part2: <+white><r>");
    }
}

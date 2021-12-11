namespace aoc2021;

internal class DayTemplate
{
    internal static void Go()
    {
        Logger.Log("Day #");
        Logger.Log("-----");
        var lines = File.ReadAllLines("inputs/##.txt");
        Part1(lines);
        Part2(lines);
        Logger.Log("");
    }

    private static void Part1(IEnumerable<string> lines)
    {
        using var t = new Timer();

        Logger.Log($"part1: ");
    }

    private static void Part2(IEnumerable<string> lines)
    {
        using var t = new Timer();

        Logger.Log($"part2: ");
    }
}

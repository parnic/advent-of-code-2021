namespace aoc2021;

internal class Day07 : Day
{
    internal override void Go()
    {
        var nums = File.ReadAllText("inputs/07.txt").Split(',').Select(int.Parse);
        Part1(nums);
        Part2(nums);
    }

    private static (long, int) Solve(IEnumerable<int> nums, Func<long, long> formula)
    {
        var min = nums.Min();
        var max = nums.Max();
        long minDist = long.MaxValue;
        int minNum = 0;
        for (int i = min; i <= max; i++)
        {
            long dist = 0;
            foreach (var num in nums)
            {
                dist += formula(Math.Abs(num - i));
            }

            if (dist < minDist)
            {
                minDist = dist;
                minNum = i;
            }
        }

        return (minDist, minNum);
    }

    private static void Part1(IEnumerable<int> nums)
    {
        using var t = new Timer();

        var (minDist, minNum) = Solve(nums, (d) => d);

        t.Stop();
        Logger.Log($"<+black>> part1: position: {minNum}, fuel cost: <+white>{minDist}<r>");
    }

    private static void Part2(IEnumerable<int> nums)
    {
        using var t = new Timer();

        // summation formula from https://en.wikipedia.org/wiki/Summation
        // found by searching "factorial but with addition" because i'm smart like that.
        var (minDist, minNum) = Solve(nums, (d) => ((d * d) + d) / 2);

        t.Stop();
        Logger.Log($"<+black>> part2: position: {minNum}, fuel cost: <+white>{minDist}<r>");
    }
}

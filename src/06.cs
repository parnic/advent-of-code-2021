using System.Diagnostics;

namespace aoc2021;

internal class Day06 : Day
{
    [DebuggerDisplay("{State}")]
    struct Fish
    {
        public int State;
    }

    internal override void Go()
    {
        Logger.Log("Day 6");
        Logger.Log("-----");
        var input = File.ReadAllText("inputs/06.txt");
        List<Fish> fish = new();
        foreach (var state in input.Split(','))
        {
            fish.Add(new Fish() { State = Convert.ToInt32(state) });
        }
        Part1(fish);
        Part2(fish);
        Logger.Log("");
    }

    private static void Part1(IEnumerable<Fish> fish)
    {
        using var t = new Timer();

        var list = fish.ToList();
        // brute force method (my initial solution)
        for (int day = 0; day < 80; day++)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Fish f = list[i];
                if (f.State == 0)
                {
                    list.Add(new Fish() { State = 8 });
                    f.State = 7;
                }

                f.State--;
                list[i] = f;
            }
        }

#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available - Count is of type int, list might be longer than that
        Logger.Log($"part1: #fish={list.LongCount()}");
#pragma warning restore CA1829 // Use Length/Count property instead of Count() when available
    }

    private static void Part2(IEnumerable<Fish> fish)
    {
        using var t = new Timer();

        // fast method (when brute force threatened to blow RAM and take way too long)
        Dictionary<int, long> fishAtState = new();
        for (int i = 0; i <= 8; i++)
        {
            fishAtState[i] = fish.Count(x => x.State == i);
        }

        for (int day = 0; day < 256; day++)
        {
            var adders = fishAtState[0];
            for (int i = 0; i < 8; i++)
            {
                fishAtState[i] = fishAtState[i + 1];
            }

            fishAtState[6] += adders;
            fishAtState[8] = adders;
        }

        Logger.Log($"part2: #fish={fishAtState.Values.Sum()}");
    }
}

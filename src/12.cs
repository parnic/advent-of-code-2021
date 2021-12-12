namespace aoc2021;

internal class Day12
{
    internal static void Go()
    {
        Logger.Log("Day 12");
        Logger.Log("-----");
        var lines = File.ReadAllLines("inputs/12.txt");
        var paths = new Dictionary<string, List<string>>();
        foreach (var line in lines)
        {
            var points = line.Split('-');
            if (points[0] == "start" || points[0] == "end")
            {
                points[0] = points[0].ToUpper();
            }
            if (points[1] == "start" || points[1] == "end")
            {
                points[1] = points[1].ToUpper();
            }

            if (!paths.ContainsKey(points[0]))
            {
                paths[points[0]] = new List<string>();
            }
            if (!paths.ContainsKey(points[1]))
            {
                paths[points[1]] = new List<string>();
            }

            if (points[0] != "END" && points[1] != "START")
            {
                paths[points[0]].Add(points[1]);
            }
            if (points[1] != "END" && points[0] != "START")
            {
                paths[points[1]].Add(points[0]);
            }
        }
        Part1(paths);
        Part2(paths);
        Logger.Log("");
    }

    private static void Part1(Dictionary<string, List<string>> paths)
    {
        using var t = new Timer();

        var validPaths = new List<List<string>>();
        FindPaths(paths, validPaths, new List<string>(){ "START" }, false);

        Logger.Log($"part1: {validPaths.Count}");
        //validPaths.ForEach(path => Logger.Log($"  {string.Join(',', path)}"));
    }

    private static void FindPaths(Dictionary<string, List<string>> paths, List<List<string>> routes, List<string> currRoute, bool canVisitSmallCaveTwice, bool hasDoubledCave = false)
    {
        //Logger.Log($"Current path: {string.Join(',', currRoute)}");
        var curr = currRoute.Last();
        bool justDoubled = false;
        foreach (var next in paths[curr])
        {
            //Logger.Log($"  Evaluating next: {next}");
            if (IsSmallCave(next) && currRoute.Contains(next))
            {
                if (!canVisitSmallCaveTwice || hasDoubledCave)
                {
                    //Logger.Log($"    is already-visited small cave, skipping");
                    continue;
                }

                hasDoubledCave = true;
                justDoubled = true;
            }

            //Logger.Log($"    adding to route");
            currRoute.Add(next);
            if (next == "END")
            {
                //Logger.Log($"    is end, so adding {string.Join(',', currRoute)} to valid paths");
                routes.Add(new List<string>(currRoute));
            }
            else
            {
                FindPaths(paths, routes, currRoute, canVisitSmallCaveTwice, hasDoubledCave);
            }

            currRoute.RemoveAt(currRoute.Count - 1);
            if (justDoubled)
            {
                hasDoubledCave = false;
            }
        }

        //Logger.Log("  done with route");
    }

    private static bool IsSmallCave(string cave) => cave[0] >= 'a' && cave[0] <= 'z';

    private static void Part2(Dictionary<string, List<string>> paths)
    {
        using var t = new Timer();

        var validPaths = new List<List<string>>();
        FindPaths(paths, validPaths, new List<string>() { "START" }, true);

        Logger.Log($"part2: {validPaths.Count}");
        //validPaths.ForEach(path => Logger.Log($"  {string.Join(',', path)}"));
    }
}

namespace aoc2021;

internal class Day11
{
    internal static void Go()
    {
        Logger.Log("Day 11");
        Logger.Log("-----");

        var lines = File.ReadAllLines("inputs/11.txt");
        var grid = new byte[lines.Length, lines[0].Length];
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                var num = line[j];
                grid[i, j] = (byte)char.GetNumericValue(num);
            }
        }

        //var gridCopy = new byte[lines.Length, lines[0].Length];
        //Buffer.BlockCopy(grid, 0, gridCopy, 0, grid.Length);

        Part1(grid);
        Part2(grid);

        Logger.Log("");
    }

    private static IEnumerable<(int, int)> Adjacent(byte[,] grid, int i, int j)
    {
        if (i > 0)
        {
            if (j > 0)
            {
                yield return (i - 1, j - 1);
            }
            yield return (i - 1, j);
            if (j < grid.GetLength(1) - 1)
            {
                yield return (i - 1, j + 1);
            }
        }
        if (j > 0)
        {
            yield return (i, j - 1);
        }
        if (j < grid.GetLength(1) - 1)
        {
            yield return (i, j + 1);
        }
        if (i < grid.GetLength(1) - 1)
        {
            if (j > 0)
            {
                yield return (i + 1, j - 1);
            }
            yield return (i + 1, j);
            if (j < grid.GetLength(1) - 1)
            {
                yield return (i + 1, j + 1);
            }
        }
    }

    private static void Flash(byte[,] grid, int i, int j, List<(int, int)> flashed)
    {
        flashed.Add((i, j));
        foreach (var pt in Adjacent(grid, i, j))
        {
            grid[pt.Item1, pt.Item2]++;
            if (grid[pt.Item1, pt.Item2] == 10)
            {
                Flash(grid, pt.Item1, pt.Item2, flashed);
            }
        }
    }

    private static void Part1(byte[,] grid)
    {
        using var t = new Timer();

        long numFlashes = 0;
        for (int step = 0; step < 100; step++)
        {
            var flashed = new List<(int, int)>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j]++;
                    if (grid[i, j] == 10)
                    {
                        Flash(grid, i, j, flashed);
                    }
                }
            }

            foreach (var pt in flashed)
            {
                grid[pt.Item1, pt.Item2] = 0;
                numFlashes++;
            }
        }

        Logger.Log($"part1: {numFlashes}");
    }

    private static void Part2(byte[,] grid)
    {
        using var t = new Timer();

        int step = 101;
        for (; ; step++)
        {
            var flashed = new List<(int, int)>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j]++;
                    if (grid[i, j] == 10)
                    {
                        Flash(grid, i, j, flashed);
                    }
                }
            }

            if (flashed.Count == grid.GetLength(0) * grid.GetLength(1))
            {
                break;
            }
            foreach (var pt in flashed)
            {
                grid[pt.Item1, pt.Item2] = 0;
            }
        }

        Logger.Log($"part2: {step}");
    }
}

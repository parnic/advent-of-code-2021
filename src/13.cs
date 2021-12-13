namespace aoc2021;

internal class Day13 : Day
{
    internal override void Go()
    {
        Logger.Log("Day 13");
        Logger.Log("-----");
        var lines = File.ReadAllLines("inputs/13.txt");
        int phase = 0;
        var points = new List<(int, int)>();
        var folds = new List<(char, int)>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                phase++;
                continue;
            }

            if (phase == 0)
            {
                var coords = line.Split(',');
                points.Add((Convert.ToInt32(coords[1]), Convert.ToInt32(coords[0])));
            }
            else
            {
                var instruction = line.Split('=');
                folds.Add((instruction[0].Last(), Convert.ToInt32(instruction[1])));
            }
        }
        var grid = new bool[points.Max(x => x.Item1) + 1, points.Max(x => x.Item2) + 1];
        foreach (var point in points)
        {
            grid[point.Item1, point.Item2] = true;
        }
        Part1(grid, folds);
        Part2(grid, folds);
        Logger.Log("");
    }

    private static void Part1(bool[,] grid, IEnumerable<(char, int)> folds)
    {
        using var t = new Timer();

        int maxX = grid.GetLength(0);
        int maxY = grid.GetLength(1);

        foreach (var fold in folds)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var newX = i;
                    var newY = j;
                    if (fold.Item1 == 'y' && newX > fold.Item2)
                    {
                        newX = maxX - 1 - i;
                    }
                    if (fold.Item1 == 'x' && newY > fold.Item2)
                    {
                        newY = maxY - 1 - j;
                    }

                    if (grid[i,j])
                    {
                        grid[newX, newY] = grid[i, j];
                    }
                }
            }

            if (fold.Item1 == 'x')
            {
                maxY -= fold.Item2 + 1;
            }
            else if (fold.Item1 == 'y')
            {
                maxX -= fold.Item2 + 1;
            }

            break;
        }

        int numDots = 0;
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                if (grid[i, j] == true)
                {
                    numDots++;
                }
            }
        }
        Logger.Log($"part1: {numDots}");
    }

    private static void Part2(bool[,] grid, IEnumerable<(char, int)> folds)
    {
        using var t = new Timer();

        int maxX = grid.GetLength(0);
        int maxY = grid.GetLength(1);

        for (int foldNum = 0; foldNum < folds.Count(); foldNum++)
        {
            var fold = folds.ElementAt(foldNum);
            if (foldNum > 0)
            {
                var iStart = 0;
                var jStart = 0;
                if (fold.Item1 == 'x')
                {
                    jStart = fold.Item2 + 1;
                }
                if (fold.Item1 == 'y')
                {
                    iStart = fold.Item2 + 1;
                }
                for (int i = iStart; i < maxX; i++)
                {
                    for (int j = jStart; j < maxY; j++)
                    {
                        var newX = i;
                        var newY = j;
                        if (fold.Item1 == 'y' && newX > fold.Item2)
                        {
                            newX = maxX - 1 - i;
                        }
                        if (fold.Item1 == 'x' && newY > fold.Item2)
                        {
                            newY = maxY - 1 - j;
                        }

                        if (grid[i, j])
                        {
                            grid[newX, newY] = grid[i, j];
                        }
                    }
                }
            }

            if (fold.Item1 == 'x')
            {
                maxY -= fold.Item2 + 1;
            }
            else if (fold.Item1 == 'y')
            {
                maxX -= fold.Item2 + 1;
            }
        }

        int numDots = 0;
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                if (grid[i, j] == true)
                {
                    Console.Write("#");
                    numDots++;
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
        Logger.Log($"part2: {numDots}");
    }
}

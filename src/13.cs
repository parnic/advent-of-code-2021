namespace aoc2021;

internal class Day13 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/13.txt");
        int phase = 0;
        var points = new HashSet<(int, int)>();
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
        Part1(points, folds);
        Part2(points, folds);
    }

    private static void Fold(ICollection<(int x, int y)> points, char axis, int line)
    {
        if (axis == 'x')
        {
            points.Where(x => x.y > line).ToList().ForEach(x =>
            {
                points.Add((x.x, x.y - (x.y - line) * 2));
                points.Remove(x);
            });
        }
        else
        {
            points.Where(x => x.x > line).ToList().ForEach(x =>
            {
                points.Add((x.x - (x.x - line) * 2, x.y));
                points.Remove(x);
            });
        }
    }

    private static void Part1(ICollection<(int x, int y)> grid, IList<(char axis, int line)> folds)
    {
        using var t = new Timer();

        Fold(grid, folds[0].axis, folds[0].line);

        t.Stop();
        Logger.Log($"<+black>> part1: <+white>{grid.Count}<r>");
    }

    private static void Part2(ICollection<(int x, int y)> grid, IList<(char axis, int line)> folds)
    {
        using var t = new Timer();

        folds.Skip(1).ForEach(x => Fold(grid, x.axis, x.line));
        int maxX = grid.Max(x => x.x);
        int maxY = grid.Max(x => x.y);

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i <= maxX; i++)
        {
            for (int j = 0; j <= maxY; j++)
            {
                if (grid.Contains((i, j)))
                {
                    sb.Append('█');
                }
                else
                {
                    sb.Append(' ');
                }
            }
            if (i < maxX)
            {
                sb.Append('\n');
            }
        }

        t.Stop();
        Logger.Log($"<+white>{sb}<r>");
        Logger.Log($"<+black>> part2: {grid.Count}<r>");
    }
}

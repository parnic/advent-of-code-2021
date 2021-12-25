namespace aoc2021;

internal class Day25 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/25.txt");
        var grid = new byte[lines.Count(), lines.ElementAt(0).Length];
        for (int i = 0; i < lines.Count(); i++)
        {
            var line = lines.ElementAt(i);
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == '>')
                {
                    grid[i, j] = 1;
                }
                else if (line[j] == 'v')
                {
                    grid[i, j] = 2;
                }
            }
        }
        Part1(grid);
    }

    private static void Part1(byte[,] grid)
    {
        using var t = new Timer();

        var xlen = grid.GetLength(0);
        var ylen = grid.GetLength(1);

        bool stop = false;
        int step;
        for (step = 0; !stop; step++)
        {
            List<(int x, int y)> toMove = new();
            for (int i = 0; i < xlen; i++)
            {
                for (int j = 0; j < ylen; j++)
                {
                    if (grid[i,j] == 1)
                    {
                        if (grid[i,(j + 1) % ylen] == 0)
                        {
                            toMove.Add((i, j));
                        }
                    }
                }
            }

            var eastMoves = toMove.Count;
            foreach (var (x, y) in toMove)
            {
                grid[x, (y + 1) % ylen] = grid[x, y];
                grid[x, y] = 0;
            }
            toMove.Clear();

            for (int i = 0; i < xlen; i++)
            {
                for (int j = 0; j < ylen; j++)
                {
                    if (grid[i, j] == 2)
                    {
                        if (grid[(i + 1) % xlen, j] == 0)
                        {
                            toMove.Add((i, j));
                        }
                    }
                }
            }

            if (eastMoves == 0 && !toMove.Any())
            {
                stop = true;
            }
            else
            {
                foreach (var (x, y) in toMove)
                {
                    grid[(x + 1) % xlen, y] = grid[x, y];
                    grid[x, y] = 0;
                }
            }
        }

        t.Stop();
        Logger.Log($"<+black>> part1: stops at step <+white>{step}<r>");
    }
}

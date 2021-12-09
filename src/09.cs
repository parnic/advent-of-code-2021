namespace aoc2021
{
    internal class Day09
    {
        internal static void Go()
        {
            Logger.Log("Day 9");
            Logger.Log("-----");
            var lines = File.ReadAllLines("inputs/09.txt");
            byte[,] grid = new byte[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    grid[i, j] = (byte)char.GetNumericValue(lines[i][j]);
                }
            }
            Part1(grid);
            Part2(grid);
            Logger.Log("");
        }

        private static void Part1(byte[,] grid)
        {
            using var t = new Timer();

            var lowPoints = GetLowPoints(grid);
            var totalRisk = lowPoints.Sum(x => grid[x.Item1, x.Item2] + 1);

            Logger.Log($"part1: {totalRisk}");
        }

        private static List<(int, int)> GetLowPoints(byte[,] grid)
        {
            List<(int, int)> lowPoints = new();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    byte val = grid[i, j];
                    if (i > 0 && grid[i - 1, j] <= val)
                    {
                        continue;
                    }
                    if (i < grid.GetLength(0) - 1 && grid[i + 1, j] <= val)
                    {
                        continue;
                    }
                    if (j > 0 && grid[i, j - 1] <= val)
                    {
                        continue;
                    }
                    if (j < grid.GetLength(1) - 1 && grid[i, j + 1] <= val)
                    {
                        continue;
                    }

                    lowPoints.Add((i, j));
                }
            }

            return lowPoints;
        }

        private static void Part2(byte[,] grid)
        {
            using var t = new Timer();

            var lowPoints = GetLowPoints(grid);
            List<int> basins = new();
            foreach (var point in lowPoints)
            {
                var basinPoints = GetBasinSize(grid, point.Item1, point.Item2);
                basins.Add(basinPoints.Distinct().Count() + 1);
            }
            var top3Mult = basins.OrderByDescending(x => x).Take(3).Aggregate(1, (x,y) => x * y);

            Logger.Log($"part2: {top3Mult}");
        }

        private static List<(int, int)> GetBasinSize(byte[,] grid, int i, int j)
        {
            List<(int, int)> basinPoints = new();

            if (i >= grid.GetLength(0) || j >= grid.GetLength(1) || i < 0 || j < 0)
            {
                return new();
            }

            if (!basinPoints.Contains((i - 1, j)) && IsBasinPoint(grid, grid[i, j], i - 1, j))
            {
                basinPoints.Add((i - 1, j));
                basinPoints.AddRange(GetBasinSize(grid, i - 1, j));
            }
            if (!basinPoints.Contains((i + 1, j)) && IsBasinPoint(grid, grid[i, j], i + 1, j))
            {
                basinPoints.Add((i + 1, j));
                basinPoints.AddRange(GetBasinSize(grid, i + 1, j));
            }
            if (!basinPoints.Contains((i, j - 1)) && IsBasinPoint(grid, grid[i, j], i, j - 1))
            {
                basinPoints.Add((i, j - 1));
                basinPoints.AddRange(GetBasinSize(grid, i, j - 1));
            }
            if (!basinPoints.Contains((i, j + 1)) && IsBasinPoint(grid, grid[i, j], i, j + 1))
            {
                basinPoints.Add((i, j + 1));
                basinPoints.AddRange(GetBasinSize(grid, i, j + 1));
            }

            return basinPoints;
        }

        private static bool IsBasinPoint(byte[,] grid, byte val, int i, int j)
        {
            if (i >= grid.GetLength(0) || j >= grid.GetLength(1) || i < 0 || j < 0)
            {
                return false;
            }
            if (grid[i, j] == 9 || val == 9)
            {
                return false;
            }

            return grid[i, j] > val;
        }
    }
}

namespace aoc2021;

internal class Day15 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/15.txt");

        var grid = new byte[lines.ElementAt(0).Length, lines.Count()];
        for (int y = 0; y < lines.Count(); y++)
        {
            for (int x = 0; x < lines.ElementAt(y).Length; x++)
            {
                grid[x, y] = (byte)char.GetNumericValue(lines.ElementAt(y)[x]);
            }
        }

        Part1(grid);
        Part2(grid);
    }

    private static IEnumerable<(int x, int y)> Adjacent(byte[,] grid, int i, int j)
    {
        if (i > 0)
        {
            yield return (i - 1, j);
        }
        if (j > 0)
        {
            yield return (i, j - 1);
        }
        if (i < grid.GetLength(0) - 1)
        {
            yield return (i + 1, j);
        }
        if (j < grid.GetLength(1) - 1)
        {
            yield return (i, j + 1);
        }
    }

    private static int Solve(byte[,] riskMap)
    {
        var start = (x: 0, y: 0);
        var (xDim, yDim) = (riskMap.GetLength(0), riskMap.GetLength(1));
        var end = (x: xDim - 1, y: yDim - 1);

        var q = new PriorityQueue<(int, int), int>();
        q.Enqueue(start, 0);

        var totalRiskMap = new int[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                totalRiskMap[x, y] = int.MaxValue;
            }
        }
        totalRiskMap[start.x, start.y] = 0;

        for (var currPoint = start; currPoint != end; currPoint = q.Dequeue())
        {
            foreach (var adjacent in Adjacent(riskMap, currPoint.x, currPoint.y))
            {
                var totalRiskThroughP = totalRiskMap[currPoint.x, currPoint.y] + riskMap[adjacent.x, adjacent.y];
                if (totalRiskThroughP < totalRiskMap[adjacent.x, adjacent.y])
                {
                    totalRiskMap[adjacent.x, adjacent.y] = totalRiskThroughP;
                    q.Enqueue(adjacent, totalRiskThroughP);
                }
            }
        }

        return totalRiskMap[end.x, end.y];
    }

    private static void Part1(byte[,] grid)
    {
        using var t = new Timer();

        var risk = Solve(grid);

        Logger.Log($"part1: <blue>{risk}<r>");
    }

    private static byte[,] ScaleUp(byte[,] map)
    {
        var (numCols, numRows) = (map.GetLength(0), map.GetLength(1));

        var res = new byte[numCols * 5, numRows * 5];
        for (var x = 0; x < numCols * 5; x++)
        {
            for (var y = 0; y < numRows * 5; y++)
            {
                var baseY = y % numRows;
                var baseX = x % numCols;
                var baseRiskLevel = map[baseX, baseY];

                var tileDistance = (y / numRows) + (x / numCols);

                var riskLevel = ((baseRiskLevel + tileDistance - 1) % 9) + 1;
                res[x, y] = (byte)riskLevel;
            }
        }

        return res;
    }

    private static void Part2(byte[,] grid)
    {
        using var t = new Timer();

        var risk = Solve(ScaleUp(grid));

        Logger.Log($"part2: <blue>{risk}<r>");
    }
}

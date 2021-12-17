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

    private static readonly (int x, int y)[] Neighbors =
    {
        (-1,  0),
        ( 0, -1),
        ( 1,  0),
        ( 0,  1),
    };

    private static bool IsValidPoint(byte[,] grid, int x, int y) => x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);

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
            foreach (var adjacent in Neighbors)
            {
                var (x, y) = (currPoint.x + adjacent.x, currPoint.y + adjacent.y);
                if (!IsValidPoint(riskMap, x, y))
                {
                    continue;
                }
                var totalRiskThroughP = totalRiskMap[currPoint.x, currPoint.y] + riskMap[x, y];
                if (totalRiskThroughP < totalRiskMap[x, y])
                {
                    totalRiskMap[x, y] = totalRiskThroughP;
                    q.Enqueue((x, y), totalRiskThroughP);
                }
            }
        }

        return totalRiskMap[end.x, end.y];
    }

    private static void Part1(byte[,] grid)
    {
        using var t = new Timer();

        var risk = Solve(grid);

        t.Stop();
        Logger.Log($"<+black>> part1: <+white>{risk}<r>");
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

        t.Stop();
        Logger.Log($"<+black>> part2: <+white>{risk}<r>");
    }
}

namespace aoc2021;

internal class Day20 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/20.txt");
        string key = lines.ElementAt(0);
        var grid = new List<List<byte>>(lines.ElementAt(2).Length);
        for (int i = 2; i < lines.Count(); i++)
        {
            var line = lines.ElementAt(i);
            var row = new List<byte>(line.Length);
            grid.Add(row);
            line.ForEach(ch => row.Add(ch switch
            {
                '#' => 1,
                _ => 0,
            }));
        }

        Part1(grid, key);
        Part2(grid, key);
    }

    private static void Part1(List<List<byte>> grid, string key)
    {
        int numLit = grid.Sum(row => row.Count(col => col == 1));

        using var t = new Timer();

        (grid, numLit) = Enhance(grid, key);
        (grid, numLit) = Enhance(grid, key);

        t.Stop();

        Logger.Log($"<+black>> part1: #lit=<+white>{numLit}<r>");
    }

    private static void Part2(List<List<byte>> grid, string key)
    {
        using var t = new Timer();

        int numLit = 0;
        for (int i = 0; i < 50; i++)
        {
            (grid, numLit) = Enhance(grid, key);
        }

        t.Stop();
        Logger.Log($"<+black>> part2: #lit=<+white>{numLit}<r>");
    }

    private static bool IsValidIdx(int len, int idx) => idx >= 0 && idx < len;

    private static (List<List<byte>> grid, int numLit) Enhance(List<List<byte>> grid, string key)
    {
        var newSize = grid.Count + 2;
        var emptyRow = Enumerable.Range(0, newSize).Select(_ => (byte)0);
        var outImg = new List<List<byte>>(newSize);
        for (int rowIdx = 0; rowIdx < newSize; rowIdx++)
        {
            outImg.Add(new List<byte>(emptyRow));
        }

        var def = key[0] == '#' && grid.Count % 4 != 0 ? '1' : '0';
        int numLit = 0;

        for (int row = - 1; row < grid.Count + 1; row++)
        {
            for (int col = - 1; col < grid.Count + 1; col++)
            {
                var num = Enumerable.Range(0, 9).Select(_ => def).ToArray();
                if (IsValidIdx(grid.Count, row - 1))
                {
                    if (IsValidIdx(grid.Count, col - 1))
                        num[0] = (char)(grid[row - 1][col - 1] + '0');
                    if (IsValidIdx(grid.Count, col))
                        num[1] = (char)(grid[row - 1][col] + '0');
                    if (IsValidIdx(grid.Count, col + 1))
                        num[2] = (char)(grid[row - 1][col + 1] + '0');
                }
                if (IsValidIdx(grid.Count, row))
                {
                    if (IsValidIdx(grid.Count, col - 1))
                        num[3] = (char)(grid[row][col - 1] + '0');
                    if (IsValidIdx(grid.Count, col))
                        num[4] = (char)(grid[row][col] + '0');
                    if (IsValidIdx(grid.Count, col + 1))
                        num[5] = (char)(grid[row][col + 1] + '0');
                }
                if (IsValidIdx(grid.Count, row + 1))
                {
                    if (IsValidIdx(grid.Count, col - 1))
                        num[6] = (char)(grid[row + 1][col - 1] + '0');
                    if (IsValidIdx(grid.Count, col))
                        num[7] = (char)(grid[row + 1][col] + '0');
                    if (IsValidIdx(grid.Count, col + 1))
                        num[8] = (char)(grid[row + 1][col + 1] + '0');
                }

                var keyIdx = Convert.ToInt32(new string(num), 2);
                var ch = (byte)(key[keyIdx] == '#' ? 1 : 0);
                outImg[row + 1][col + 1] = ch;
                if (ch == 1)
                {
                    numLit++;
                }
            }
        }

        return (outImg, numLit);
    }
}

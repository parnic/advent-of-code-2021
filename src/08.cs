namespace aoc2021;

internal class Day08 : Day
{
    internal override void Go()
    {
        var lines = File.ReadAllLines("inputs/08.txt");
        List<(List<string>, List<string>)> puzzle = new();
        foreach (var line in lines)
        {
            var portions = line.Split(" | ");
            puzzle.Add((new List<string>(portions[0].Split(' ')), new List<string>(portions[1].Split(' '))));
        }
        Part1(puzzle);
        Part2(puzzle);
    }

    private static void Part1(List<(List<string>, List<string>)> lines)
    {
        using var t = new Timer();

        int count = 0;
        foreach (var line in lines)
        {
            foreach (var combo in line.Item2)
            {
                switch (combo.Length)
                {
                    case 2: // 1
                    case 4: // 4
                    case 3: // 7
                    case 7: // 8
                        count++;
                        break;
                }
            }
        }

        Logger.Log($"part1: <blue>{count}<r>");
    }

    private static void Part2(List<(List<string>, List<string>)> lines)
    {
        using var t = new Timer();

        long sum = 0;
        foreach (var line in lines)
        {
            var segments = new char[7];
            var one = line.Item1.First(x => x.Length == 2);
            var four = line.Item1.First(x => x.Length == 4);
            var seven = line.Item1.First(x => x.Length == 3);
            var eight = line.Item1.First(x => x.Length == 7);

            var len5 = line.Item1.Where(x => x.Length == 5); // 2, 3, 5
            var len6 = line.Item1.Where(x => x.Length == 6); // 0, 6, 9

            // top
            segments[0] = seven.First(x => !one.Contains(x));

            // bottom
            foreach (var pattern in len6)
            {
                var leftover = pattern.Where(x => !seven.Contains(x) && !four.Contains(x));
                if (leftover.Count() == 1)
                {
                    segments[6] = leftover.First();
                }
            }

            var three = len5.First(seg => seg.Contains(segments[0]) && seg.Contains(segments[6]) && seg.Contains(one[0]) && seg.Contains(one[1]));
            // center
            segments[3] = three.First(x => x != segments[0] && x != segments[6] && !one.Contains(x));

            // bottom left
            segments[4] = eight.First(x => x != segments[0] && !four.Contains(x) && x != segments[6]);

            // top left
            segments[1] = eight.First(x => !three.Contains(x) && x != segments[4]);

            var two = len5.First(x => x.Where(y => y != segments[0] && y != segments[6]).Except(four).Count() == 1);
            //var five = len5.First(x => x != two && x != three);
            var nine = len6.First(x => !x.Except(four).Except(new List<char>() { segments[0], segments[6] }).Any());
            var zero = len6.First(x => !x.Contains(segments[3]));
            var six = len6.First(x => x != zero && x != nine);

            // bottom right
            segments[5] = six.Except(two).First(x => x != segments[1]);

            // top right
            segments[2] = one.First(x => x != segments[5]);

            int num = 0;
            for (int i = 0; i < line.Item2.Count; i++)
            {
                var numInt = FindNum(segments, line.Item2[i]);
                num += numInt * (int)Math.Pow(10, (line.Item2.Count - i - 1));
            }

            sum += num;
        }

        Logger.Log($"part2: <blue>{sum}<r>");
    }

    private static int FindNum(char[] segments, string num)
    {
        // i already solved for each number in part2, so it's kind of dumb to rebuild my own set of numbers here,
        // but this is all left over from my various different attempts to solve this different ways.
        // and it works, so whatever.
        var zero = new List<char>() { segments[0], segments[1], segments[2], segments[4], segments[5], segments[6] };
        var six = new List<char>() { segments[0], segments[1], segments[3], segments[4], segments[5], segments[6] };
        var nine = new List<char>() { segments[0], segments[1], segments[2], segments[3], segments[5], segments[6] };
        var two = new List<char>() { segments[0], segments[2], segments[3], segments[4], segments[6] };
        var three = new List<char>() { segments[0], segments[2], segments[3], segments[5], segments[6] };
        var five = new List<char>() { segments[0], segments[1], segments[3], segments[5], segments[6] };

        switch (num.Length)
        {
            case 2:
                return 1;

            case 3:
                return 7;

            case 4:
                return 4;

            case 7:
                return 8;

            case 6:
                if (num.All(x => zero.Contains(x)))
                {
                    return 0;
                }
                else if (num.All(x => six.Contains(x)))
                {
                    return 6;
                }
                else if (num.All(x => nine.Contains(x)))
                {
                    return 9;
                }

                throw new Exception();

            case 5:
                if (num.All(x => two.Contains(x)))
                {
                    return 2;
                }
                else if (num.All(x => three.Contains(x)))
                {
                    return 3;
                }
                else if (num.All(x => five.Contains(x)))
                {
                    return 5;
                }

                throw new Exception();

            default:
                throw new Exception();
        }

        throw new Exception();
    }
}

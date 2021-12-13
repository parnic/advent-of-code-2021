namespace aoc2021;

internal class Day10 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/10.txt");
        Part1(lines);
        Part2(lines);
    }

    private static readonly List<char> Openers = new()
    {
        '(',
        '[',
        '{',
        '<',
    };

    private static readonly List<char> Closers = new()
    {
        ')',
        ']',
        '}',
        '>',
    };

    private static bool IsMatching(char open, char close) => Closers.IndexOf(close) == Openers.IndexOf(open);

    private static void Part1(IEnumerable<string> lines)
    {
        List<int> charVals = new()
        {
            3,
            57,
            1197,
            25137,
        };

        using var t = new Timer();

        long score = 0;
        foreach (var line in lines)
        {
            var (corrupted, ch) = IsCorrupted(line);
            if (corrupted)
            {
                score += charVals[Closers.IndexOf(ch)];
            }
        }

        Logger.Log($"part1: <blue>{score}<r>");
    }

    private static (bool, char) IsCorrupted(string line)
    {
        var s = new Stack<char>();
        foreach (var ch in line)
        {
            if (Openers.Contains(ch))
            {
                s.Push(ch);
            }
            else if (Closers.Contains(ch))
            {
                var popped = s.Pop();
                if (!IsMatching(popped, ch))
                {
                    return (true, ch);
                }
            }
            else
            {
                throw new Exception();
            }
        }

        return (false, '\0');
    }

    private static void Part2(IEnumerable<string> lines)
    {
        List<int> charVals = new()
        {
            1,
            2,
            3,
            4,
        };

        using var t = new Timer();

        List<long> scores = new();
        foreach (var line in lines)
        {
            var (corrupted, _) = IsCorrupted(line);
            if (corrupted)
            {
                continue;
            }

            var s = new Stack<char>();
            foreach (var ch in line)
            {
                if (Openers.Contains(ch))
                {
                    s.Push(ch);
                }
                else if (Closers.Contains(ch))
                {
                    s.Pop();
                }
                else
                {
                    throw new Exception();
                }
            }

            long score = 0;
            while (s.Count > 0)
            {
                var ch = s.Pop();
                score = (score * 5) + charVals[Openers.IndexOf(ch)];
            }

            scores.Add(score);
        }

        if (scores.Count % 2 == 0)
        {
            throw new Exception();
        }

        var final = scores.OrderBy(x => x).Skip(scores.Count / 2).First();

        Logger.Log($"part2: <blue>{final}<r>");
    }
}

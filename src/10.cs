namespace aoc2021
{
    internal class Day10
    {
        internal static void Go()
        {
            Logger.Log("Day 10");
            Logger.Log("-----");
            var lines = File.ReadAllLines("inputs/10.txt");
            Part1(lines);
            Part2(lines);
            Logger.Log("");
        }

        private static void Part1(IEnumerable<string> lines)
        {
            Dictionary<char, int> charVals = new()
            {
                { ')', 3 },
                { ']', 57 },
                { '}', 1197 },
                { '>', 25137 },
            };

            using var t = new Timer();

            long score = 0;
            foreach (var line in lines)
            {
                var (corrupted, ch) = IsCorrupted(line);
                if (corrupted)
                {
                    score += charVals[ch];
                }
            }

            Logger.Log($"part1: {score}");
        }

        private static (bool, char) IsCorrupted(string line)
        {
            var s = new Stack<char>();
            foreach (var ch in line)
            {
                switch (ch)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        s.Push(ch);
                        break;

                    case ')':
                    case ']':
                    case '}':
                    case '>':
                        var popped = s.Pop();
                        if (ch == ')' && popped != '('
                            || ch == ']' && popped != '['
                            || ch == '}' && popped != '{'
                            || ch == '>' && popped != '<')
                        {
                            return (true, ch);
                        }
                        break;

                    default:
                        throw new Exception();
                }
            }

            return (false, '\0');
        }

        private static void Part2(IEnumerable<string> lines)
        {
            Dictionary<char, int> charVals = new()
            {
                { '(', 1 },
                { '[', 2 },
                { '{', 3 },
                { '<', 4 },
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
                    switch (ch)
                    {
                        case '(':
                        case '[':
                        case '{':
                        case '<':
                            s.Push(ch);
                            break;

                        case ')':
                        case ']':
                        case '}':
                        case '>':
                            s.Pop();
                            break;

                        default:
                            throw new Exception();
                    }
                }

                long score = 0;
                while (s.Count > 0)
                {
                    var ch = s.Pop();
                    if (ch == '(')
                    {
                        score *= 5;
                        score += charVals[ch];
                    }
                    else if (ch == '[')
                    {
                        score *= 5;
                        score += charVals[ch];
                    }
                    else if (ch == '{')
                    {
                        score *= 5;
                        score += charVals[ch];
                    }
                    else if (ch == '<')
                    {
                        score *= 5;
                        score += charVals[ch];
                    }
                }

                scores.Add(score);
            }

            if (scores.Count % 2 == 0)
            {
                throw new Exception();
            }

            var final = scores.OrderBy(x => x).Skip((int)Math.Floor(scores.Count / 2.0)).First();

            Logger.Log($"part2: {final}");
        }
    }
}

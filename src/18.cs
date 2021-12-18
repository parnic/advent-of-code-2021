using System.Diagnostics;

namespace aoc2021;

internal class Day18 : Day
{
    class SFNum
    {
        public int? left;
        public int? right;
        public SFNum? leftNum;
        public SFNum? rightNum;
        public SFNum? owner;

        public bool exploding;
        public bool splittingLeft;
        public bool splittingRight;

        public override string ToString() => $"{(exploding ? "<+white>" : "")}" +
            $"[" +
            $"{leftNum?.ToString() ?? ""}{(splittingLeft ? "<+white>" : "")}{left?.ToString() ?? ""}{(splittingLeft ? "<r>" : "")}" +
            $"," +
            $"{rightNum?.ToString() ?? ""}{(splittingRight ? "<+white>" : "")}{right?.ToString() ?? ""}{(splittingRight ? "<r>" : "")}" +
            $"]" +
            $"{(exploding ? "<r>" : "")}";

        public SFNum()
        {

        }

        public SFNum(SFNum other)
        {
            left = other.left;
            right = other.right;
            if (other.leftNum != null)
            {
                leftNum = new SFNum(other.leftNum)
                {
                    owner = this
                };
            }
            if (other.rightNum != null)
            {
                rightNum = new SFNum(other.rightNum)
                {
                    owner = this
                };
            }

            exploding = other.exploding;
            splittingLeft = other.splittingLeft;
            splittingRight = other.splittingRight;
        }

        public SFNum Root
        {
            get
            {
                SFNum curr = this;
                while (curr.owner != null)
                {
                    curr = curr.owner;
                }

                return curr;
            }
        }

        public long Magnitude
        {
            get
            {
                long result = 0;
                if (leftNum != null)
                {
                    result += 3 * leftNum.Magnitude;
                }
                else
                {
                    result += 3 * (int)left!;
                }

                if (rightNum != null)
                {
                    result += 2 * rightNum.Magnitude;
                }
                else
                {
                    result += 2 * (int)right!;
                }

                return result;
            }
        }
    }

    [Conditional("DEBUG")]
    private static void DoTests()
    {
        DoExplodeTests();
        DoSplitTests();
        DoMagnitudeTests();
        DoAddTests();
        DoReduceTests();
        DoPart1Tests();
    }

    private static void DoExplodeTests()
    {
        Logger.Log("<underline>test: explode<r>");
        var tests = new List<string>()
        {
            "[[[[[9,8],1],2],3],4]",
            "[7,[6,[5,[4,[3,2]]]]]",
            "[[6,[5,[4,[3,2]]]],1]",
            "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]",
            "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]",
            "[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]",
            "[[[[0,7],4],[7,[[8,4],9]]],[1,1]]",
            "[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]",
        };
        var exploded = new List<string>()
        {
            "[[[[0,9],2],3],4]",
            "[7,[6,[5,[7,0]]]]",
            "[[6,[5,[7,0]]],3]",
            "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]",
            "[[3,[2,[8,0]]],[9,[5,[7,0]]]]",
            "[[[[0,7],4],[7,[[8,4],9]]],[1,1]]",
            "[[[[0,7],4],[15,[0,13]]],[1,1]]",
            "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]",
        };

        for (int i = 0; i < tests.Count; i++)
        {
            Logger.Log($"<magenta>1.{(i + 1)}<r>");
            SFNum test = ParseSFNum(tests[i]);
            SFNum result = ParseSFNum(exploded[i]);
            var didExplode = CheckExplodes(test);
            if (!didExplode || test.ToString() != result.ToString())
            {
                throw new Exception();
            }
            Logger.Log("<green>✓<r>");
        }

        var twoStepTests = new List<string>()
        {
            "[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]",
        };
        var twoStepExploded = new List<string>()
        {
            "[[[[0,7],4],[15,[0,13]]],[1,1]]",
        };

        for (int i = 0; i < twoStepTests.Count; i++)
        {
            Logger.Log($"<magenta>2.{(i + 1)}<r>");
            SFNum test = ParseSFNum(twoStepTests[i]);
            SFNum result = ParseSFNum(twoStepExploded[i]);
            var didExplode = CheckExplodes(test);
            if (!didExplode)
            {
                throw new Exception();
            }
            didExplode = CheckExplodes(test);
            if (!didExplode || test.ToString() != result.ToString())
            {
                throw new Exception();
            }
            Logger.Log("<green>✓<r>");
        }
    }

    private static void DoSplitTests()
    {
        Logger.Log("<underline>test: split<r>");
        var tests = new List<string>()
        {
            "[[[[0,7],4],[15,[0,13]]],[1,1]]",
            "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]",
            "[[[[7,7],[7,8]],[[9,5],[8,0]]],[[[9,10],20],[8,[9,0]]]]",
        };
        var split = new List<string>()
        {
            "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]",
            "[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]",
            "[[[[7,7],[7,8]],[[9,5],[8,0]]],[[[9,[5,5]],20],[8,[9,0]]]]",
        };

        for (int i = 0; i < tests.Count; i++)
        {
            Logger.Log($"<magenta>{(i + 1)}<r>");
            SFNum test = ParseSFNum(tests[i]);
            SFNum result = ParseSFNum(split[i]);
            var didSplit = CheckSplits(test);
            if (!didSplit || test.ToString() != result.ToString())
            {
                throw new Exception();
            }
            // todo: add tests verifying owners are correct
            Logger.Log("<green>✓<r>");
        }
    }

    private static void DoMagnitudeTests()
    {
        Logger.Log("<underline>test: magnitude<r>");
        var tests = new List<string>()
        {
            "[9,1]",
            "[1,9]",
            "[[9,1],[1,9]]",
            "[[1,2],[[3,4],5]]",
            "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]",
            "[[[[1,1],[2,2]],[3,3]],[4,4]]",
            "[[[[3,0],[5,3]],[4,4]],[5,5]]",
            "[[[[5,0],[7,4]],[5,5]],[6,6]]",
            "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]",
        };
        var magnitudes = new List<long>()
        {
            29,
            21,
            129,
            143,
            1384,
            445,
            791,
            1137,
            3488,
        };

        for (int i = 0; i < tests.Count; i++)
        {
            Logger.Log($"<magenta>{(i + 1)}<r>");
            SFNum test = ParseSFNum(tests[i]);
            var magnitude = test.Magnitude;
            if (magnitude != magnitudes[i])
            {
                throw new Exception();
            }
            Logger.Log("<green>✓<r>");
        }
    }

    private static void DoAddTests()
    {
        Logger.Log("<underline>test: add<r>");
        var tests = new List<List<string>>()
        {
            new()
            {
                "[[[[4,3],4],4],[7,[[8,4],9]]]",
                "[1,1]",
            },
        };
        var results = new List<string>()
        {
            "[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]",
        };

        for (int i = 0; i < tests.Count; i++)
        {
            Logger.Log($"<magenta>{(i + 1)}<r>");
            SFNum test = ParseSFNum(tests[i][0]);
            for (int j = 1; j < tests[i].Count; j++)
            {
                test = Add(test, ParseSFNum(tests[i][j]));
            }
            SFNum result = ParseSFNum(results[i]);
            if (test.ToString() != result.ToString())
            {
                throw new Exception();
            }
            if (result.leftNum!.owner != result || result.rightNum!.owner != result)
            {
                throw new Exception();
            }
            Logger.Log("<green>✓<r>");
        }
    }

    private static void DoReduceTests()
    {
        Logger.Log("<underline>test: reduce<r>");
        var tests = new List<string>()
        {
            "[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]",
        };
        var results = new List<string>()
        {
            "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]",
        };

        for (int i = 0; i < tests.Count; i++)
        {
            Logger.Log($"<magenta>{(i + 1)}<r>");
            SFNum test = ParseSFNum(tests[i]);
            SFNum result = ParseSFNum(results[i]);
            Reduce(test);
            if (test.ToString() != result.ToString())
            {
                throw new Exception();
            }
            Logger.Log("<green>✓<r>");
        }
    }

    private static void DoPart1Tests()
    {
        Logger.Log("<underline>test: add+reduce<r>");
        var vals = new List<List<SFNum>>()
        {
            new()
            {
                ParseSFNum("[1,1]"),
                ParseSFNum("[2,2]"),
                ParseSFNum("[3,3]"),
                ParseSFNum("[4,4]"),
            },
            new()
            {
                ParseSFNum("[1,1]"),
                ParseSFNum("[2,2]"),
                ParseSFNum("[3,3]"),
                ParseSFNum("[4,4]"),
                ParseSFNum("[5,5]"),
            },
            new()
            {
                ParseSFNum("[1,1]"),
                ParseSFNum("[2,2]"),
                ParseSFNum("[3,3]"),
                ParseSFNum("[4,4]"),
                ParseSFNum("[5,5]"),
                ParseSFNum("[6,6]"),
            },
            new()
            {
                ParseSFNum("[[[[4,3],4],4],[7,[[8,4],9]]]"),
                ParseSFNum("[1,1]"),
            },
            new()
            {
                ParseSFNum("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]"),
                ParseSFNum("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]"),
            },
            new()
            {
                ParseSFNum("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]"),
                ParseSFNum("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]"),
            },
            new()
            {
                ParseSFNum("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]"),
                ParseSFNum("[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]"),
            },
            new()
            {
                ParseSFNum("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]"),
                ParseSFNum("[7,[5,[[3,8],[1,4]]]]"),
            },
            new()
            {
                ParseSFNum("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]"),
                ParseSFNum("[[2,[2,2]],[8,[8,1]]]"),
            },
            new()
            {
                ParseSFNum("[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]"),
                ParseSFNum("[2,9]"),
            },
            new()
            {
                ParseSFNum("[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]"),
                ParseSFNum("[1,[[[9,3],9],[[9,0],[0,7]]]]"),
            },
            new()
            {
                ParseSFNum("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]"),
                ParseSFNum("[[[5,[7,4]],7],1]"),
            },
            new()
            {
                ParseSFNum("[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]"),
                ParseSFNum("[[[[4,2],2],6],[8,7]]"),
            },
        };
        var results = new List<SFNum>()
        {
            ParseSFNum("[[[[1,1],[2,2]],[3,3]],[4,4]]"),
            ParseSFNum("[[[[3,0],[5,3]],[4,4]],[5,5]]"),
            ParseSFNum("[[[[5,0],[7,4]],[5,5]],[6,6]]"),
            ParseSFNum("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]"),
            ParseSFNum("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]"),
            ParseSFNum("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]"),
            ParseSFNum("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]"),
            ParseSFNum("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]"),
            ParseSFNum("[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]"),
            ParseSFNum("[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]"),
            ParseSFNum("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]"),
            ParseSFNum("[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]"),
            ParseSFNum("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"),
        };

        for (int i = 0; i < vals.Count; i++)
        {
            Logger.Log($"<magenta>{(i + 1)}<r>");
            SFNum curr = vals[i][0];
            for (int j = 1; j < vals[i].Count; j++)
            {
                curr = Add(curr, vals[i][j]);
                Reduce(curr);
            }

            if (curr.ToString() != results[i].ToString())
            {
                Logger.Log($" expected: {results[i]}");
                throw new Exception();
            }
            Logger.Log("<green>✓<r>");
        }
    }

    internal override void Go()
    {
        DoTests();

        var lines = Util.ReadAllLines("inputs/18.txt");
        List<SFNum> nums = new();
        foreach (var line in lines)
        {
            SFNum parsed = ParseSFNum(line);
            nums.Add(parsed);
        }
        Part1(nums);
        Part2(nums);
    }

    private static SFNum ParseSFNum(string line)
    {
        int phase = 0;
        SFNum root = new();
        SFNum? curr = null;
        string numStr = string.Empty;

        var assignNum = () =>
        {
            if (numStr.Length == 0)
            {
                return;
            }

            var val = Convert.ToInt32(numStr);
            if (phase == 0)
            {
                curr!.left = val;
            }
            else
            {
                curr!.right = val;
            }

            numStr = string.Empty;
        };

        foreach (var ch in line)
        {
            switch (ch)
            {
                case '[':
                    if (curr == null)
                    {
                        curr = root;
                        break;
                    }

                    if (phase == 0)
                    {
                        curr!.leftNum = new()
                        {
                            owner = curr,
                        };
                        curr = curr.leftNum;
                    }
                    else
                    {
                        curr!.rightNum = new()
                        {
                            owner = curr,
                        };
                        curr = curr.rightNum;
                    }
                    phase = 0;
                    break;

                case ',':
                    assignNum();
                    phase = 1;
                    break;

                case ']':
                    assignNum();
                    curr = curr!.owner;
                    break;

                default:
                    numStr += ch;
                    break;
            }
        }

        return root;
    }

    private static void Part1(IEnumerable<SFNum> nums)
    {
        using var t = new Timer();

        SFNum? curr = null;
        foreach (var num in nums)
        {
            if (curr == null)
            {
                curr = num;
                continue;
            }
            curr = Add(curr, num);
            Reduce(curr);
        }

        t.Stop();
        Logger.Log($"<+black>> part1: {curr} -> <+white>{curr!.Magnitude}<r>");
    }

    private static void Part2(IEnumerable<SFNum> nums)
    {
        using var t = new Timer();

        List<(SFNum first, SFNum second, SFNum reduced, long magnitude)> totals = new();
        for (int i = 0; i < nums.Count() - 1; i++)
        {
            for (int j = i + 1; j < nums.Count(); j++)
            {
                var first = nums.ElementAt(i);
                var second = nums.ElementAt(j);
                var curr = Add(first, second);
                Reduce(curr);
                totals.Add((first, second, curr, curr.Magnitude));

                curr = Add(second, first);
                Reduce(curr);
                totals.Add((second, first, curr, curr.Magnitude));
            }
        }

        var highest = totals.MaxBy(x => x.magnitude);

        t.Stop();
        Logger.Log($"<+black>> part2: {highest.first} + {highest.second} = {highest.reduced} -> <+white>{highest.magnitude}<r>");
    }

    private static SFNum Add(SFNum first, SFNum second)
    {
        var result = new SFNum()
        {
            leftNum = new SFNum(first),
            rightNum = new SFNum(second),
        };
        result.leftNum.owner = result;
        result.rightNum.owner = result;

        return result;
    }

    private static void Reduce(SFNum num)
    {
        for (int i = 0; i < 10000; i++)
        {
            if (CheckExplodes(num))
            {
                continue;
            }

            if (CheckSplits(num))
            {
                continue;
            }

            return;
        }

        throw new Exception();
    }

    private static bool CheckExplodes(SFNum num, int depth = 0)
    {
#if DEBUG
        var initial = new SFNum()
        {
            left = num.left,
            right = num.right,
            leftNum = num.leftNum,
            rightNum = num.rightNum,
            owner = num.owner,
        };
#endif
        if (num.leftNum == null && num.rightNum == null && depth >= 4)
        {
            Explode(num);
#if DEBUG
            if (initial.Root.ToString() == num.Root.ToString())
            {
                throw new Exception();
            }
#endif
            return true;
        }

        if (num.leftNum != null)
        {
            if (CheckExplodes(num.leftNum, depth + 1))
            {
                return true;
            }
        }
        if (num.rightNum != null)
        {
            if (CheckExplodes(num.rightNum, depth + 1))
            {
                return true;
            }
        }

        return false;
    }

    private static void Explode(SFNum num)
    {
#if DEBUG
        num.exploding = true;
        Logger.Log($"exploding: {num.Root}");
#endif

        var curr = num;
        var last = num;
        while (true)
        {
            curr = curr.owner;
            if (curr == null)
            {
                break;
            }

            if (last == curr.leftNum)
            {
                last = curr;
                continue;
            }

            if (curr.leftNum != null)
            {
                curr = curr.leftNum;
                while (curr.rightNum != null)
                {
                    curr = curr.rightNum;
                }
                curr.right += num.left;
            }
            else
            {
                curr.left += num.left;
            }
            break;
        }

        curr = num;
        last = num;
        while (true)
        {
            curr = curr.owner;
            if (curr == null)
            {
                break;
            }

            if (last == curr.rightNum)
            {
                last = curr;
                continue;
            }

            if (curr.rightNum != null)
            {
                curr = curr.rightNum;
                while (curr.leftNum != null)
                {
                    curr = curr.leftNum;
                }
                curr.left += num.right;
            }
            else
            {
                curr.right += num.right;
            }
            break;
        }

        if (num == num.owner?.leftNum)
        {
            num.owner.leftNum = null;
            num.owner.left = 0;
        }
        else if (num == num.owner?.rightNum)
        {
            num.owner.rightNum = null;
            num.owner.right = 0;
        }

#if DEBUG
        Logger.Log($"        -> {num.Root}");
#endif

        num.owner = null;
    }

    private static bool CheckSplits(SFNum num)
    {
        if (num.leftNum != null)
        {
            if (CheckSplits(num.leftNum))
            {
                return true;
            }
        }

        if (Split(num))
        {
            return true;
        }

        if (num.rightNum != null)
        {
            if (CheckSplits(num.rightNum))
            {
                return true;
            }
        }

        return false;
    }

    private static bool Split(SFNum num)
    {
        if (num.left != null && num.left >= 10)
        {
            num.splittingLeft = true;
#if DEBUG
            Logger.Log($"splitting: {num.Root}");
#endif
            num.leftNum = new()
            {
                left = (int)Math.Floor((int)num.left / 2.0),
                right = (int)Math.Ceiling((int)num.left / 2.0),
                owner = num,
            };
            num.left = null;
            num.splittingLeft = false;
#if DEBUG
            Logger.Log($"        -> {num.Root}");
#endif
            return true;
        }

        if (num.right != null && num.right >= 10)
        {
            num.splittingRight = true;
#if DEBUG
            Logger.Log($"splitting: {num.Root}");
#endif
            num.rightNum = new()
            {
                left = (int)Math.Floor((int)num.right / 2.0),
                right = (int)Math.Ceiling((int)num.right / 2.0),
                owner = num,
            };
            num.right = null;
            num.splittingRight = false;
#if DEBUG
            Logger.Log($"        -> {num.Root}");
#endif
            return true;
        }

        return false;
    }
}

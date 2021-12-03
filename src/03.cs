namespace aoc2021
{
    internal class Day03
    {
        internal static void Go()
        {
            Logger.Log("Day 3");
            Logger.Log("-----");
            var lines = File.ReadAllLines("inputs/03.txt");
            Part1(lines);
            Part2(lines);
            Logger.Log("");
        }

        private static void Part1(IEnumerable<string> lines)
        {
            using var t = new Timer();
            var len = lines.First().Length;
            int gammaRate = 0;
            int epsilonRate = 0;
            for (int i = 0; i < len; i++)
            {
                var numZero = 0;
                var numOne = 0;
                foreach (var line in lines)
                {
                    if (line[i] == '0')
                    {
                        numZero++;
                    }
                    else if (line[i] == '1')
                    {
                        numOne++;
                    }
                }

                if (numOne > numZero)
                {
                    gammaRate |= (1 << (len - i - 1));
                }
                else if (numZero > numOne)
                {
                    epsilonRate |= (1 << (len - i - 1));
                }
            }

            Logger.Log($"part1: gamma rate: {gammaRate}, epsilon rate: {epsilonRate}, mult: {gammaRate * epsilonRate}");
        }

        private static void Part2(IEnumerable<string> lines)
        {
            using var t = new Timer();
            int o2 = 0;
            int co2 = 0;
            var filtered = lines.ToList();
            for (int i = 0; i < lines.First().Length; i++)
            {
                var numZero = 0;
                var numOne = 0;
                foreach (var line in filtered)
                {
                    if (line[i] == '0')
                    {
                        numZero++;
                    }
                    else if (line[i] == '1')
                    {
                        numOne++;
                    }
                }

                if (numOne > numZero)
                {
                    filtered.RemoveAll(x => x[i] != '1');
                }
                else if (numZero > numOne)
                {
                    filtered.RemoveAll(x => x[i] != '0');
                }
                else
                {
                    filtered.RemoveAll(x => x[i] != '1');
                }

                if (filtered.Count == 1)
                {
                    o2 = Convert.ToInt32(filtered[0], 2);
                    break;
                }
            }

            filtered = lines.ToList();
            for (int i = 0; i < lines.First().Length; i++)
            {
                var numZero = 0;
                var numOne = 0;
                foreach (var line in filtered)
                {
                    if (line[i] == '0')
                    {
                        numZero++;
                    }
                    else if (line[i] == '1')
                    {
                        numOne++;
                    }
                }

                if (numOne < numZero)
                {
                    filtered.RemoveAll(x => x[i] != '1');
                }
                else if (numZero < numOne)
                {
                    filtered.RemoveAll(x => x[i] != '0');
                }
                else
                {
                    filtered.RemoveAll(x => x[i] != '0');
                }

                if (filtered.Count == 1)
                {
                    co2 = Convert.ToInt32(filtered[0], 2);
                    break;
                }
            }

            Logger.Log($"part2: o2*co2 = {o2} * {co2} = {o2 * co2}");
        }
    }
}

using System.Text;

namespace aoc2021;

internal class Day14 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/14.txt");

        string template = string.Empty;
        Dictionary<string, char> rules = new();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(template))
            {
                template = line;
            }
            else if (!string.IsNullOrEmpty(line))
            {
                var pair = line.Split(" -> ");
                rules.Add(pair[0], pair[1][0]);
            }
        }

        Part1(template, rules);
        Part2(template, rules);
    }

    private static void Part1(string template, Dictionary<string, char> rules)
    {
        using var t = new Timer();

        string curr = template;

        for (int i = 0; i < 10; i++)
        {
            StringBuilder sb = new();
            for (int j = 0; j < curr.Length - 1; j++)
            {
                sb.Append(curr[j]);
                sb.Append(rules[curr[j..(j + 2)]]);
            }
            sb.Append(curr[curr.Length - 1]);
            curr = sb.ToString();
        }

        Dictionary<char, int> frequency = new();
        foreach (var c in curr)
        {
            if (!frequency.ContainsKey(c))
            {
                frequency.Add(c, 0);
            }

            frequency[c]++;
        }
        var least = frequency.MinBy(x => x.Value);
        var most = frequency.MaxBy(x => x.Value);

        Logger.Log($"part1: <blue>{most.Value - least.Value}<r>");
    }

    private static void Part2(string template, Dictionary<string, char> rules)
    {
        using var t = new Timer();

        Dictionary<string, long> pairs = new();
        Dictionary<char, long> frequencies = new();
        foreach (var pair in rules)
        {
            pairs.Add(pair.Key, 0);
            if (!frequencies.ContainsKey(pair.Key[0]))
            {
                frequencies.Add(pair.Key[0], 0);
            }
            if (!frequencies.ContainsKey(pair.Key[1]))
            {
                frequencies.Add(pair.Key[1], 0);
            }
        }
        for (int i = 0; i < template.Length - 1; i++)
        {
            var pair = template[i..(i + 2)];
            pairs[pair]++;
            frequencies[template[i]]++;
        }
        frequencies[template[^1]]++;

        for (int round = 0; round < 40; round++)
        {
            foreach (var pair in pairs.Where(x => x.Value > 0).ToList())
            {
                pairs[pair.Key] -= pair.Value;
                frequencies[rules[pair.Key]] += pair.Value;

                var np1 = $"{pair.Key[0]}{rules[pair.Key]}";
                var np2 = $"{rules[pair.Key]}{pair.Key[1]}";
                pairs[np1] += pair.Value;
                pairs[np2] += pair.Value;
            }
        }

        var least = frequencies.MinBy(x => x.Value);
        var most = frequencies.MaxBy(x => x.Value);

        Logger.Log($"part2: <blue>{most.Value - least.Value}<r>");
    }
}

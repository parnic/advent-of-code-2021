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

        Dictionary<char, int> frequencies = new();
        foreach (var pair in rules)
        {
            frequencies[pair.Key[0]] = 0;
            frequencies[pair.Key[1]] = 0;
        }

        for (int i = 0; i < 10; i++)
        {
            StringBuilder sb = new();
            for (int j = 0; j < curr.Length - 1; j++)
            {
                sb.Append(curr[j]);
                sb.Append(rules[curr[j..(j + 2)]]);
            }
            sb.Append(curr[^1]);
            curr = sb.ToString();
        }

        foreach (var c in curr)
        {
            frequencies[c]++;
        }
        var least = frequencies.Min(x => x.Value);
        var most = frequencies.Max(x => x.Value);

        t.Stop();
        Logger.Log($"<+black>> part1: <+white>{most - least}<r>");
    }

    private static void Part2(string template, Dictionary<string, char> rules)
    {
        using var t = new Timer();

        Dictionary<string, long> pairs = new();
        Dictionary<char, long> frequencies = new();
        foreach (var pair in rules)
        {
            pairs[pair.Key] = 0;
            frequencies[pair.Key[0]] = 0;
            frequencies[pair.Key[1]] = 0;
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
            foreach (var pair in pairs.ToList())
            {
                pairs[pair.Key] -= pair.Value;
                frequencies[rules[pair.Key]] += pair.Value;

                var np1 = $"{pair.Key[0]}{rules[pair.Key]}";
                var np2 = $"{rules[pair.Key]}{pair.Key[1]}";
                pairs[np1] += pair.Value;
                pairs[np2] += pair.Value;
            }
        }

        long least = long.MaxValue;
        long most = long.MinValue;
        foreach (var pair in frequencies)
        {
            if (pair.Value < least)
            {
                least = pair.Value;
            }
            if (pair.Value > most)
            {
                most = pair.Value;
            }
        }

        t.Stop();
        Logger.Log($"<+black>> part2: <+white>{most - least}<r>");
    }
}

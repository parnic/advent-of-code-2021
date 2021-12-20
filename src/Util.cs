using System.Diagnostics;
using System.Text;

namespace aoc2021;

internal static class Util
{
    private static readonly char[] StripPreamble = new char[] { (char)8745, (char)9559, (char)9488, };
    internal static IEnumerable<string> ReadAllLines(string filename)
    {
        if (Console.IsInputRedirected)
        {
            string? line;
            List<string> lines = new();
            while ((line = Console.In.ReadLine()) != null)
            {
                if (!lines.Any())
                {
                    var preamble = Encoding.UTF8.GetPreamble();
                    if (Enumerable.SequenceEqual(line[0..preamble.Length], preamble.Select(x => (char)x)))
                    {
                        line = line[preamble.Length..];
                    }
                    else if (Enumerable.SequenceEqual(line[0..StripPreamble.Length].ToCharArray(), StripPreamble))
                    {
                        line = line[StripPreamble.Length..];
                    }
                }
                lines.Add(line);
            }

            if (lines.Any())
            {
                return lines;
            }
        }

        return File.ReadAllLines(filename);
    }

    internal static void StartTestSet(string name)
    {
        Logger.Log($"<underline>test: {name}<r>");
    }

    internal static void StartTest(string label)
    {
        Logger.Log($"<magenta>{label}<r>");
    }

    internal static void TestCondition(Func<bool> a, bool printResult = true)
    {
        if (a?.Invoke() == false)
        {
            Debug.Assert(false);
            if (printResult)
            {
                Logger.Log("<red>x<r>");
            }
        }
        else
        {
            if (printResult)
            {
                Logger.Log("<green>✓<r>");
            }
        }
    }
}

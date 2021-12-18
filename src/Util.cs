using System.Diagnostics;
using System.Text;

namespace aoc2021;

internal static class Util
{
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
                    if (line.StartsWith(Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble())))
                    {
                        line = line[Encoding.UTF8.GetPreamble().Length..];
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

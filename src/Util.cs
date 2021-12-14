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
}

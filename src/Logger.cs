using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc2021
{
    internal class Logger
    {
        public static void Log(string msg)
        {
            Console.WriteLine(InsertColorCodes(msg));
            Debug.WriteLine(StripColorCodes(msg));
        }

        private static string InsertColorCodes(string msg)
        {
            return msg.Replace("<blue>", "\u001b[36;1m")
                .Replace("<r>", "\u001b[0m");
        }

        private static readonly Regex colorCodes = new Regex(@"(\u001b\[(?:\d+;)?(?:\d+;)?\d+m)", RegexOptions.Compiled);
        private static string StripColorCodes(string msg)
        {
            var ret = msg;
            var matches = colorCodes.Matches(msg);
            matches?.ForEach(match => ret = ret.Replace(match.Groups[1].Value, ""));
            return ret;
        }
    }
}

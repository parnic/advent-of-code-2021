using System.Diagnostics;

namespace aoc2021
{
    internal class Logger
    {
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }
    }
}

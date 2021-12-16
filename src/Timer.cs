using System.Diagnostics;

namespace aoc2021
{
    internal class Timer : IDisposable
    {
        private readonly Stopwatch stopwatch = Stopwatch.StartNew();
        private readonly string? name;
        private bool stopped = false;

        public Timer(string? inName = null)
        {
            name = inName;
        }

        public void Stop()
        {
            if (stopped)
            {
                return;
            }

            stopwatch.Stop();
            stopped = true;
        }

        public void Dispose()
        {
            Stop();
            var (elapsed, unit) = ConvertElapsedToHumanReadable();
            var color = "<red>";
            if (unit == "us" || (unit == "ms" && elapsed < 10))
            {
                color = "<green>";
            }
            else if (unit == "ms" && elapsed < 250)
            {
                color = "<yellow>";
            }
            Logger.Log($"<cyan>{name}{(!string.IsNullOrEmpty(name) ? " t" : "T")}ook {color}{elapsed:N1}{unit}<r>");
        }

        public (double elapsed, string unit) ConvertElapsedToHumanReadable()
        {
            return ConvertElapsedToHumanReadable(stopwatch.ElapsedTicks, Stopwatch.Frequency);
        }

        public static (double elapsed, string unit) ConvertElapsedToHumanReadable(double ticks, long frequency)
        {
            var elapsed = 1.0d * ticks / frequency;
            var unit = "s";
            if (elapsed < 0.001)
            {
                elapsed *= 1e+6;
                unit = "us";
            }
            else if (elapsed < 1)
            {
                elapsed *= 1000;
                unit = "ms";
            }
            else if (elapsed < 60)
            {
                unit = "s";
            }
            else if (elapsed < 60 * 60)
            {
                elapsed /= 60;
                unit = "m";
            }

            return (elapsed, unit);
        }
    }
}

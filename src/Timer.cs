using System.Diagnostics;

namespace aoc2021
{
    internal class Timer : IDisposable
    {
        private readonly Stopwatch stopwatch = Stopwatch.StartNew();
        public void Dispose()
        {
            stopwatch.Stop();
            var (elapsed, unit) = ConvertElapsedToHumanReadable();
            Logger.Log($"Took {elapsed:N1}{unit}");
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

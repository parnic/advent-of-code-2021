namespace aoc2021;

internal abstract class Day : IDisposable
{
    public Day()
    {
        Logger.Log($"<reverse>{GetType().Name}<r>");
        //Logger.Log("<reverse>-----<r>");
    }

    public void Dispose()
    {
        Logger.Log("");
    }

    internal abstract void Go();
}

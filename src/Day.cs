namespace aoc2021;

internal abstract class Day : IDisposable
{
    public Day()
    {
        Logger.Log($"\u001b[47m\u001b[30m{GetType().Name}\u001b[0m");
        //Logger.Log("\u001b[47m\u001b[30m-----\u001b[0m");
    }

    public void Dispose()
    {
        Logger.Log("");
    }

    internal abstract void Go();
}

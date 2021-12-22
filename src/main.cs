using aoc2021;

var types = System.Reflection.Assembly
             .GetExecutingAssembly()
             .GetTypes()
             .Where(t => t.IsSubclassOf(typeof(Day)) && !t.IsAbstract && t.Name != "DayTemplate")
             .OrderBy(t => t.Name);

var arg = args.FirstOrDefault();
if (arg == "all")
{
    foreach (var type in types)
    {
        using var day = (Day)Activator.CreateInstance(type)!;
        day.Go();
    }
}
else
{
    Day? day = null;
    if (string.IsNullOrEmpty(arg))
    {
        day = new Day21();
    }
    else
    {
        var type = types.FirstOrDefault(x => x.Name == $"Day{arg?.PadLeft(2, '0')}");
        if (type == null)
        {
            Logger.Log($"Unknown day <cyan>{arg}<r>");
        }
        else
        {
            day = (Day?)Activator.CreateInstance(type);
        }
    }
    day?.Go();
}

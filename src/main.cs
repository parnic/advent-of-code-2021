using aoc2021;

var arg = args.FirstOrDefault();
if (arg == "all")
{
    var types = System.Reflection.Assembly
             .GetExecutingAssembly()
             .GetTypes()
             .Where(t => t.IsSubclassOf(typeof(Day)) && !t.IsAbstract && t.Name != "DayTemplate")
             .OrderBy(t => t.Name);

    foreach (var type in types)
    {
        var day = (Day)Activator.CreateInstance(type)!;
        day.Go();
    }
}
else
{
    Day day = arg switch
    {
        "1" => new Day01(),
        "2" => new Day02(),
        "3" => new Day03(),
        //"4" => new Day04(),
        "5" => new Day05(),
        "6" => new Day06(),
        "7" => new Day07(),
        "8" => new Day08(),
        "9" => new Day09(),
        "10" => new Day10(),
        "11" => new Day11(),
        _ => new Day12(),
    };
    day.Go();
}

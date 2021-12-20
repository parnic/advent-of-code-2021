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
        using var day = (Day)Activator.CreateInstance(type)!;
        day.Go();
    }
}
else
{
    using Day day = arg switch
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
        "12" => new Day12(),
        "13" => new Day13(),
        "14" => new Day14(),
        "15" => new Day15(),
        "16" => new Day16(),
        "17"=> new Day17(),
        "18" => new Day18(),
        _ => new Day20(),
    };
    day.Go();
}

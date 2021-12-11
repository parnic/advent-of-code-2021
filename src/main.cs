var arg = args.FirstOrDefault();
switch (arg)
{
    case "1":
        aoc2021.Day01.Go();
        break;

    case "2":
        aoc2021.Day02.Go();
        break;

    case "3":
        aoc2021.Day03.Go();
        break;

    case "5":
        aoc2021.Day05.Go();
        break;

    case "6":
        aoc2021.Day06.Go();
        break;

    case "7":
        aoc2021.Day07.Go();
        break;

    case "8":
        aoc2021.Day08.Go();
        break;

    case "9":
        aoc2021.Day09.Go();
        break;

    case "10":
        aoc2021.Day10.Go();
        break;

    default:
        aoc2021.Day11.Go();
        break;
}

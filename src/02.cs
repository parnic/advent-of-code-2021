namespace aoc2021
{
    internal class Day02
    {
        internal static void Go()
        {
            Logger.Log("Day 2");
            Logger.Log("-----");
            var lines = File.ReadAllLines("inputs/02.txt");
            var instructions = new List<Instruction>();
            foreach (var instruction in lines)
            {
                var fmt = instruction.Split(' ');
                instructions.Add(new Instruction()
                {
                    Direction = fmt[0],
                    Amount = Convert.ToInt64(fmt[1]),
                });
            }

            Part1(instructions);
            Part2(instructions);
            Logger.Log("");
        }

        struct Instruction
        {
            public string Direction;
            public long Amount;
        }

        struct Position
        {
            public long h;
            public long d;
        }

        private static void Part1(IEnumerable<Instruction> instructions)
        {
            using var t = new Timer();
            Position pos = new();
            foreach (var instruction in instructions)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        pos.h += instruction.Amount;
                        break;

                    case "down":
                        pos.d += instruction.Amount;
                        break;

                    case "up":
                        pos.d -= instruction.Amount;
                        break;
                }
            }

            Logger.Log($"part1: h: {pos.h}, d: {pos.d}, result: {pos.h * pos.d}");
        }

        private static void Part2(IEnumerable<Instruction> instructions)
        {
            using var t = new Timer();
            Position pos = new();
            long aim = 0;
            foreach (var instruction in instructions)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        pos.h += instruction.Amount;
                        pos.d += aim * instruction.Amount;
                        break;

                    case "down":
                        aim += instruction.Amount;
                        break;

                    case "up":
                        aim -= instruction.Amount;
                        break;
                }
            }

            Logger.Log($"part2: h: {pos.h}, d: {pos.d}, result: {pos.h * pos.d}");
        }
    }
}

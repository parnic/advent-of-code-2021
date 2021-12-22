using System.Diagnostics;

namespace aoc2021;

internal class Day22 : Day
{
    [DebuggerDisplay("{(on ? \"On\" : \"Off\")}, {min} -> {max} ({NumInRange(min, max)})")]
    private class Instruction
    {
        public bool On;
        public (int x, int y, int z) Min;
        public (int x, int y, int z) Max;

        public Instruction(bool inOn, (int x, int y, int z) inMin, (int x, int y, int z) inMax)
        {
            On = inOn;
            Min = inMin;
            Max = inMax;
        }

        public Instruction(Instruction other)
            : this(other.On, other.Min, other.Max)
        {
        }
    }

    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/22.txt");
        var instructions = new List<Instruction>();
        foreach (var line in lines)
        {
            var instruction = line.Split(' ');
            var coords = instruction[1].Split(',');
            var min = (x: 0, y: 0, z: 0);
            var max = (x: 0, y: 0, z: 0);
            foreach (var coord in coords)
            {
                var pieces = coord.Split('=');
                var range = pieces[1].Split("..");

                if (pieces[0] == "x")
                {
                    min.x = int.Parse(range[0]);
                    max.x = int.Parse(range[1]);
                }
                else if (pieces[0] == "y")
                {
                    min.y = int.Parse(range[0]);
                    max.y = int.Parse(range[1]);
                }
                else if (pieces[0] == "z")
                {
                    min.z = int.Parse(range[0]);
                    max.z = int.Parse(range[1]);
                }
                else
                {
                    throw new Exception();
                }
            }

            instructions.Add(new Instruction(instruction[0] == "on", min, max));
        }

        Part1(instructions);
        Part2(instructions);
    }

    private static void Part1(IEnumerable<Instruction> instructions)
    {
        using var t = new Timer();

        // keeping my original brute-force approach for posterity even though part 2 implements a much faster way to handle this
        Dictionary<(int x, int y, int z), bool> states = new();
        foreach (var inst in instructions)
        {
            if (inst.Min.x > 50 || inst.Min.y > 50 || inst.Min.z > 50 || inst.Max.x < -50 || inst.Max.y < -50 || inst.Max.z < -50)
            {
                continue;
            }

            for (int x = inst.Min.x; x <= inst.Max.x; x++)
            {
                if (x < -50 || x > 50)
                {
                    continue;
                }

                for (int y = inst.Min.y; y <= inst.Max.y; y++)
                {
                    if (y < -50 || y > 50)
                    {
                        continue;
                    }

                    for (int z = inst.Min.z; z <= inst.Max.z; z++)
                    {
                        if (z < -50 || z > 50)
                        {
                            continue;
                        }

                        states[(x, y, z)] = inst.On;
                    }
                }
            }
        }

        var count = states.LongCount(x => x.Value);

        t.Stop();
        Logger.Log($"<+black>> part1: <+white>{count}<r>");
    }

    private static void Part2(IEnumerable<Instruction> instructions)
    {
        using var t = new Timer();

        List<Instruction> modded = new();
        long lightsOn = 0;
        foreach (var inst in instructions)
        {
            foreach (var mod in modded.ToList())
            {
                var overlap = GetOverlapRange(mod, inst);
                if (overlap != null)
                {
                    modded.Add(new Instruction(!mod.On, overlap.min, overlap.max));
                    var overlapNum = NumInRange(overlap.min, overlap.max);
                    lightsOn += mod.On ? -overlapNum : overlapNum;
                }
            }

            if (inst.On)
            {
                modded.Add(inst);
                lightsOn += NumInRange(inst);
            }
        }

        t.Stop();
        Logger.Log($"<+black>> part2: <+white>{lightsOn}<r>");
    }

    private static long NumInRange((int x, int y, int z) min, (int x, int y, int z) max)
    {
        long numX = max.x - min.x + 1;
        long numY = max.y - min.y + 1;
        long numZ = max.z - min.z + 1;
        return numX * numY * numZ;
    }

    private static long NumInRange(Instruction inst) => NumInRange(inst.Min, inst.Max);

    private static OverlapRange? GetOverlapRange(Instruction inst1, Instruction inst2)
    {
        if (inst1.Min.x > inst2.Max.x || inst1.Min.y > inst2.Max.y || inst1.Min.z > inst2.Max.z)
        {
            return null;
        }
        if (inst2.Min.x > inst1.Max.x || inst2.Min.y > inst1.Max.y || inst2.Min.z > inst1.Max.z)
        {
            return null;
        }

        var overlapMin = (x: Math.Max(inst1.Min.x, inst2.Min.x), y: Math.Max(inst1.Min.y, inst2.Min.y), z: Math.Max(inst1.Min.z, inst2.Min.z));
        var overlapMax = (x: Math.Min(inst1.Max.x, inst2.Max.x), y: Math.Min(inst1.Max.y, inst2.Max.y), z: Math.Min(inst1.Max.z, inst2.Max.z));
        return new OverlapRange() { min = overlapMin, max = overlapMax };
    }

    [DebuggerDisplay("{min} -> {max}")]
    private class OverlapRange
    {
        public (int x, int y, int z) min;
        public (int x, int y, int z) max;
    }
}

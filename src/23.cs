using System.Collections.Concurrent;
using System.Diagnostics;

namespace aoc2021;

internal class Day23 : Day
{
    private static bool IsPart2 = false;

    [DebuggerDisplay("Cost: {Cost}, Location: {Location}, Moves: {Moves} (Spent cost: {SpentCost})")]
    class Amphipod : IEquatable<Amphipod>
    {
        public int Cost;
        public int Location;
        public int Moves = 0;

        public int SpentCost => Cost * Moves;

        public Amphipod()
        {

        }

        public Amphipod(int cost, int location)
        {
            Cost = cost;
            Location = location;
        }

        public Amphipod(Amphipod other)
        {
            Cost = other.Cost;
            Location = other.Location;
            Moves = other.Moves;
        }

        public override int GetHashCode() => HashCode.Combine(Cost, Location, Moves);

        public bool Equals(Amphipod? other) => Cost == other?.Cost && Location == other.Location && Moves == other.Moves;

        public bool IsHome()
        {
            var isInHomeSlot = !IsPart2 ? costToHomeMap[Cost].Contains(Location) : costToHomeMapP2[Cost].Contains(Location);
            return isInHomeSlot && (Moves > 0 || Location > (!IsPart2 ? 4 : 12));
        }

        public char DebugChar() => Cost switch
        {
            1 => 'A',
            10 => 'B',
            100 => 'C',
            _ => 'D',
        };

        public override bool Equals(object? obj) => Equals(obj as Amphipod);
    }

    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/23.txt");
        int numFound = 0;
        int hallLen = 0;
        List<Amphipod> amps = new();
        foreach (var line in lines)
        {
            foreach (var ch in line)
            {
                if (ch >= 'A' && ch <= 'D')
                {
                    amps.Add(new Amphipod
                    {
                        Cost = ch switch
                        {
                            'A' => 1,
                            'B' => 10,
                            'C' => 100,
                            _ => 1000,
                        },
                        Location = numFound + 1,
                    });
                    numFound++;
                }
                else if (ch == '.')
                {
                    hallLen++;
                }
            }
        }
        Part1(CopyAmphipods(amps));

        IsPart2 = true;
        amps.ForEach(x =>
        {
            if (x.Location > 4)
            {
                x.Location += 8;
            }
        });
        amps.Insert(4, new Amphipod(1000, 5));
        amps.Insert(5, new Amphipod(100, 6));
        amps.Insert(6, new Amphipod(10, 7));
        amps.Insert(7, new Amphipod(1, 8));
        amps.Insert(8, new Amphipod(1000, 9));
        amps.Insert(9, new Amphipod(10, 10));
        amps.Insert(10, new Amphipod(1, 11));
        amps.Insert(11, new Amphipod(100, 12));
        Part2(CopyAmphipods(amps));
    }

    private static readonly int[] validHallStops = new int[]
    {
        -1,
        -2,
        -4,
        -6,
        -8,
        -10,
        -11,
    };

    private static readonly Dictionary<int, int> roomExits = new()
    {
        { 1, -3 },
        { 2, -5 },
        { 3, -7 },
        { 4, -9 },
        { 5, -3 },
        { 6, -5 },
        { 7, -7 },
        { 8, -9 },
    };

    private static readonly Dictionary<int, int> roomExitsP2 = new()
    {
        { 1, -3 },
        { 2, -5 },
        { 3, -7 },
        { 4, -9 },
        { 5, -3 },
        { 6, -5 },
        { 7, -7 },
        { 8, -9 },
        { 9, -3 },
        { 10, -5 },
        { 11, -7 },
        { 12, -9 },
        { 13, -3 },
        { 14, -5 },
        { 15, -7 },
        { 16, -9 },
    };

    private static Dictionary<int, int> GetRoomExits(ICollection<Amphipod> amps)
    {
        if (amps.Count == 8)
        {
            return roomExits;
        }

        return roomExitsP2;
    }

    private static readonly Dictionary<int, List<int>> costToHomeMap = new()
    {
        { 1, new List<int>(){ 5, 1 } },
        { 10, new List<int>(){ 6, 2 } },
        { 100, new List<int>(){ 7, 3 } },
        { 1000, new List<int>(){ 8, 4 } },
    };

    private static readonly Dictionary<int, List<int>> costToHomeMapP2 = new()
    {
        { 1, new List<int>(){ 13, 9, 5, 1 } },
        { 10, new List<int>(){ 14, 10, 6, 2 } },
        { 100, new List<int>(){ 15, 11, 7, 3 } },
        { 1000, new List<int>(){ 16, 12, 8, 4 } },
    };

    private static Dictionary<int, List<int>> GetCostToHomeMap(ICollection<Amphipod> amps)
    {
        if (amps.Count == 8)
        {
            return costToHomeMap;
        }

        return costToHomeMapP2;
    }

    private static bool CanMoveTo(ICollection<Amphipod> amps, Amphipod mover, int target)
    {
        // into hall
        if (mover.Moves == 0)
        {
            if (target > 0)
            {
                throw new Exception();
            }

            for (int i = target - 4; i >= 0; i -= 4)
            {
                if (amps.Any(x => x.Location == i))
                {
                    return false;
                }
            }

            var max = Math.Max(GetRoomExits(amps)[mover.Location], target);
            var min = Math.Min(GetRoomExits(amps)[mover.Location], target);
            if (amps.Any(x => x.Location >= min && x.Location <= max))
            {
                return false;
            }

            return true;
        }
        // into home
        else if (mover.Location < 0)
        {
            if (target < 0)
            {
                throw new Exception();
            }

            var hallDest = GetRoomExits(amps)[target];
            var max = Math.Max(hallDest, mover.Location);
            var min = Math.Min(hallDest, mover.Location);
            if (amps.Any(x => x != mover && x.Location >= min && x.Location <= max))
            {
                return false;
            }

            for (int i = target; i > 0; i -= 4)
            {
                if (amps.Any(x => x.Location == i))
                {
                    return false;
                }
            }

            for (int i = target + 4; i <= amps.Count; i += 4)
            {
                if (!amps.Any(x => x.Location == i) || amps.Any(x => x.Cost != mover.Cost && x.Location == i))
                {
                    return false;
                }
            }

            if (!GetCostToHomeMap(amps)[mover.Cost].Contains(target))
            {
                return false;
            }

            return !amps.Any(x => x.Location == target);
        }

        throw new Exception();
    }

    private static int GetCostTo(int loc, int target)
    {
        int cost = 1;
        // hall to room
        if (loc < 0 && target > 0)
        {
            if (target > 4)
            {
                cost++;
            }
            if (target > 8)
            {
                cost++;
            }
            if (target > 12)
            {
                cost++;
            }
            var hallDest = !IsPart2 ? roomExits[target] : roomExitsP2[target];
            cost += Math.Abs(hallDest - loc);
        }
        // room to hall
        else if (loc > 0 && target < 0)
        {
            if (loc > 4)
            {
                cost++;
            }
            if (loc > 8)
            {
                cost++;
            }
            if (loc > 12)
            {
                cost++;
            }

            cost += Math.Abs((!IsPart2 ? roomExits[loc] : roomExitsP2[loc]) - target);
        }

        return cost;
    }

    private static List<Amphipod> CopyAmphipods(ICollection<Amphipod> amps)
    {
        var copied = new List<Amphipod>(amps.Count);
        foreach (var amp in amps)
        {
            copied.Add(new Amphipod(amp));
        }
        return copied;
    }

    private static List<Amphipod> MoveTo(IList<Amphipod> amps, Amphipod mover, int target)
    {
        int currIdx = amps.IndexOf(mover);
        var copied = CopyAmphipods(amps);

        var copiedAmp = copied[currIdx];
        copiedAmp.Moves += GetCostTo(copiedAmp.Location, target);
        copiedAmp.Location = target;

        return copied;
    }

    private static bool IsSolved(IEnumerable<Amphipod> amps) => amps.All(x => x.IsHome());

    private static int TotalCost(IEnumerable<Amphipod> amps) => amps.Sum(x => x.SpentCost);

    private static long NumSolves = 0;
    private static long NumAttempts = 0;
    private static int LowestCost = int.MaxValue;
    private static long Universe = 0;

    private static readonly ConcurrentDictionary<int, bool> cachedCases = new();

    private static bool Solve(IList<Amphipod> amps)
    {
        var ampHash = GetHash(amps);
        if (cachedCases.TryGetValue(ampHash, out bool success))
        {
            return success;
        }

        if (IsSolved(amps))
        {
            NumSolves++;
            var solveCost = TotalCost(amps);
            if (solveCost < LowestCost)
            {
                LowestCost = solveCost;
            }
            cachedCases[ampHash] = true;
            Universe--;
            return true;
        }

        NumAttempts++;
        Universe++;

        List<Task> tasks = new();
        int lastRowStart = amps.Count - 4 + 1;
        var eligibleMovers = amps.Where(x => (x.Location < 0 && x.Moves > 0) || (x.Location > 0 && x.Moves == 0 && (x.Location <= 4 || !amps.Any(y => y.Location == x.Location - 4)) && (x.Location < lastRowStart || !x.IsHome())));
        foreach (var mover in eligibleMovers)
        {
            if (mover.Moves == 0)
            {
                foreach (var option in validHallStops)
                {
                    if (CanMoveTo(amps, mover, option))
                    {
                        var task = () =>
                        {
                            var copied = MoveTo(amps, mover, option);
                            if (Solve(copied))
                            {
                                cachedCases[ampHash] = true;
                            }
                        };
                        if (Universe == 1)
                        {
                            tasks.Add(Task.Run(task));
                        }
                        else
                        {
                            task();
                        }
                    }
                }
            }
            else if (mover.Location < 0)
            {
                foreach (var option in GetCostToHomeMap(amps)[mover.Cost])
                {
                    if (CanMoveTo(amps, mover, option))
                    {
                        var task = () =>
                        {
                            var copied = MoveTo(amps, mover, option);
                            if (Solve(copied))
                            {
                                cachedCases[ampHash] = true;
                            }
                        };
                        if (Universe == 1)
                        {
                            tasks.Add(Task.Run(task));
                        }
                        else
                        {
                            task();
                        }
                    }
                }
            }
        }

        Task.WaitAll(tasks.ToArray());

        cachedCases[ampHash] = false;
        Universe--;
        return false;
    }

    private static int GetHash(IEnumerable<Amphipod> amps)
    {
        int hash = 0;
        foreach (var amp in amps)
        {
            hash = HashCode.Combine(hash, amp);
        }
        return hash;
    }

    // i know this could be much better. i just needed something quick and dirty.
    private static void Draw(IEnumerable<Amphipod> amps)
    {
        var line = string.Empty;
        for (int i = 0; i < 13; i++)
        {
            line += '█';
        }
        Logger.Log(line);

        line = string.Empty;
        for (int i = 0; i < 13; i++)
        {
            if (i == 0 || i == 12)
            {
                line += '█';
            }
            else
            {
                var here = amps.FirstOrDefault(x => x.Location == -i);
                line += here?.DebugChar() ?? '.';
            }
        }
        Logger.Log(line);

        line = string.Empty;
        for (int i = 0; i < 13; i++)
        {
            if (i == 3 || i == 5 || i == 7 || i == 9)
            {
                var here = amps.FirstOrDefault(x => x.Location == (i - 1) / 2);
                line += here?.DebugChar() ?? '.';
            }
            else
            {
                line += '█';
            }
        }
        Logger.Log(line);

        line = string.Empty;
        for (int i = 0; i < 13; i++)
        {
            if (i == 3 || i == 5 || i == 7 || i == 9)
            {
                var here = amps.FirstOrDefault(x => x.Location == 4 + ((i - 1) / 2));
                line += here?.DebugChar() ?? '.';
            }
            else
            {
                line += '█';
            }
        }
        Logger.Log(line);

        if (amps.Count() > 8)
        {
            line = string.Empty;
            for (int i = 0; i < 13; i++)
            {
                if (i == 3 || i == 5 || i == 7 || i == 9)
                {
                    var here = amps.FirstOrDefault(x => x.Location == 8 + ((i - 1) / 2));
                    line += here?.DebugChar() ?? '.';
                }
                else
                {
                    line += '█';
                }
            }
            Logger.Log(line);

            line = string.Empty;
            for (int i = 0; i < 13; i++)
            {
                if (i == 3 || i == 5 || i == 7 || i == 9)
                {
                    var here = amps.FirstOrDefault(x => x.Location == 12 + ((i - 1) / 2));
                    line += here?.DebugChar() ?? '.';
                }
                else
                {
                    line += '█';
                }
            }
            Logger.Log(line);
        }

        line = string.Empty;
        for (int i = 0; i < 13; i++)
        {
            line += '█';
        }
        Logger.Log(line);
    }

    private static void Part1(List<Amphipod> amps)
    {
        Draw(amps);
        using var t = new Timer();

        Solve(amps);

        t.Stop();
        Logger.Log($"<+black>> part1: in {NumAttempts:N0} universes, found {NumSolves:N0} solves, and the lowest cost was <+white>{LowestCost}<r>");
    }

    private static void Part2(List<Amphipod> amps)
    {
        NumSolves = 0;
        NumAttempts = 0;
        LowestCost = int.MaxValue;
        Universe = 0;
        cachedCases.Clear();
        Draw(amps);
        using var t = new Timer();

        Solve(amps);

        t.Stop();
        Logger.Log($"<+black>> part2: in {NumAttempts:N0} universes, found {NumSolves:N0} solves, and the lowest cost was <+white>{LowestCost}<r>");
    }
}

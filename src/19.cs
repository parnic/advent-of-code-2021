namespace aoc2021;

internal class Day19 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/19.txt");
        var scanners = new List<HashSet<Vector3>>();
        foreach (var line in lines)
        {
            if (line.StartsWith("--"))
            {
                scanners.Add(new());
            }
            else if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            else
            {
                var points = line.Split(',');
                scanners[^1].Add((Convert.ToInt32(points[0]), Convert.ToInt32(points[1]), Convert.ToInt32(points[2])));
            }
        }

        var result = Part1(scanners);
        Part2(result);
    }

    record struct Vector3(int X, int Y, int Z)
    {
        public static implicit operator Vector3((int x, int y, int z) value) => new(value.x, value.y, value.z);
        public static Vector3 operator+(Vector3 p, Vector3 v) => (p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        public static Vector3 operator-(Vector3 p1, Vector3 p2) => (p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

        public int DistanceTo(Vector3 other) => (int)(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2) + Math.Pow(other.Z - Z, 2));

        public int ManhattanDistanceTo(Vector3 other)
        {
            var (dx, dy, dz) = this - other;
            return Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz);
        }

        public Vector3 Transform(Vector3 up, int rotation)
        {
            Vector3 reoriented = up switch
            {
                (0, +1, 0) => (X, Y, Z),
                (0, -1, 0) => (X, -Y, -Z),
                (+1, 0, 0) => (Y, X, -Z),
                (-1, 0, 0) => (Y, -X, Z),
                (0, 0, +1) => (Y, Z, X),
                (0, 0, -1) => (Y, -Z, -X),
                _ => throw new Exception("Invalid up vector")
            };

            return rotation switch
            {
                0 => reoriented,
                1 => (reoriented.Z, reoriented.Y, -reoriented.X),
                2 => (-reoriented.X, reoriented.Y, -reoriented.Z),
                3 => (-reoriented.Z, reoriented.Y, reoriented.X),
                _ => throw new Exception("Invalid rotation")
            };
        }
    }

    private static readonly (int, int, int)[] Axes = new[]
    {
        ( 0, +1,  0),
        ( 0, -1,  0),
        (+1,  0,  0),
        (-1,  0,  0),
        ( 0,  0, +1),
        ( 0,  0, -1)
    };

    private static (IEnumerable<Vector3> alignedBeacons, Vector3 translation, Vector3 up, int rotation)? Align(ICollection<Vector3> beacons1, ICollection<Vector3> beacons2)
    {
        // Fix beacons1, rotate beacons2
        for (int axis = 0; axis < Axes.Length; axis++)
        {
            for (int rotation = 0; rotation < 4; rotation++)
            {
                var rotatedBeacons2 = new HashSet<Vector3>(beacons2.Select(b => b.Transform(Axes[axis], rotation)));

                foreach (var b1 in beacons1)
                {
                    // Assume b1 matches some b2
                    foreach (var matchingB1InB2 in rotatedBeacons2)
                    {
                        // Move all b2 so matchingB1InB2 matches b1, in scanner 1 coordinates
                        var delta = b1 - matchingB1InB2;
                        var transformedBeacons2 = rotatedBeacons2.Select(b => b + delta);

                        // How many overlaps did we get?
                        var intersection = new HashSet<Vector3>(transformedBeacons2);
                        intersection.IntersectWith(beacons1);
                        if (intersection.Count >= 12)
                        {
                            // Found the right orientation
                            return (transformedBeacons2, delta, Axes[axis], rotation);
                        }
                    }
                }
            }
        }
        return null;
    }

    private static (IEnumerable<HashSet<Vector3>> scans, IEnumerable<HashSet<Vector3>> scanners) Reduce(IEnumerable<HashSet<Vector3>> scans, IEnumerable<HashSet<Vector3>> scanners)
    {
        var toRemove = new HashSet<int>();
        for (int i = 0; i < scans.Count() - 1; i++)
        {
            for (int j = i + 1; j < scans.Count(); j++)
            {
                if (toRemove.Contains(j))
                {
                    // Already merged
                    continue;
                }

                var alignment = Align(scans.ElementAt(i), scans.ElementAt(j));
                if (alignment != null)
                {
                    // Convert all scanners from j coordinates to i coordinates
                    foreach (var s in scanners.ElementAt(j))
                    {
                        var scanner = alignment.Value.translation + s.Transform(alignment.Value.up, alignment.Value.rotation);
                        scanners.ElementAt(i).Add(scanner);
                    }
                    // Merge the scan sets
                    scans.ElementAt(i).UnionWith(alignment.Value.alignedBeacons);
                    toRemove.Add(j);
                }
            }
        }
        // Skip all scans and scanners that were merged
        return (scans.Where((_, i) => !toRemove.Contains(i)), scanners.Where((_, i) => !toRemove.Contains(i)));
    }

    private static ICollection<Vector3> Part1(List<HashSet<Vector3>> input)
    {
        using var t = new Timer();

        IEnumerable<HashSet<Vector3>> scans = input;
        IEnumerable<HashSet<Vector3>> scanners = input.Select((_) => new HashSet<Vector3> { (0, 0, 0) }).ToList();
        while (scans.Count() > 1)
        {
            // Note that this will loop forever if there is no alignment
            (scans, scanners) = Reduce(scans, scanners);
        }

        var allBeacons = scans.ElementAt(0);

        t.Stop();
        Logger.Log($"<+black>> part1: <+white>{allBeacons.Count}<r>");

        return scanners.ElementAt(0);
    }

    private static void Part2(ICollection<Vector3> scanners)
    {
        using var t = new Timer();

        var scannerList = scanners.ToList();
        var farthest =
            Enumerable.Range(0, scannerList.Count - 1)
                .SelectMany(i => Enumerable.Range(i + 1, scannerList.Count - i - 1).Select(j => (i, j)))
                .Max(pair => scannerList[pair.i].ManhattanDistanceTo(scannerList[pair.j]));

        t.Stop();
        Logger.Log($"<+black>> part2: <+white>{farthest}<r>");
    }
}

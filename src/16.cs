using System.Text;

namespace aoc2021;

internal class Day16 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/16.txt");
        var input = lines.ElementAt(0);
        var binStr = string.Empty;
        foreach (var ch in input)
        {
            binStr += Convert.ToString(Convert.ToInt64(ch.ToString(), 16), 2).PadLeft(4, '0');
        }
        int idx = 0;
        using var t = new Timer("Decoding packet");
        (var versionTotal, var result) = DecodePacket(binStr, ref idx);
        t.Stop();
        Part1(versionTotal);
        Part2(result);
    }

    private static void Part1(long versionTotal)
    {
        Logger.Log($"<+black>> part1: version total: <+white>{versionTotal}<r>");
    }

    private static void Part2(long result)
    {
        Logger.Log($"<+black>> part2: operator result: <+white>{result}<r>");
    }

    private static (long versionTotal, long result) DecodePacket(string binary, ref int idx)
    {
        long versionTotal = 0;

        (var version, var typeID) = ParsePacketHeader(binary, ref idx);
        versionTotal += version;
        long result;
        switch (typeID)
        {
            case 4:
                result = ParseLiteralPacket(binary, ref idx);
                break;

            default:
                (version, result) = ParseOperatorPacket(binary, typeID, ref idx);
                versionTotal += version;
                break;
        }

        return (versionTotal, result);
    }

    private static (long, long) ParsePacketHeader(string binary, ref int idx)
    {
        var version = Convert.ToInt64(binary[idx..(idx + 3)], 2);
        idx += 3;
        var typeID = Convert.ToInt64(binary[idx..(idx + 3)], 2);
        idx += 3;
        return (version, typeID);
    }

    private static long ParseLiteralPacket(string binary, ref int idx)
    {
        StringBuilder numStr = new();
        bool done = false;
        while (!done)
        {
            if (binary[idx] == '0')
            {
                done = true;
            }

            numStr.Append(binary[(idx + 1)..(idx + 5)]);
            idx += 5;
        }

        return Convert.ToInt64(numStr.ToString(), 2);
    }

    private static (long versionTotal, long result) ParseOperatorPacket(string binary, long inType, ref int idx)
    {
        var lengthType = Convert.ToInt64(binary[idx..(idx + 1)], 2);
        idx++;

        long totalLength = 0;
        long numSubPackets = 0;
        switch (lengthType)
        {
            case 0:
                totalLength = Convert.ToInt64(binary[idx..(idx + 15)], 2);
                idx += 15;
                break;

            case 1:
                numSubPackets = Convert.ToInt64(binary[idx..(idx + 11)], 2);
                idx += 11;
                break;
        }

        long versionTotal = 0;

        bool done = false;
        int startIdx = idx;
        long lengthProcessed = 0;
        long subPacketsProcessed = 0;
        List<long> operands = new();
        while (!done)
        {
            (var version, var operand) = DecodePacket(binary, ref idx);
            operands.Add(operand);
            subPacketsProcessed++;
            lengthProcessed = idx - startIdx;
            versionTotal += version;

            done = done || (numSubPackets != 0 && subPacketsProcessed == numSubPackets);
            done = done || (totalLength != 0 && lengthProcessed == totalLength);
        }

        var result = inType switch
        {
            0 => operands.Sum(),
            1 => operands.Aggregate(1L, (agg, x) => x * agg),
            2 => operands.Min(),
            3 => operands.Max(),
            4 => throw new Exception(),
            5 => operands[0] > operands[1] ? 1 : 0,
            6 => operands[0] < operands[1] ? 1 : 0,
            7 => operands[0] == operands[1] ? 1 : 0,
            _ => throw new Exception(),
        };

        return (versionTotal, result);
    }
}

using System.Diagnostics;

namespace aoc2021;

internal class Day24 : Day
{
    public record struct Instruction(string Opcode, char Op1, string? Op2);

    [DebuggerDisplay("Input: {InputStr}, w: {w}, x: {x}, y: {y}, z: {z}")]
    public class Machine
    {
        public string InputStr;
        private int InputStrIdx = 0;
        public long w = 0;
        public long x = 0;
        public long y = 0;
        public long z = 0;

        public Machine(string input)
        {
            InputStr = input;
        }

        public void ProcessInstruction(Instruction inst)
        {
            switch (inst.Opcode)
            {
                case "inp":
                    StoreValue(inst.Op1, (long)char.GetNumericValue(InputStr[InputStrIdx]));
                    InputStrIdx++;
                    break;

                default:
                    var val = RetrieveValue(inst.Op1);
                    switch (inst.Opcode)
                    {
                        case "add":
                            val += GetValue(inst.Op2!);
                            break;

                        case "mul":
                            val *= GetValue(inst.Op2!);
                            break;

                        case "div":
                            val /= GetValue(inst.Op2!);
                            break;

                        case "mod":
                            val %= GetValue(inst.Op2!);
                            break;

                        case "eql":
                            var b = GetValue(inst.Op2!);
                            val = val == b ? 1 : 0;
                            break;

                        default:
                            throw new Exception();
                    }
                    StoreValue(inst.Op1, val);
                    break;
            }
        }

        public long GetValue(string op2)
        {
            if (long.TryParse(op2, out long iop2))
            {
                return iop2;
            }

            return RetrieveValue(op2[0]);
        }

        public long RetrieveValue(char var) => var switch
        {
            'w' => w,
            'x' => x,
            'y' => y,
            'z' => z,
            _ => throw new Exception(),
        };

        public void StoreValue(char var, long value)
        {
            switch (var)
            {
                case 'w':
                    w = value;
                    break;

                case 'x':
                    x = value;
                    break;

                case 'y':
                    y = value;
                    break;

                case 'z':
                    z = value;
                    break;

                default:
                    throw new Exception();
            }
        }
    }

    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/24.txt");
        List<Instruction> instructions = new();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            var inst = line.Split(' ', 2);
            switch (inst[0])
            {
                case "inp":
                    instructions.Add(new Instruction(inst[0], inst[1][0], null));
                    break;

                default:
                    {
                        var operands = inst[1].Split(' ');
                        instructions.Add(new Instruction(inst[0], operands[0][0], operands[1]));
                    }
                    break;
            }
        }
        Part1(instructions);
        Part2(instructions);
    }

    private static void Part1(IList<Instruction> instructions)
    {
        using var t = new Timer();

        Machine m = new("99394899891971");
        foreach (var inst in instructions)
        {
            m.ProcessInstruction(inst);
        }
        if (m.z != 0)
        {
            throw new Exception();
        }

        t.Stop();
        Logger.Log($"<+black>> part1: <+white>99394899891971<r>");
    }

    private static void Part2(IList<Instruction> instructions)
    {
        using var t = new Timer();

        Machine m = new("92171126131911");
        foreach (var inst in instructions)
        {
            m.ProcessInstruction(inst);
        }
        if (m.z != 0)
        {
            throw new Exception();
        }

        t.Stop();
        Logger.Log($"<+black>> part2: <+white>92171126131911<r>");
    }
}

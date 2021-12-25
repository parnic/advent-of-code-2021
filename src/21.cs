namespace aoc2021;

internal class Day21 : Day
{
    internal override void Go()
    {
        var lines = Util.ReadAllLines("inputs/21.txt");
        var player1Pos = int.Parse(lines.ElementAt(0).Split(": ")[1]);
        var player2Pos = int.Parse(lines.ElementAt(1).Split(": ")[1]);
        Part1(player1Pos, player2Pos);
        Part2(player1Pos, player2Pos);
    }

    private static void Part1(int player1Pos, int player2Pos)
    {
        using var t = new Timer();

        var playerPos = new int[2]
        {
            player1Pos,
            player2Pos,
        };
        var (playerScore, numRolls) = PlayGame(playerPos, 1000, 10);

        t.Stop();
        Logger.Log($"<+black>> part1: <+white>{numRolls * playerScore.Min()}<r>");
    }

    private static void Part2(int player1Pos, int player2Pos)
    {
        using var t = new Timer();

        var (p1wins, p2wins) = PlayQuantumGame(player1Pos - 1, player2Pos - 1, 0, 0, 21);

        t.Stop();
        Logger.Log($"<+black>> part2: p1: {p1wins:N0}, p2: {p2wins:N0} -> <+white>{Math.Max(p1wins, p2wins)}<r>");
    }

    private static (long[] scores, long numRolls) PlayGame(int[] playerPos, int maxScore, int dieSides)
    {
        var playerScore = new long[2]
        {
            0,
            0,
        };

        int dieVal = 1;
        int turn = 0;
        long numRolls = 0;
        while (!playerScore.Any(x => x >= maxScore))
        {
            for (int i = 0; i < 3; i++)
            {
                playerPos[turn] = PlayOneRoll(playerPos[turn], dieVal);
                dieVal = (dieVal + 1) % dieSides;
                numRolls++;
            }

            playerScore[turn] += playerPos[turn];
            turn = 1 - turn;
        }

        return (playerScore, numRolls);
    }

    private static int PlayOneRoll(int playerPos, int dieVal)
    {
        return ((playerPos + dieVal - 1) % 10) + 1;
    }

    private static readonly Dictionary<(int, int, int, int), (long, long)> cachedWinCases = new();

    private static (long, long) PlayQuantumGame(int p1Pos, int p2Pos, int p1Score, int p2Score, int maxScore)
    {
        if (p1Score >= maxScore)
        {
            return (1, 0);
        }
        if (p2Score >= maxScore)
        {
            return (0, 1);
        }
        if (cachedWinCases.TryGetValue((p1Pos, p2Pos, p1Score, p2Score), out (long, long) numWins))
        {
            return numWins;
        }

        (long p1, long p2) wins = (0, 0);
        for (int d1 = 1; d1 <= 3; d1++)
        {
            for (int d2 = 1; d2 <= 3; d2++)
            {
                for (int d3 = 1; d3 <= 3; d3++)
                {
                    var newP1Pos = (p1Pos + d1 + d2 + d3) % 10;
                    var newP1Score = p1Score + newP1Pos + 1;
                    var (p2wins, p1wins) = PlayQuantumGame(p2Pos, newP1Pos, p2Score, newP1Score, maxScore);
                    wins.p1 += p1wins;
                    wins.p2 += p2wins;
                }
            }
        }

        cachedWinCases[(p1Pos, p2Pos, p1Score, p2Score)] = wins;
        return wins;
    }
}

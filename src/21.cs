namespace aoc2021;

internal class Day21 : Day
{
    private record struct GameState((int p1, int p2) Positions, int Turn, (int p1, int p2) Score, (long p1, long p2) Wins, int RollVal, int TotalRolls, int TotalRounds)
    {
        public override int GetHashCode() => HashCode.Combine(Positions.p1, Positions.p2, Turn, RollVal, TotalRounds);
    }

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

        var playerPos = new int[2]
        {
            player1Pos,
            player2Pos,
        };
        var (p1wins, p2wins) = PlayQuantumGame(new List<int>(playerPos), 21);

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

    private static readonly Dictionary<int, GameState> cachedWinCases = new();

    private static (long, long) PlayQuantumGame(List<int> playerPos, int maxScore, List<int>? playerScores = null, int turn = 0, int rollNum = 0, int rollVal = 0, int totalRounds = 0, int totalRolls = 0)
    {
        playerScores ??= new List<int> { 0, 0 };
        if (cachedWinCases.TryGetValue(HashCode.Combine(playerPos[0], playerPos[1], turn, rollVal, totalRounds), out GameState winState))
        {
            return winState.Wins;
        }

        var wins = (0L, 0L);
        while (true)
        {
            totalRounds++;
            for (int i = rollNum; i < 3; i++)
            {
                totalRolls++;
                var twoWins = PlayQuantumGame(new List<int>(playerPos), maxScore, new List<int>(playerScores), turn, i + 1, 2, totalRounds, totalRolls);
                var threeWins = PlayQuantumGame(new List<int>(playerPos), maxScore, new List<int>(playerScores), turn, i + 1, 3, totalRounds, totalRolls);
                wins = (wins.Item1 + twoWins.Item1 + threeWins.Item1, wins.Item2 + twoWins.Item2 + threeWins.Item2);
                playerPos[turn] = PlayOneRoll(playerPos[turn], 1);
                rollVal = 1;
            }
            if (rollNum == 3)
            {
                playerPos[turn] = PlayOneRoll(playerPos[turn], rollVal);
            }

            playerScores[turn] += playerPos[turn];
            if (playerScores[turn] >= maxScore)
            {
                if (turn == 0)
                {
                    wins.Item1++;
                }
                else
                {
                    wins.Item2++;
                }

                GameState state = new((playerPos[0], playerPos[1]), turn, (playerScores[0], playerScores[1]), wins, rollVal, totalRolls, totalRounds);
                cachedWinCases[state.GetHashCode()] = state;

                break;
            }

            turn = 1 - turn;
            rollNum = 0;
            rollVal = 0;
        }

        return wins;
    }
}

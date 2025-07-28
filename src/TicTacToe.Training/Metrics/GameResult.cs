using TicTacToe.Engine;

namespace TicTacToe.Training.Metrics;

public record GameResult(
    string GameId,
    Player? Winner,
    int MoveCount,
    TimeSpan Duration,
    Player StartingPlayer,
    IReadOnlyList<Move> MoveHistory)
{
    public bool IsWin(Player player) => Winner == player;
    public bool IsDraw => Winner == null;
    public bool IsLoss(Player player) => Winner != null && Winner != player;
}

public record TrainingMetrics(
    int TotalGames,
    int XWins,
    int OWins,
    int Draws,
    double AverageGameLength,
    TimeSpan TotalDuration)
{
    public double XWinRate => TotalGames > 0 ? (double)XWins / TotalGames : 0.0;
    public double OWinRate => TotalGames > 0 ? (double)OWins / TotalGames : 0.0;
    public double DrawRate => TotalGames > 0 ? (double)Draws / TotalGames : 0.0;
    
    public double WinRate(Player player) => player == Player.X ? XWinRate : OWinRate;
}

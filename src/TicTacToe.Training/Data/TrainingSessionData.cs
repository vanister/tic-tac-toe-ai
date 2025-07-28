using TicTacToe.Engine.States;

namespace TicTacToe.Training.Data;

public record TrainingSessionData(
    string SessionId,
    DateTime Timestamp,
    string PlayerXName,
    string PlayerOName,
    int TotalGames,
    int XWins,
    int OWins,
    int Draws,
    double AverageGameLength,
    TimeSpan TotalDuration,
    List<GameResultData> Games);

public record GameResultData(
    string GameId,
    DateTime Timestamp,
    string? Winner,
    int MoveCount,
    TimeSpan Duration,
    string StartingPlayer,
    List<MoveData> Moves);

public record MoveData(
    string Player,
    int Position,
    DateTime Timestamp);

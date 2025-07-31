using System.Text.Json;
using TicTacToe.Training.Data;
using TicTacToe.Training.Metrics;
using TicTacToe.Engine.States;

namespace TicTacToe.Training.Services;

public class TrainingDataExporter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task SaveToJsonLinesAsync(
        string filePath,
        TrainingMetrics metrics,
        IReadOnlyList<GameResult> gameResults,
        string playerXName,
        string playerOName)
    {
        var sessionId = Guid.NewGuid().ToString("N")[..8];
        var timestamp = DateTime.UtcNow;

        // Ensure directory exists
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var writer = new StreamWriter(filePath);

        // Write session metadata first
        var sessionData = new TrainingSessionData(
            SessionId: sessionId,
            Timestamp: timestamp,
            PlayerXName: playerXName,
            PlayerOName: playerOName,
            TotalGames: metrics.TotalGames,
            XWins: metrics.XWins,
            OWins: metrics.OWins,
            Draws: metrics.Draws,
            AverageGameLength: metrics.AverageGameLength,
            TotalDuration: metrics.TotalDuration,
            Games: ConvertGameResults(gameResults));

        var sessionJson = JsonSerializer.Serialize(new { type = "session", data = sessionData }, JsonOptions);
        await writer.WriteLineAsync(sessionJson);

        // Write individual game results
        foreach (var game in gameResults)
        {
            var gameData = ConvertGameResult(game);
            var gameJson = JsonSerializer.Serialize(new { type = "game", data = gameData }, JsonOptions);
            await writer.WriteLineAsync(gameJson);
        }
    }

    private static List<GameResultData> ConvertGameResults(IReadOnlyList<GameResult> gameResults)
    {
        return gameResults.Select(ConvertGameResult).ToList();
    }

    private static GameResultData ConvertGameResult(GameResult game)
    {
        var moves = game.MoveHistory.Select(m => new MoveData(
            Player: m.Player.ToString(),
            Position: m.Position,
            Timestamp: m.Timestamp)).ToList();

        var gameResultData = new GameResultData(
            GameId: game.GameId,
            Timestamp: game.MoveHistory.FirstOrDefault()?.Timestamp ?? DateTime.UtcNow,
            Winner: game.Winner?.ToString(),
            MoveCount: game.MoveCount,
            Duration: game.Duration,
            StartingPlayer: game.StartingPlayer.ToString(),
            Moves: moves);

        return gameResultData;
    }

    public static string GenerateDefaultFilePath()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        return Path.Combine("data", "training", $"training-{timestamp}.jsonl");
    }
}

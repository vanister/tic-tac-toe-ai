using TicTacToe.Engine;
using TicTacToe.Training.Metrics;

namespace TicTacToe.Training;

public class TrainingLoop
{
    private readonly GameLoop _gameLoop;
    private readonly List<GameResult> _gameResults;

    public TrainingLoop()
    {
        _gameLoop = new GameLoop();
        _gameResults = new List<GameResult>();
    }

    public TrainingMetrics RunTraining(
        IPlayer playerX, 
        IPlayer playerO, 
        int numberOfGames,
        IProgress<int>? progress = null)
    {
        _gameResults.Clear();
        var startTime = DateTime.UtcNow;

        for (int gameIndex = 0; gameIndex < numberOfGames; gameIndex++)
        {
            var startingPlayer = DetermineStartingPlayer(gameIndex);
            var result = _gameLoop.PlayGame(playerX, playerO, startingPlayer);
            _gameResults.Add(result);

            progress?.Report(gameIndex + 1);
        }

        var totalDuration = DateTime.UtcNow - startTime;
        return CalculateMetrics(totalDuration);
    }

    private static Player DetermineStartingPlayer(int gameIndex)
    {
        return gameIndex % 2 == 0 ? Player.X : Player.O;
    }

    private TrainingMetrics CalculateMetrics(TimeSpan totalDuration)
    {
        var xWins = _gameResults.Count(r => r.Winner == Player.X);
        var oWins = _gameResults.Count(r => r.Winner == Player.O);
        var draws = _gameResults.Count(r => r.Winner == null);
        var averageGameLength = _gameResults.Average(r => r.MoveCount);

        return new TrainingMetrics(
            TotalGames: _gameResults.Count,
            XWins: xWins,
            OWins: oWins,
            Draws: draws,
            AverageGameLength: averageGameLength,
            TotalDuration: totalDuration);
    }

    public IReadOnlyList<GameResult> GetResults() => _gameResults.AsReadOnly();
}

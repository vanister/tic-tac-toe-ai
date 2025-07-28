using TicTacToe.Engine;
using TicTacToe.Training.Metrics;

namespace TicTacToe.Training;

public class TrainingLoop
{
    private readonly GameEngine _gameEngine;
    private readonly List<GameResult> _gameResults;

    public TrainingLoop()
    {
        _gameEngine = new GameEngine();
        _gameResults = new List<GameResult>();
    }

    public TrainingMetrics RunTraining(
        IAiPlayer playerX, 
        IAiPlayer playerO, 
        int numberOfGames,
        IProgress<int>? progress = null)
    {
        _gameResults.Clear();
        var startTime = DateTime.UtcNow;

        for (int gameIndex = 0; gameIndex < numberOfGames; gameIndex++)
        {
            var startingPlayer = DetermineStartingPlayer(gameIndex);
            var result = PlaySingleGame(playerX, playerO, startingPlayer);
            _gameResults.Add(result);

            progress?.Report(gameIndex + 1);
        }

        var totalDuration = DateTime.UtcNow - startTime;
        return CalculateMetrics(totalDuration);
    }

    private GameResult PlaySingleGame(IAiPlayer playerX, IAiPlayer playerO, Player startingPlayer)
    {
        _gameEngine.ResetGame();
        _gameEngine.StartGame(startingPlayer);

        while (!_gameEngine.IsGameFinished())
        {
            var currentPlayer = _gameEngine.GetCurrentPlayer();
            var currentAi = currentPlayer == Player.X ? playerX : playerO;
            
            var move = currentAi.SelectMove(_gameEngine.GetState(), currentPlayer);
            
            if (!_gameEngine.TryMakeMove(currentPlayer, move, out var error))
            {
                throw new InvalidOperationException(
                    $"AI player {currentAi.Name} made invalid move: {error}");
            }
        }

        var finalState = _gameEngine.GetState();
        return new GameResult(
            GameId: finalState.GameId,
            Winner: _gameEngine.GetWinner(),
            MoveCount: finalState.MoveHistory.Count,
            Duration: finalState.MoveHistory.Count > 0 ? 
                finalState.MoveHistory.Last().Timestamp - finalState.StartTime : 
                TimeSpan.Zero,
            StartingPlayer: startingPlayer,
            MoveHistory: finalState.MoveHistory);
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

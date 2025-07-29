using TicTacToe.Engine;
using TicTacToe.Training.Metrics;

namespace TicTacToe.Training;

/// <summary>
/// A unified game loop that can handle any combination of player types (AI, human, etc.)
/// </summary>
public class GameLoop
{
    private readonly GameEngine _gameEngine;

    public GameLoop()
    {
        _gameEngine = new GameEngine();
    }

    /// <summary>
    /// Play a single game between two players
    /// </summary>
    public GameResult PlayGame(IPlayer playerX, IPlayer playerO, Player startingPlayer)
    {
        _gameEngine.ResetGame();
        _gameEngine.StartGame(startingPlayer);

        while (!_gameEngine.IsGameFinished())
        {
            var currentPlayer = _gameEngine.GetCurrentPlayer();
            var currentPlayerInstance = currentPlayer == Player.X ? playerX : playerO;
            
            var move = currentPlayerInstance.SelectMove(_gameEngine.GetState(), currentPlayer);
            
            if (move == null)
            {
                // Player quit/forfeited - treat as loss for that player
                var winner = currentPlayer == Player.X ? Player.O : Player.X;
                var finalState = _gameEngine.GetState();
                return new GameResult(
                    GameId: finalState.GameId,
                    Winner: winner,
                    MoveCount: finalState.MoveHistory.Count,
                    Duration: finalState.MoveHistory.Count > 0 ? 
                        finalState.MoveHistory.Last().Timestamp - finalState.StartTime : 
                        TimeSpan.Zero,
                    StartingPlayer: startingPlayer,
                    MoveHistory: finalState.MoveHistory);
            }
            
            if (!_gameEngine.TryMakeMove(currentPlayer, move.Value, out var error))
            {
                throw new InvalidOperationException(
                    $"Player {currentPlayerInstance.Name} made invalid move: {error}");
            }
        }

        var gameState = _gameEngine.GetState();
        return new GameResult(
            GameId: gameState.GameId,
            Winner: _gameEngine.GetWinner(),
            MoveCount: gameState.MoveHistory.Count,
            Duration: gameState.MoveHistory.Count > 0 ? 
                gameState.MoveHistory.Last().Timestamp - gameState.StartTime : 
                TimeSpan.Zero,
            StartingPlayer: startingPlayer,
            MoveHistory: gameState.MoveHistory);
    }

    /// <summary>
    /// Get the current game state (useful for display during interactive games)
    /// </summary>
    public GameState GetCurrentState() => _gameEngine.GetState();

    /// <summary>
    /// Check if the current game is finished
    /// </summary>
    public bool IsGameFinished() => _gameEngine.IsGameFinished();
}

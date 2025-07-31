using TicTacToe.Engine;
using TicTacToe.Training.Metrics;

namespace TicTacToe.Training;

public class GameLoop
{
    private readonly GameEngine _gameEngine;

    public GameLoop()
    {
        _gameEngine = new GameEngine();
    }

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
                var duration = finalState.MoveHistory.Count > 0 ? 
                    finalState.MoveHistory[finalState.MoveHistory.Count - 1].Timestamp - finalState.StartTime : 
                    TimeSpan.Zero;
                
                var forfeitResult = new GameResult(
                    GameId: finalState.GameId,
                    Winner: winner,
                    MoveCount: finalState.MoveHistory.Count,
                    Duration: duration,
                    StartingPlayer: startingPlayer,
                    MoveHistory: finalState.MoveHistory);
                
                return forfeitResult;
            }
            
            if (!_gameEngine.TryMakeMove(currentPlayer, move.Value, out var error))
            {
                throw new InvalidOperationException(
                    $"Player {currentPlayerInstance.Name} made invalid move: {error}");
            }
        }

        var gameState = _gameEngine.GetState();
        var gameDuration = gameState.MoveHistory.Count > 0 ? 
            gameState.MoveHistory.Last().Timestamp - gameState.StartTime : 
            TimeSpan.Zero;
        
        var gameResult = new GameResult(
            GameId: gameState.GameId,
            Winner: _gameEngine.GetWinner(),
            MoveCount: gameState.MoveHistory.Count,
            Duration: gameDuration,
            StartingPlayer: startingPlayer,
            MoveHistory: gameState.MoveHistory);
        
        return gameResult;
    }

    public GameState GetCurrentState() => _gameEngine.GetState();

    public bool IsGameFinished() => _gameEngine.IsGameFinished();
}

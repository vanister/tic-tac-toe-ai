using TicTacToe.Engine;
using TicTacToe.Training;
using TicTacToe.Training.Players;

namespace TicTacToe.Integrations;

public class UnifiedPlayerIntegrationTests
{
    [Fact]
    public void GameLoop_CanHandleAiVsAiGame()
    {
        // Arrange
        var gameLoop = new GameLoop();
        var playerX = new RandomAiPlayer(seed: 42);
        var playerO = new RandomAiPlayer(seed: 123);

        // Act
        var result = gameLoop.PlayGame(playerX, playerO, Player.X);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Winner == Player.X || result.Winner == Player.O || result.Winner == null);
        Assert.True(result.MoveCount > 0);
        Assert.True(result.MoveCount <= 9);
        Assert.Equal(Player.X, result.StartingPlayer);
    }

    [Fact]
    public void TrainingLoop_WorksWithUnifiedPlayerInterface()
    {
        // Arrange
        var trainingLoop = new TrainingLoop();
        var playerX = new RandomAiPlayer(seed: 42);
        var playerO = new RandomAiPlayer(seed: 123);

        // Act
        var metrics = trainingLoop.RunTraining(playerX, playerO, 10);

        // Assert
        Assert.Equal(10, metrics.TotalGames);
        Assert.Equal(10, metrics.XWins + metrics.OWins + metrics.Draws);
        Assert.True(metrics.AverageGameLength > 0);
        Assert.True(metrics.TotalDuration >= TimeSpan.Zero);
    }

    [Fact]
    public void GameLoop_HandlesDifferentPlayerTypes()
    {
        // Arrange
        var gameLoop = new GameLoop();
        var playerX = new RandomAiPlayer(seed: 42);
        var playerO = new RandomAiPlayer(seed: 999);

        // Act - Different AI players with different seeds should work
        var result1 = gameLoop.PlayGame(playerX, playerO, Player.X);
        var result2 = gameLoop.PlayGame(playerO, playerX, Player.O);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal(Player.X, result1.StartingPlayer);
        Assert.Equal(Player.O, result2.StartingPlayer);
    }

    [Fact]
    public void RandomAiPlayer_NeverReturnsNull()
    {
        // Arrange
        var player = new RandomAiPlayer(seed: 42);
        var gameEngine = new GameEngine();
        gameEngine.StartGame(Player.X);

        // Act & Assert - AI players should never quit/forfeit
        var move = player.SelectMove(gameEngine.GetState(), Player.X);
        Assert.NotNull(move);
        Assert.InRange(move.Value, 0, 8);
    }

    [Fact]
    public void GameLoop_HandlesPlayerQuitting()
    {
        // Arrange
        var gameLoop = new GameLoop();
        var quittingPlayer = new TestQuittingPlayer();
        var aiPlayer = new RandomAiPlayer();

        // Act
        var result = gameLoop.PlayGame(quittingPlayer, aiPlayer, Player.X);

        // Assert - Quitting player should lose
        Assert.Equal(Player.O, result.Winner); // O wins because X quit
        Assert.Equal(Player.X, result.StartingPlayer);
    }

    private class TestQuittingPlayer : IPlayer
    {
        public string Name => "Test Quitting Player";

        public int? SelectMove(GameState gameState, Player player)
        {
            return null; // Always quit
        }
    }
}

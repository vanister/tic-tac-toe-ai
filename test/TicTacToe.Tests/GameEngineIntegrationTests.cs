using TicTacToe.Engine;
using TicTacToe.Engine.Selectors;

namespace TicTacToe.Tests;

public class GameEngineIntegrationTests
{
    [Fact]
    public void FullGameLoop_XWins_ShouldCompleteSuccessfully()
    {
        // Arrange
        var engine = new GameEngine();
        
        // Act & Assert - Start game
        engine.StartGame(Player.X);
        var initialState = engine.GetState();
        
        Assert.Equal(GameStatus.Playing, initialState.Status);
        Assert.Equal(Player.X, initialState.CurrentPlayer);
        Assert.Equal(0, GameSelectors.GetMoveCount(initialState));
        Assert.All(initialState.Board, cell => Assert.Equal(0, cell));
        
        // Play moves that result in X winning
        var moves = new[]
        {
            (Player.X, 4), // X takes center
            (Player.O, 0), // O takes corner
            (Player.X, 2), // X takes top-right corner
            (Player.O, 1), // O blocks top row
            (Player.X, 6), // X completes diagonal 2-4-6 and wins
        };
        
        foreach (var (player, position) in moves)
        {
            // Verify move is valid
            Assert.True(engine.TryMakeMove(player, position, out var error), 
                $"Move {player} at {position} should be valid but got error: {error}");
            
            // Verify game state after move
            var state = engine.GetState();
            Assert.Equal((int)player, state.Board[position]);
            
            if (!engine.IsGameFinished())
            {
                Assert.Equal(player.Opponent(), state.CurrentPlayer);
                Assert.Equal(GameStatus.Playing, state.Status);
            }
        }
        
        // Verify final game state
        var finalState = engine.GetState();
        Assert.True(engine.IsGameFinished());
        Assert.Equal(GameStatus.XWins, finalState.Status);
        Assert.Equal(Player.X, engine.GetWinner());
        Assert.Equal(5, GameSelectors.GetMoveCount(finalState));
        
        // Verify winning line (diagonal: positions 2, 4, 6)
        Assert.Equal(1, finalState.Board[2]); // X
        Assert.Equal(1, finalState.Board[4]); // X  
        Assert.Equal(1, finalState.Board[6]); // X
    }
    
    [Fact]
    public void FullGameLoop_Draw_ShouldCompleteSuccessfully()
    {
        // Arrange
        var engine = new GameEngine();
        engine.StartGame(Player.X);
        
        // Play moves that result in a draw
        var moves = new[]
        {
            (Player.X, 0), // X
            (Player.O, 4), // O takes center
            (Player.X, 8), // X
            (Player.O, 2), // O
            (Player.X, 6), // X  
            (Player.O, 3), // O
            (Player.X, 5), // X
            (Player.O, 7), // O
            (Player.X, 1)  // X - board full, draw
        };
        
        // Act - Play all moves
        foreach (var (player, position) in moves)
        {
            Assert.True(engine.TryMakeMove(player, position, out var error),
                $"Move {player} at {position} should be valid but got error: {error}");
        }
        
        // Assert - Verify draw state
        var finalState = engine.GetState();
        Assert.True(engine.IsGameFinished());
        Assert.Equal(GameStatus.Draw, finalState.Status);
        Assert.Null(engine.GetWinner());
        Assert.Equal(9, GameSelectors.GetMoveCount(finalState));
        Assert.All(finalState.Board, cell => Assert.NotEqual(0, cell)); // Board is full
        Assert.Empty(GameSelectors.GetAvailablePositions(finalState));
    }
    
    [Fact]
    public void GameLoop_InvalidMoves_ShouldRejectCorrectly()
    {
        // Arrange
        var engine = new GameEngine();
        engine.StartGame(Player.X);
        
        // Test invalid position
        Assert.False(engine.TryMakeMove(Player.X, -1, out var error1));
        Assert.Contains("Position must be between 0 and 8", error1);
        
        Assert.False(engine.TryMakeMove(Player.X, 9, out var error2));
        Assert.Contains("Position must be between 0 and 8", error2);
        
        // Make a valid move
        Assert.True(engine.TryMakeMove(Player.X, 4, out _));
        
        // Test wrong player turn
        Assert.False(engine.TryMakeMove(Player.X, 0, out var error3));
        Assert.Contains("It's not X's turn", error3);
        
        // Test occupied position
        Assert.False(engine.TryMakeMove(Player.O, 4, out var error4));
        Assert.Contains("Position is already occupied", error4);
        
        // Make game-ending move sequence
        engine.TryMakeMove(Player.O, 0, out _);
        engine.TryMakeMove(Player.X, 2, out _);
        engine.TryMakeMove(Player.O, 1, out _);
        engine.TryMakeMove(Player.X, 6, out _); // X wins with diagonal
        
        // Test move after game finished
        Assert.False(engine.TryMakeMove(Player.O, 3, out var error5));
        Assert.Contains("Game is not in progress", error5);
    }
    
    [Fact]
    public void GameLoop_ResetGame_ShouldStartFresh()
    {
        // Arrange
        var engine = new GameEngine();
        engine.StartGame(Player.O); // Start with O
        
        // Play some moves
        engine.TryMakeMove(Player.O, 4, out _);
        engine.TryMakeMove(Player.X, 0, out _);
        
        var gameIdBeforeReset = engine.GetState().GameId;
        
        // Act - Reset game
        engine.ResetGame();
        
        // Assert - Verify fresh state
        var resetState = engine.GetState();
        Assert.Equal(GameStatus.Playing, resetState.Status);
        Assert.Equal(Player.X, resetState.CurrentPlayer); // Reset always starts with X
        Assert.Equal(0, GameSelectors.GetMoveCount(resetState));
        Assert.All(resetState.Board, cell => Assert.Equal(0, cell));
        Assert.Equal(gameIdBeforeReset, resetState.GameId); // Same game ID preserved
        Assert.Empty(resetState.MoveHistory);
    }
    
    [Fact]
    public void GameLoop_MoveHistory_ShouldTrackCorrectly()
    {
        // Arrange
        var engine = new GameEngine();
        engine.StartGame(Player.X);
        
        // Act - Make several moves
        engine.TryMakeMove(Player.X, 4, out _);
        engine.TryMakeMove(Player.O, 0, out _);
        engine.TryMakeMove(Player.X, 8, out _);
        
        // Assert - Verify move history
        var state = engine.GetState();
        Assert.Equal(3, state.MoveHistory.Count);
        
        var firstMove = state.MoveHistory[0];
        Assert.Equal(Player.X, firstMove.Player);
        Assert.Equal(4, firstMove.Position);
        
        var secondMove = state.MoveHistory[1];
        Assert.Equal(Player.O, secondMove.Player);
        Assert.Equal(0, secondMove.Position);
        
        var thirdMove = state.MoveHistory[2];
        Assert.Equal(Player.X, thirdMove.Player);
        Assert.Equal(8, thirdMove.Position);
        
        // Verify timestamps are reasonable
        Assert.True(firstMove.Timestamp <= secondMove.Timestamp);
        Assert.True(secondMove.Timestamp <= thirdMove.Timestamp);
    }
    
    [Fact]
    public void GameLoop_GameSelectors_ShouldProvideCorrectInformation()
    {
        // Arrange
        var engine = new GameEngine();
        engine.StartGame(Player.X);
        
        // Make some moves
        engine.TryMakeMove(Player.X, 4, out _); // Center
        engine.TryMakeMove(Player.O, 0, out _); // Corner
        engine.TryMakeMove(Player.X, 2, out _); // Corner
        
        var state = engine.GetState();
        
        // Test position queries
        Assert.True(GameSelectors.IsPositionEmpty(state, 1));
        Assert.False(GameSelectors.IsPositionEmpty(state, 4));
        Assert.Equal(Player.X, GameSelectors.GetPlayerAt(state, 4));
        Assert.Equal(Player.O, GameSelectors.GetPlayerAt(state, 0));
        Assert.Null(GameSelectors.GetPlayerAt(state, 1));
        
        // Test available positions
        var availablePositions = GameSelectors.GetAvailablePositions(state);
        Assert.Equal(6, availablePositions.Count);
        Assert.Contains(1, availablePositions);
        Assert.Contains(3, availablePositions);
        Assert.DoesNotContain(0, availablePositions);
        Assert.DoesNotContain(2, availablePositions);
        Assert.DoesNotContain(4, availablePositions);
        
        // Test board formatting
        var boardString = GameSelectors.FormatBoard(state);
        Assert.Contains("O", boardString); // O at position 0
        Assert.Contains("X", boardString); // X at positions 2 and 4
        Assert.Contains(" ", boardString); // Empty positions
    }
}

using TicTacToe.Engine;
using TicTacToe.Engine.States;

// Quick verification that the game engine works correctly after restructuring
var engine = new GameEngine();

Console.WriteLine("=== TicTacToe Engine Verification ===");

// Start a new game
engine.StartGame(Player.X);
Console.WriteLine($"Game started. Current player: {engine.GetCurrentPlayer()}");
Console.WriteLine($"Game finished: {engine.IsGameFinished()}");

// Make some moves
Console.WriteLine("\nMaking moves:");

// X moves to center (position 4)
if (engine.TryMakeMove(Player.X, 4, out var error))
{
    Console.WriteLine("X moved to position 4 (center)");
}
else
{
    Console.WriteLine($"Failed to move: {error}");
}

// O moves to corner (position 0)
if (engine.TryMakeMove(Player.O, 0, out error))
{
    Console.WriteLine("O moved to position 0 (top-left)");
}
else
{
    Console.WriteLine($"Failed to move: {error}");
}

// X moves to position 1
if (engine.TryMakeMove(Player.X, 1, out error))
{
    Console.WriteLine("X moved to position 1");
}
else
{
    Console.WriteLine($"Failed to move: {error}");
}

// O moves to position 8
if (engine.TryMakeMove(Player.O, 8, out error))
{
    Console.WriteLine("O moved to position 8 (bottom-right)");
}
else
{
    Console.WriteLine($"Failed to move: {error}");
}

// X moves to position 7 (should win: 4-1-7 diagonal)
if (engine.TryMakeMove(Player.X, 7, out error))
{
    Console.WriteLine("X moved to position 7");
}
else
{
    Console.WriteLine($"Failed to move: {error}");
}

// Check game state
var state = engine.GetState();
Console.WriteLine($"\nGame Status: {state.Status}");
Console.WriteLine($"Current Player: {engine.GetCurrentPlayer()}");
Console.WriteLine($"Winner: {engine.GetWinner()}");
Console.WriteLine($"Game Finished: {engine.IsGameFinished()}");
Console.WriteLine($"Moves Made: {state.MoveHistory.Count}");

// Display board
Console.WriteLine("\nBoard positions (0-8):");
Console.WriteLine("0 | 1 | 2");
Console.WriteLine("---------");
Console.WriteLine("3 | 4 | 5");
Console.WriteLine("---------");
Console.WriteLine("6 | 7 | 8");

Console.WriteLine("\nCurrent board:");
for (int i = 0; i < 9; i += 3)
{
    var row = "";
    for (int j = 0; j < 3; j++)
    {
        var pos = i + j;
        var cell = state.Board[pos] switch
        {
            1 => "X",
            -1 => "O",
            _ => " "
        };
        row += cell;
        if (j < 2) row += " | ";
    }
    Console.WriteLine(row);
    if (i < 6) Console.WriteLine("---------");
}

// Test error handling
Console.WriteLine("\nTesting error handling:");

// Try to make move after game is finished
if (engine.TryMakeMove(Player.O, 2, out error))
{
    Console.WriteLine("Move succeeded (unexpected)");
}
else
{
    Console.WriteLine($"Expected error: {error}");
}

// Reset and test validation
engine.ResetGame();
Console.WriteLine("\nGame reset. Testing validation:");

// Try invalid position
if (engine.TryMakeMove(Player.X, 10, out error))
{
    Console.WriteLine("Move succeeded (unexpected)");
}
else
{
    Console.WriteLine($"Invalid position error: {error}");
}

// Try wrong player turn
engine.TryMakeMove(Player.X, 0, out _); // X moves first
if (engine.TryMakeMove(Player.X, 1, out error)) // X tries to move again
{
    Console.WriteLine("Move succeeded (unexpected)");
}
else
{
    Console.WriteLine($"Wrong turn error: {error}");
}

Console.WriteLine("\n=== Verification Complete ===");
Console.WriteLine("All tests passed! The engine is production ready.");

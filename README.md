# Tic Tac Toe AI Training System

A C#/.NET project for building an AI training system for Tic Tac Toe with production-ready architecture patterns.

## Architecture

This project follows a Redux-style state management pattern with strict separation of concerns:

- **`TicTacToe.Engine`** - Core game logic with Redux-style state management
- **`TicTacToe.Console`** - Interactive console application for manual play and debugging
- **`TicTacToe.Integrations`** - Integration tests

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later

### Building the Project

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

## Console Application

The console application provides an interactive way to play Tic Tac Toe manually, which is perfect for testing the game engine and understanding the game mechanics before training AI players.

### Running the Console Game

```bash
dotnet run --project src/TicTacToe.Console
```

Or use the VS Code task: `Ctrl+Shift+P` → "Tasks: Run Task" → "Run Console Game"

### Features

- **Interactive Gameplay**: Play as either X or O with clear turn-based interaction
- **Visual Board Display**: Shows both position numbers (0-8) and current game state
- **Move History**: Complete history of all moves made during the game
- **Game State Debug Info**: View game ID, timing, and move count
- **Input Validation**: Clear error messages for invalid moves
- **Game Rules**: Built-in help system explaining how to play

### Board Layout

The game uses a 9-position grid numbered 0-8:

```
0 | 1 | 2
---------
3 | 4 | 5
---------
6 | 7 | 8
```

### Example Gameplay

1. Choose "Play Game" from the main menu
2. Select who goes first (X or O)
3. Enter positions 0-8 to make moves
4. The game shows the current board state and move history
5. Game ends when someone wins or it's a tie

## Game Engine API

The `GameEngine` class provides a clean API for game management:

```csharp
var engine = new GameEngine();
engine.StartGame(Player.X);

if (engine.TryMakeMove(Player.X, 4, out var error))
{
    // Move successful
}
else
{
    // Handle error
    Console.WriteLine(error);
}

var winner = engine.GetWinner();
var isFinished = engine.IsGameFinished();
```

## Project Structure

```
src/
├── TicTacToe.Engine/     # Core game logic
│   ├── GameEngine.cs     # Public API
│   ├── Actions/          # Redux actions
│   ├── States/           # Game state models
│   ├── Reducers/         # Action routing
│   └── Updaters/         # State transformation logic
└── TicTacToe.Console/    # Interactive console app
    ├── Program.cs        # Entry point
    ├── ConsoleGame.cs    # Game controller
    └── ConsoleGameRenderer.cs # UI rendering

test/
└── TicTacToe.Integrations/ # Integration tests
```

## Future Development

The console application serves as a foundation for:

- **AI Training**: Understanding game mechanics before implementing AI players
- **Debugging**: Testing edge cases and game logic validation
- **Performance Testing**: Analyzing game state management and move validation
- **Rule Verification**: Ensuring the game engine correctly implements Tic Tac Toe rules

Next planned components:
- `TicTacToe.Training` - AI players and training loops
- Enhanced metrics collection for AI training
- Multiple AI strategies and performance comparison

## Development

This project follows strict coding standards:
- Redux-style state management with immutable updates
- Guard clauses and early returns for control flow
- Comprehensive validation before state changes
- Integration-focused testing approach

See `instructions.md` and `.github/copilot-instructions.md` for detailed development guidelines.

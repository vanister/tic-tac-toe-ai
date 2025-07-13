# Tic Tac Toe AI - Copilot Instructions

This is a C#/.NET project building an AI training system for Tic Tac Toe with production-ready architecture patterns.

## Architecture Overview

### Redux-Style State Management
The core follows a Redux pattern with strict separation of concerns:
- **`GameEngine`** - Public API layer with validation (in root of Engine project)
- **`States/GameStore`** - Manages current state and dispatches actions
- **`States/GameReducer`** - Routes actions to appropriate updaters (pure routing only)
- **`States/GameUpdater`** - Contains all state transformation logic
- **`States/GameAction`** - Action definitions (`StartGameAction`, `MakeMoveAction`, `ResetGameAction`)

### Key State Model
- **Board**: `int[]` of 9 elements (1=X, -1=O, 0=empty) representing positions 0-8
- **Players**: `Player.X = 1`, `Player.O = -1` (numeric for AI training)
- **Immutable**: All state updates return new `GameState` records using `with` syntax
- **Move History**: Complete replay capability with timestamped `Move` records

## Critical Patterns

### Validation Before Dispatch
```csharp
// GameEngine validates BEFORE dispatching to store
private string? ValidateMove(Player player, int position)
{
    // Validation logic here - NOT in reducer
}
```

### Action → Reducer → Updater Flow
```csharp
// Reducer only routes, never contains logic
StartGameAction startAction => GameUpdater.StartGame(startAction)
```

### State Transformation
```csharp
// Always use 'with' for immutable updates
return state with { Board = newBoard, Status = newStatus };
```

## Code Style Requirements (From instructions.md)

### Control Flow (Strictly Enforced)
- Guard clauses first with early returns
- Never `if (condition) return value;` - always use separate lines
- Always use curly braces: `if (condition) { return value; }`
- Max 2-3 levels of nesting
- Keep functions focused on single responsibility
- Format code to 100 characters per line maximum

### Comments Policy
- NO XML documentation comments unless explicitly requested
- Limit comments to essential explanations of business logic or complex algorithms
- Prefer explicit, readable code over terse implementations
- Use descriptive variable and function names

### AI Assistant Behavior
- Always confirm with user before writing or implementing code
- Always ask before writing or implementing tests
- Follow .editorconfig rules if present
- Prefer integration-style tests over unit tests when possible

## Testing Guidelines (From instructions.md)
- Use xUnit for all tests
- Test method names should clearly describe the scenario being tested
- Aim for high test coverage, especially for core game logic
- Tests should be fast, isolated, and repeatable
- Mock external dependencies where possible
- AI assistant will NOT manage testing guidelines unless explicitly asked

### Dependency Management
- Use NuGet packages for external dependencies
- Keep dependencies updated but avoid experimental packages
- Remove unused dependencies promptly
- AI assistant will NOT add/update/remove dependencies unless explicitly asked

## Development Commands

### Building & Testing
```bash
dotnet build          # Build from solution root
dotnet test           # Run xUnit tests
```

### Project Structure
```
TicTacToe.Engine/     # Game logic (currently implemented)
TicTacToe.Tests/      # xUnit tests (empty - tests need confirmation)
```

## Game Logic Specifics

### Board Positions
```
0 | 1 | 2
---------
3 | 4 | 5    # Position mapping for 9-element array
---------
6 | 7 | 8
```

### Winner Detection
Uses sum-based detection on 8 winning combinations:
- `sum == 3` → X wins, `sum == -3` → O wins
- Rows: `[0,1,2], [3,4,5], [6,7,8]`
- Columns: `[0,3,6], [1,4,7], [2,5,8]`
- Diagonals: `[0,4,8], [2,4,6]`

### API Usage Patterns
```csharp
var engine = new GameEngine();
engine.StartGame(Player.X);
if (engine.TryMakeMove(Player.X, 4, out var error)) {
    // Success path
} else {
    // Handle error message
}
```

## Future Architecture (From game-design.md)
- `TicTacToe.Training` - AI players and training loops
- `TicTacToe.Console` - Entry point for running training
- Focus on metrics collection and AI training infrastructure

## Important Notes
- Always confirm before implementing code or tests
- Follow `instructions.md` for detailed coding standards
- State management is complete; focus areas are training/AI components
- Board representation optimized for AI training (numeric encoding)

# Tic Tac Toe AI Game Design

## Overview
Building an AI training system for Tic Tac Toe using C#/.NET to learn reinforcement learning concepts with a production-ready architecture.

## Architecture

### Project Structure
```
TicTacToeAI.Engine     - Game logic and state management
TicTacToeAI.Training   - AI players and training loops
TicTacToeAI.Console    - Entry point for running training
TicTacToeAI.Tests      - Unit tests
```

### Core Components

#### Game Engine
- **Responsibility**: Manage game state and enforce rules
- **Pattern**: Redux-like state management with immutable updates
- **Key APIs**:
  - `StartGame(startingPlayer)` - Initialize new game
  - `MakeMove(player, position)` - Validate and execute move
  - `ResetGame()` - Start fresh
  - `GetState()` - Current game state
  - `IsGameFinished()` - Check if game over

#### State Management
- **State Shape**:
  ```csharp
  {
    Board: int[],           // 9-element array: 1 (X), -1 (O), 0 (empty)
    CurrentPlayer: int,     // 1 or -1
    GameStatus: string,     // "playing", "x_wins", "o_wins", "draw"
    MoveHistory: Move[],    // For replay and analysis
    Winner: int?            // 1, -1, or null
  }
  ```

- **Actions**:
  - `start_game` with payload `{startingPlayer: int}`
  - `make_move` with payload `{player: int, position: int}`
  - `reset_game` with payload `{}`

#### Board Representation
- 3x3 grid flattened to single array (positions 0-8)
- Numeric encoding: 1 = X, -1 = O, 0 = empty
- Position mapping:
  ```
  0 | 1 | 2
  ---------
  3 | 4 | 5
  ---------
  6 | 7 | 8
  ```

## Game Logic

### Move Validation
**Order of validation checks**:
1. Game status is "playing"
2. Correct player's turn
3. Position is valid (0-8)
4. Position is empty

**Return specific error messages** (not just booleans) for better debugging and AI training feedback.

### Winner Detection
**Winning combinations** (8 total):
```csharp
private readonly int[][] WinningCombinations = {
    [0, 1, 2], [3, 4, 5], [6, 7, 8],  // rows
    [0, 3, 6], [1, 4, 7], [2, 5, 8],  // columns
    [0, 4, 8], [2, 4, 6]              // diagonals
};
```

### Game Flow
1. Validate move
2. Update board state
3. Check for winner/draw
4. Switch current player (if game continues)
5. All in single atomic operation

## Training System

### Training Loop Design
**Separate programs for different scenarios**:
- **Training Loop**: AI vs AI, thousands of games, collect data
- **Human vs AI Loop**: Handle user input, display board
- **Tournament Loop**: Multiple AIs competing
- **Debug/Replay Loop**: Step through games

### AI Training Loop Structure
```csharp
// Outer loop: number of games
for (int game = 0; game < numGames; game++) {
    var startingPlayer = DetermineStartingPlayer(game);
    gameEngine.StartGame(startingPlayer);
    
    // Inner loop: game moves
    while (!gameEngine.IsGameFinished()) {
        var currentPlayer = gameEngine.GetCurrentPlayer();
        var move = GetAiMove(currentPlayer, gameEngine.GetState());
        gameEngine.MakeMove(currentPlayer, move);
    }
    
    RecordGameResult(gameEngine.GetWinner());
}
```

### AI Player Progression
**Phase 1**: Random moves - validate infrastructure
**Phase 2**: Rule-based AI - simple heuristics
**Phase 3**: Q-learning - reinforcement learning
**Phase 4**: Advanced techniques

### Rule-Based AI Strategy
**Priority order**:
1. Win immediately (complete 2-in-a-row)
2. Block opponent wins
3. Take center position
4. Take corners
5. Take edges

## Metrics and Analytics

### Game-Level Metrics
- Move history (from Redux store)
- Winner or draw result
- Winning move position
- Game duration
- Starting player
- Game length (number of moves)

### Training Progress Indicators
- **Move quality improvement** over time
- **Game duration decrease** (faster decisions)
- **Invalid move attempts decrease**
- **Win rate** against rule-based opponents
- **Strategic consistency** (variance in play style)

### Implementation
```csharp
class GameMetrics {
    public string GameId { get; set; }
    public List<Move> MoveHistory { get; set; }
    public int? Winner { get; set; }
    public int? WinningMove { get; set; }
    public TimeSpan Duration { get; set; }
    public int FirstPlayer { get; set; }
    public int GameLength { get; set; }
}
```

## Development Approach

### Build Strategy
1. **KISS Principle**: Start simple, add complexity gradually
2. **Validation First**: Build complete game engine and validate correctness
3. **Incremental AI**: Random → Rules → Learning algorithms
4. **Metrics Early**: Track everything from the beginning

### Testing Strategy
- **Unit tests** for game logic validation
- **Integration tests** for training loop
- **Parameterized tests** for different board states
- **Performance tests** for training speed

### Technology Stack
- **Language**: C#/.NET
- **Testing**: xUnit
- **ML Framework**: ML.NET (for Q-learning)
- **Development**: VS Code with C# extension

## Next Steps

1. **Phase 1**: Build game engine and validation
2. **Phase 2**: Create training loop with random AI
3. **Phase 3**: Implement rule-based AI
4. **Phase 4**: Add Q-learning with ML.NET
5. **Phase 5**: Advanced training techniques

## Key Design Decisions

- **Single atomic moves**: Game engine handles move + winner check in one operation
- **Immutable state**: Redux-like pattern for predictable state management
- **Separation of concerns**: Game engine vs training loop vs AI players
- **Metrics-first**: Track everything for learning insights
- **Production patterns**: Build like a real system from the start
# Training Data Format

This directory contains training results from AI vs AI Tic Tac Toe games in JSON Lines format.

## File Format

Each `.jsonl` file contains training session data in JSON Lines format:
- **Line 1**: Session metadata with overall statistics
- **Lines 2+**: Individual game results

### Session Metadata Format
```json
{
  "type": "session",
  "data": {
    "sessionId": "7d82bee0",
    "timestamp": "2025-07-28T19:21:45.314974Z",
    "playerXName": "Random AI",
    "playerOName": "Random AI", 
    "totalGames": 100,
    "xWins": 48,
    "oWins": 46,
    "draws": 6,
    "averageGameLength": 7.51,
    "totalDuration": "00:00:00.0073750",
    "games": [/* summary of all games */]
  }
}
```

### Game Result Format  
```json
{
  "type": "game",
  "data": {
    "gameId": "1f36478f-3d98-498c-8f8f-4e6bbf18c9c0",
    "timestamp": "2025-07-28T19:21:45.307323Z",
    "winner": "X", // "X", "O", or null for draw
    "moveCount": 7,
    "duration": "00:00:00.0017640",
    "startingPlayer": "X",
    "moves": [
      {
        "player": "X",
        "position": 6, // 0-8 representing board positions
        "timestamp": "2025-07-28T19:21:45.307323Z"
      }
      // ... more moves
    ]
  }
}
```

## Board Position Mapping
```
0 | 1 | 2
---------
3 | 4 | 5
---------  
6 | 7 | 8
```

## Usage Examples

### Command Line Training with Data Export
```bash
# Auto-save for runs ≥100 games
dotnet run --project src/TicTacToe.Training -- 1000

# Explicit output file
dotnet run --project src/TicTacToe.Training -- 500 --output data/training/my-session.jsonl
dotnet run --project src/TicTacToe.Training -- 500 -o data/training/my-session.jsonl

# View sample games during training
dotnet run --project src/TicTacToe.Training -- 100 --samples -o data/training/detailed-run.jsonl
```

### Interactive Console
- Run training sessions and choose to save data when prompted
- Data automatically saved with timestamp-based filenames

### Analyzing Saved Data
```bash
# Count total games in a session
wc -l data/training/session.jsonl  # subtract 1 for session line

# View session summary
head -n 1 data/training/session.jsonl | jq '.data'

# View first few games  
tail -n +2 data/training/session.jsonl | head -n 5 | jq '.data.gameId, .data.winner'

# Extract all winners
tail -n +2 data/training/session.jsonl | jq -r '.data.winner // "Draw"' | sort | uniq -c
```

## File Naming Convention

- **Auto-generated**: `training-YYYYMMDD-HHMMSS.jsonl`
- **Explicit**: User-specified filename
- **Interactive**: Auto-generated with timestamp

## Git Policy

Training data files are excluded from version control via `.gitignore` to avoid repository bloat. Only the directory structure and this documentation are tracked.

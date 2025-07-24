namespace TicTacToe.Engine;

public record GameState(
    int[] Board,
    Player CurrentPlayer,
    GameStatus Status,
    IReadOnlyList<Move> MoveHistory,
    string GameId,
    DateTime StartTime)
{
    public static GameState CreateNew(Player startingPlayer, string? gameId = null)
    {
        return new GameState(
            Board: new int[9], // All zeros (empty)
            CurrentPlayer: startingPlayer,
            Status: GameStatus.Playing,
            MoveHistory: new List<Move>(),
            GameId: gameId ?? Guid.NewGuid().ToString(),
            StartTime: DateTime.UtcNow
        );
    }
}

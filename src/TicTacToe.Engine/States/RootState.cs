namespace TicTacToe.Engine.States;

public record RootState(
    GameState Game,
    HistoryState History)
{
    public static RootState CreateNew(Player startingPlayer, string? gameId = null)
    {
        return new RootState(
            Game: GameState.CreateNew(startingPlayer, gameId),
            History: HistoryState.CreateNew()
        );
    }
}

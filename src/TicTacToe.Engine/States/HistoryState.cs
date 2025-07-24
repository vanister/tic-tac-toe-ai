namespace TicTacToe.Engine.States;

public record HistoryState(
    IReadOnlyList<GameAction> Actions)
{
    public static HistoryState CreateNew()
    {
        return new HistoryState(
            Actions: []
        );
    }
}

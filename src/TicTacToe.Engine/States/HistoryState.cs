using TicTacToe.Engine.Actions;

namespace TicTacToe.Engine.States;

public record HistoryState(
    IReadOnlyList<Actions.GameAction> Actions)
{
    public static HistoryState CreateNew()
    {
        return new HistoryState(
            Actions: []
        );
    }
}

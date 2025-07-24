namespace TicTacToe.Engine.States;

public static class HistoryUpdater
{
    public static HistoryState AddAction(HistoryState state, GameAction action)
    {
        var newActions = state.Actions.ToList();
        newActions.Add(action);

        return state with { Actions = newActions };
    }
}

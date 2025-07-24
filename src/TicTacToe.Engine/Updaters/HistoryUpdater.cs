using TicTacToe.Engine.Actions;
using TicTacToe.Engine.States;

namespace TicTacToe.Engine.Updaters;

public static class HistoryUpdater
{
    public static HistoryState AddAction(HistoryState state, Actions.GameAction action)
    {
        var newActions = state.Actions.ToList();
        newActions.Add(action);

        return state with { Actions = newActions };
    }
}

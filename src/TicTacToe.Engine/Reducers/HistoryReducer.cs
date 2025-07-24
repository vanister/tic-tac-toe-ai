using TicTacToe.Engine.Actions;
using TicTacToe.Engine.States;
using TicTacToe.Engine.Updaters;

namespace TicTacToe.Engine.Reducers;

public static class HistoryReducer
{
    public static HistoryState Reduce(HistoryState state, Actions.GameAction action)
    {
        return HistoryUpdater.AddAction(state, action);
    }
}

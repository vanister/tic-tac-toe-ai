namespace TicTacToe.Engine.States;

public static class HistoryReducer
{
    public static HistoryState Reduce(HistoryState state, GameAction action)
    {
        return HistoryUpdater.AddAction(state, action);
    }
}

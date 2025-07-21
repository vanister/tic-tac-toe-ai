namespace TicTacToe.Engine.States;

public static class HistoryReducer
{
    public static HistoryState Reduce(HistoryState state, GameAction action)
    {
        return state.AddAction(action);
    }
}

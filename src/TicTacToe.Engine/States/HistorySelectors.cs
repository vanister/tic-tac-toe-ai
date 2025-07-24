namespace TicTacToe.Engine.States;

public static class HistorySelectors
{
    public static int GetActionCount(HistoryState state)
    {
        return state.Actions.Count;
    }
    
    public static IReadOnlyList<GameAction> GetActions(HistoryState state)
    {
        return state.Actions;
    }
    
    public static GameAction? GetLastAction(HistoryState state)
    {
        return state.Actions.LastOrDefault();
    }
    
    public static IReadOnlyList<GameAction> GetActionsOfType<T>(HistoryState state) where T : GameAction
    {
        return state.Actions.OfType<T>().ToList();
    }
}

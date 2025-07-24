using TicTacToe.Engine.Actions;
using TicTacToe.Engine.States;

namespace TicTacToe.Engine.Selectors;

public static class HistorySelectors
{
    public static int GetActionCount(HistoryState state)
    {
        return state.Actions.Count;
    }
    
    public static IReadOnlyList<Actions.GameAction> GetActions(HistoryState state)
    {
        return state.Actions;
    }
    
    public static Actions.GameAction? GetLastAction(HistoryState state)
    {
        return state.Actions.LastOrDefault();
    }
    
    public static IReadOnlyList<Actions.GameAction> GetActionsOfType<T>(HistoryState state) where T : Actions.GameAction
    {
        return state.Actions.OfType<T>().ToList();
    }
}

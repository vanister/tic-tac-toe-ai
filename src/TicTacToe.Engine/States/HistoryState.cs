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
    
    public HistoryState AddAction(GameAction action)
    {
        var newActions = Actions.ToList();
        newActions.Add(action);
        return this with { Actions = newActions };
    }
    
    public int ActionCount => Actions.Count;
}

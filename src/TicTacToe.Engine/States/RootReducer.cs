namespace TicTacToe.Engine.States;

public static class RootReducer
{
    public static RootState Reduce(RootState state, GameAction action)
    {
        var newGameState = GameReducer.Reduce(state.Game, action);
        return state with { Game = newGameState };
    }
}

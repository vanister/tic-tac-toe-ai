using TicTacToe.Engine.Actions;
using TicTacToe.Engine.States;

namespace TicTacToe.Engine.Reducers;

public static class RootReducer
{
    public static RootState Reduce(RootState state, Actions.GameAction action)
    {
        var newGameState = GameReducer.Reduce(state.Game, action);
        var newHistoryState = HistoryReducer.Reduce(state.History, action);
        
        return state with 
        { 
            Game = newGameState,
            History = newHistoryState
        };
    }
}

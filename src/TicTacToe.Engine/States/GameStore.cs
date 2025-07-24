using TicTacToe.Engine.Actions;
using TicTacToe.Engine.Reducers;

namespace TicTacToe.Engine.States;

public class GameStore(RootState? initialState = null)
{
    private RootState _currentState = initialState ?? RootState.CreateNew(Player.X);

    public RootState State => _currentState;

    public void Dispatch(Actions.GameAction action)
    {
        var newState = RootReducer.Reduce(_currentState, action);

        _currentState = newState;

    }
}

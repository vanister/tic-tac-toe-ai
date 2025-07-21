namespace TicTacToe.Engine.States;

public class GameStore(RootState? initialState = null)
{
    private RootState _currentState = initialState ?? RootState.CreateNew(Player.X);

    public RootState State => _currentState;

    public void Dispatch(GameAction action)
    {
        var newState = RootReducer.Reduce(_currentState, action);

        _currentState = newState;

    }
}

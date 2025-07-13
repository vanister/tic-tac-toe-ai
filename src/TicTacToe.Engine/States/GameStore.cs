namespace TicTacToe.Engine.States;

public class GameStore(GameState? initialState = null)
{
    private GameState _currentState = initialState ?? GameState.CreateNew(Player.X);

    public GameState CurrentState => _currentState;

    public void Dispatch(GameAction action)
    {
        var newState = GameReducer.Reduce(_currentState, action);
        
        if (!ReferenceEquals(newState, _currentState))
        {
            _currentState = newState;
        }
    }
}

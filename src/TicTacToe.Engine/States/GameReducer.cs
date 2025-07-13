namespace TicTacToe.Engine.States;

public static class GameReducer
{
    public static GameState Reduce(GameState state, GameAction action)
    {
        return action switch
        {
            StartGameAction startAction => GameUpdater.StartGame(startAction),
            MakeMoveAction moveAction => GameUpdater.MakeMove(state, moveAction),
            ResetGameAction => GameUpdater.ResetGame(state),
            _ => state
        };
    }
}

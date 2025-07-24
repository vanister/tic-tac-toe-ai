using TicTacToe.Engine.Actions;
using TicTacToe.Engine.States;
using TicTacToe.Engine.Updaters;

namespace TicTacToe.Engine.Reducers;

public static class GameReducer
{
    public static GameState Reduce(GameState state, Actions.GameAction action)
    {
        return action switch
        {
            Actions.StartGameAction startAction => Updaters.GameUpdater.StartGame(startAction),
            Actions.MakeMoveAction moveAction => Updaters.GameUpdater.MakeMove(state, moveAction),
            Actions.ResetGameAction => Updaters.GameUpdater.ResetGame(state),
            _ => state
        };
    }
}

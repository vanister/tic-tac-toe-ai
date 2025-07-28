using TicTacToe.Engine.Actions;
using TicTacToe.Engine.Selectors;
using TicTacToe.Engine.States;

namespace TicTacToe.Engine;

public class GameEngine
{
    private readonly GameStore _store;

    public GameEngine()
    {
        _store = new GameStore();
    }

    public GameState GetState() => _store.State.Game;

    public bool IsGameFinished() => GameSelectors.IsFinished(_store.State.Game);

    public Player GetCurrentPlayer() => _store.State.Game.CurrentPlayer;

    public Player? GetWinner() => GameSelectors.GetWinner(_store.State.Game);

    public void StartGame(Player startingPlayer, string? gameId = null)
    {
        _store.Dispatch(new StartGameAction(startingPlayer, gameId));
    }

    public bool TryMakeMove(Player player, int position, out string? error)
    {
        error = ValidateMove(player, position);
        
        if (error != null)
        {
            return false;
        }

        _store.Dispatch(new MakeMoveAction(player, position));
        return true;
    }

    public void MakeMove(Player player, int position)
    {
        if (!TryMakeMove(player, position, out var error))
        {
            throw new InvalidOperationException(error);
        }
    }

    public void ResetGame()
    {
        _store.Dispatch(new ResetGameAction());
    }

    public bool IsValidMove(Player player, int position, out string? error)
    {
        error = ValidateMove(player, position);
        return error == null;
    }

    private string? ValidateMove(Player player, int position)
    {
        var state = _store.State.Game;

        if (state.Status != GameStatus.Playing)
        {
            return "Game is not in progress";
        }

        if (player != state.CurrentPlayer)
        {
            return $"It's not {player}'s turn";
        }

        if (position < 0 || position >= 9)
        {
            return "Position must be between 0 and 8";
        }

        if (!GameSelectors.IsPositionEmpty(state, position))
        {
            return "Position is already occupied";
        }

        return null;
    }
}

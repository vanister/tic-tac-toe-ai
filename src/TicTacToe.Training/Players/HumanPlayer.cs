using TicTacToe.Engine;
using TicTacToe.Console;

namespace TicTacToe.Training.Players;

public class HumanPlayer : IPlayer
{
    private readonly IGameRenderer _renderer;

    public string Name { get; }

    public HumanPlayer(string name, IGameRenderer renderer)
    {
        Name = name;
        _renderer = renderer;
    }

    public int? SelectMove(GameState gameState, Player player)
    {
        return _renderer.GetPlayerMove(player, gameState.Board, gameState);
    }
}

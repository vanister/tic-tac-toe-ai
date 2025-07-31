using TicTacToe.Engine;

namespace TicTacToe.Training;

public interface IPlayer
{
    string Name { get; }
    int? SelectMove(GameState gameState, Player player);
}

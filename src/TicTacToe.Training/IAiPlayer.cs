using TicTacToe.Engine;

namespace TicTacToe.Training;

/// <summary>
/// Interface for AI players that can select moves in a Tic Tac Toe game
/// </summary>
public interface IAiPlayer
{
    string Name { get; }
    int SelectMove(GameState gameState, Player player);
}

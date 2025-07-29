using TicTacToe.Engine;

namespace TicTacToe.Training;

/// <summary>
/// Interface for all players (AI and human) that can select moves in a Tic Tac Toe game
/// </summary>
public interface IPlayer
{
    string Name { get; }
    int? SelectMove(GameState gameState, Player player);
}

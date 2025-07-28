using TicTacToe.Engine;

namespace TicTacToe.Console;

public interface IGameRenderer
{
    void ShowWelcome();
    string ShowMainMenu();
    Player? GetStartingPlayer();
    void DisplayGameState(GameState state);
    int? GetPlayerMove(Player player, int[] board, GameState gameState);
    void ShowError(string message);
    void ShowGameResult(Player? winner);
    void ShowGameRules();
    void ShowInvalidChoice();
    void ShowReturnToMenu();
}

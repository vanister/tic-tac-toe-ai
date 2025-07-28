using TicTacToe.Engine;

namespace TicTacToe.Console;

public class FileLogRenderer : IGameRenderer
{
    private readonly string _logFilePath;
    private readonly IGameRenderer _innerRenderer;

    public FileLogRenderer(string logFilePath, IGameRenderer innerRenderer)
    {
        _logFilePath = logFilePath;
        _innerRenderer = innerRenderer;
        
        // Initialize log file
        File.WriteAllText(_logFilePath, $"=== Tic Tac Toe Game Log - {DateTime.Now} ==={Environment.NewLine}");
    }

    public void ShowWelcome()
    {
        Log("WELCOME: Showing welcome screen");
        _innerRenderer.ShowWelcome();
    }

    public string ShowMainMenu()
    {
        Log("MENU: Displaying main menu");
        var choice = _innerRenderer.ShowMainMenu();
        Log($"MENU: User selected '{choice}'");
        return choice;
    }

    public Player? GetStartingPlayer()
    {
        Log("SETUP: Getting starting player");
        var player = _innerRenderer.GetStartingPlayer();
        Log($"SETUP: Starting player selected: {(player?.ToString() ?? "None (cancelled)")}");
        return player;
    }

    public void DisplayGameState(GameState state)
    {
        Log($"GAME STATE: Move {state.MoveHistory.Count}, Status: {state.Status}, Current Player: {state.CurrentPlayer}");
        Log($"BOARD: [{string.Join(",", state.Board)}]");
        _innerRenderer.DisplayGameState(state);
    }

    public int? GetPlayerMove(Player player, int[] board, GameState gameState)
    {
        Log($"INPUT: Getting move for player {player}");
        var position = _innerRenderer.GetPlayerMove(player, board, gameState);
        Log($"INPUT: Player {player} chose position {(position?.ToString() ?? "quit")}");
        return position;
    }

    public void ShowError(string message)
    {
        Log($"ERROR: {message}");
        _innerRenderer.ShowError(message);
    }

    public void ShowGameResult(Player? winner)
    {
        Log($"RESULT: Game finished - Winner: {(winner?.ToString() ?? "Tie")}");
        _innerRenderer.ShowGameResult(winner);
    }

    public void ShowGameRules()
    {
        Log("RULES: Displaying game rules");
        _innerRenderer.ShowGameRules();
    }

    public void ShowInvalidChoice()
    {
        Log("ERROR: Invalid menu choice");
        _innerRenderer.ShowInvalidChoice();
    }

    public void ShowReturnToMenu()
    {
        Log("NAVIGATION: Returning to main menu");
        _innerRenderer.ShowReturnToMenu();
    }

    public void WaitForKeyPress(string? message = null)
    {
        Log($"INPUT: Waiting for key press - {message ?? "Press any key to continue..."}");
        _innerRenderer.WaitForKeyPress(message);
    }

    private void Log(string message)
    {
        var logEntry = $"[{DateTime.Now:HH:mm:ss.fff}] {message}{Environment.NewLine}";
        File.AppendAllText(_logFilePath, logEntry);
    }
}

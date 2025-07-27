using TicTacToe.Engine;

namespace TicTacToe.Console;

public class ConsoleGameRenderer
{
    public static void ShowWelcome()
    {
        System.Console.WriteLine("╔══════════════════════════════════════╗");
        System.Console.WriteLine("║          TIC TAC TOE GAME            ║");
        System.Console.WriteLine("║      Manual Play & Debug Mode       ║");
        System.Console.WriteLine("╚══════════════════════════════════════╝");
        System.Console.WriteLine();
    }

    public static string ShowMainMenu()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Main Menu:");
        System.Console.WriteLine("1. Play Game");
        System.Console.WriteLine("2. Show Rules");
        System.Console.WriteLine("3. Exit");
        System.Console.WriteLine();
        System.Console.Write("Choose an option (1-3): ");
        return System.Console.ReadLine() ?? "";
    }

    public static Player? GetStartingPlayer()
    {
        System.Console.WriteLine("Who goes first?");
        System.Console.WriteLine("1. X (crosses)");
        System.Console.WriteLine("2. O (noughts)");
        System.Console.WriteLine("3. Back to main menu");
        System.Console.WriteLine();
        System.Console.Write("Choose (1-3): ");
        
        var choice = System.Console.ReadLine();
        
        return choice switch
        {
            "1" => Player.X,
            "2" => Player.O,
            _ => null
        };
    }

    public void DisplayGameState(GameState state)
    {
        System.Console.Clear();
        System.Console.WriteLine("╔══════════════════════════════════════╗");
        System.Console.WriteLine("║          TIC TAC TOE GAME            ║");
        System.Console.WriteLine("╚══════════════════════════════════════╝");
        System.Console.WriteLine();
        
        DisplayBoard(state.Board);
        System.Console.WriteLine();
        
        if (state.Status == GameStatus.Playing)
        {
            System.Console.WriteLine($"Current Player: {state.CurrentPlayer.ToChar()}");
        }

        DisplayGameInfo(state);
    }

    private void DisplayBoard(int[] board)
    {
        System.Console.WriteLine("Board positions (0-8) and current state:");
        System.Console.WriteLine();
        
        // Show position numbers
        System.Console.WriteLine("Positions:     Current:");
        System.Console.WriteLine("0 | 1 | 2      {0} | {1} | {2}",
            GetDisplayChar(board[0], 0), GetDisplayChar(board[1], 1), GetDisplayChar(board[2], 2));
        System.Console.WriteLine("----------     ----------");
        System.Console.WriteLine("3 | 4 | 5      {0} | {1} | {2}",
            GetDisplayChar(board[3], 3), GetDisplayChar(board[4], 4), GetDisplayChar(board[5], 5));
        System.Console.WriteLine("----------     ----------");
        System.Console.WriteLine("6 | 7 | 8      {0} | {1} | {2}",
            GetDisplayChar(board[6], 6), GetDisplayChar(board[7], 7), GetDisplayChar(board[8], 8));
    }

    private static char GetDisplayChar(int boardValue, int position)
    {
        if (boardValue == 0)
        {
            return position.ToString()[0];
        }
        
        return boardValue == 1 ? 'X' : 'O';
    }

    private static void DisplayGameInfo(GameState state)
    {
        System.Console.WriteLine($"Game ID: {state.GameId[..8]}..."); // Show first 8 chars
        System.Console.WriteLine($"Moves played: {state.MoveHistory.Count}");
        System.Console.WriteLine($"Started: {state.StartTime:HH:mm:ss}");
        
        if (state.MoveHistory.Count > 0)
        {
            System.Console.WriteLine("\nMove History:");
            
            for (int i = 0; i < state.MoveHistory.Count; i++)
            {
                var move = state.MoveHistory[i];
                System.Console.WriteLine($"  {i + 1}. {move.Player.ToChar()} → Position {move.Position}");
            }
        }
    }

    public int? GetPlayerMove(Player player)
    {
        System.Console.WriteLine();
        System.Console.WriteLine($"Player {player.ToChar()}'s turn");
        System.Console.Write("Enter position (0-8) or 'q' to quit: ");
        
        var input = System.Console.ReadLine()?.Trim().ToLower();
        
        if (input == "q" || input == "quit")
        {
            return null;
        }
        
        if (int.TryParse(input, out var position) && position >= 0 && position <= 8)
        {
            return position;
        }

        ShowError("Invalid input. Please enter a number between 0 and 8.");
        return GetPlayerMove(player); // Recursive retry
    }

    public static void ShowError(string message)
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"Error: {message}");
        System.Console.ResetColor();
    }

    public static void ShowGameResult(Player? winner)
    {
        System.Console.WriteLine();
        System.Console.WriteLine("═══════════════════════════════════════");
        
        if (winner.HasValue)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"🎉 GAME OVER - Player {winner.Value.ToChar()} WINS! 🎉");
        }
        else
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("🤝 GAME OVER - IT'S A TIE! 🤝");
        }
        
        System.Console.ResetColor();
        System.Console.WriteLine("═══════════════════════════════════════");
    }

    public static void ShowGameRules()
    {
        System.Console.Clear();
        System.Console.WriteLine("╔══════════════════════════════════════╗");
        System.Console.WriteLine("║            GAME RULES                ║");
        System.Console.WriteLine("╚══════════════════════════════════════╝");
        System.Console.WriteLine();
        System.Console.WriteLine("Tic Tac Toe Rules:");
        System.Console.WriteLine();
        System.Console.WriteLine("• The game is played on a 3x3 grid");
        System.Console.WriteLine("• Players take turns placing X or O");
        System.Console.WriteLine("• First player to get 3 in a row wins");
        System.Console.WriteLine("• 3 in a row can be horizontal, vertical, or diagonal");
        System.Console.WriteLine("• If all 9 squares are filled without a winner, it's a tie");
        System.Console.WriteLine();
        System.Console.WriteLine("Board positions are numbered 0-8:");
        System.Console.WriteLine();
        System.Console.WriteLine("0 | 1 | 2");
        System.Console.WriteLine("----------");
        System.Console.WriteLine("3 | 4 | 5");
        System.Console.WriteLine("----------");
        System.Console.WriteLine("6 | 7 | 8");
        System.Console.WriteLine();
        System.Console.WriteLine("Debug Features:");
        System.Console.WriteLine("• View complete move history");
        System.Console.WriteLine("• See game state and timing information");
        System.Console.WriteLine("• Position validation with clear error messages");
    }

    public void ShowInvalidChoice()
    {
        ShowError("Invalid choice. Please select 1, 2, or 3.");
    }

    public static void WaitForKeyPress()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadKey(true);
    }
}

using TicTacToe.Engine;

namespace TicTacToe.Console;

public class ConsoleGameRenderer : IGameRenderer
{
    public void ShowWelcome()
    {
        ShowHeader();
        System.Console.WriteLine("║      Manual Play & Debug Mode       ║");
        System.Console.WriteLine("╚══════════════════════════════════════╝");
        System.Console.WriteLine();
    }

    public string ShowMainMenu()
    {
        System.Console.Clear();
        ShowHeader();
        
        System.Console.WriteLine("Main Menu:");
        System.Console.WriteLine("1. Play Game");
        System.Console.WriteLine("2. Show Rules");
        System.Console.WriteLine("q. Exit");
        System.Console.WriteLine();
        System.Console.Write("Choose an option (1, 2, or q): ");
        return System.Console.ReadLine() ?? "";
    }

    public Player? GetStartingPlayer()
    {
        System.Console.Clear();
        ShowHeader();
        
        System.Console.WriteLine("Who goes first?");
        System.Console.WriteLine("1. X (crosses)");
        System.Console.WriteLine("2. O (noughts)");
        System.Console.WriteLine("q. Back to main menu");
        System.Console.WriteLine();
        System.Console.Write("Choose (1, 2, or q): ");
        
        var choice = System.Console.ReadLine()?.Trim().ToLower();
        
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
        ShowHeader();
        
        DisplayGameInfo(state);
        
        if (state.Status == GameStatus.Playing)
        {
            System.Console.WriteLine();
            System.Console.Write("Current Player: ");
            DisplayColoredPlayerChar(state.CurrentPlayer);
            System.Console.WriteLine();
        }
    }

    private static void DisplayColoredChar(int boardValue, int position)
    {
        if (boardValue == 0)
        {
            // Empty position - show position number in default color
            System.Console.Write(position.ToString()[0]);
        }
        else if (boardValue == 1)
        {
            // X player - blue color
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.Write('X');
            System.Console.ResetColor();
        }
        else
        {
            // O player - red color
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write('O');
            System.Console.ResetColor();
        }
    }

    private static void DisplayColoredPlayerChar(Player player)
    {
        if (player == Player.X)
        {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.Write('X');
            System.Console.ResetColor();
        }
        else
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write('O');
            System.Console.ResetColor();
        }
    }

    private static void DisplayGameInfo(GameState state)
    {
        System.Console.WriteLine($"Game ID: {state.GameId[..8]}..."); // Show first 8 chars
        System.Console.WriteLine($"Moves played: {state.MoveHistory.Count}");
        System.Console.WriteLine($"Started: {state.StartTime:HH:mm:ss}");
    }

    public int? GetPlayerMove(Player player, int[] board, GameState gameState)
    {
        System.Console.WriteLine();
        DisplayPositionsAndCurrentBoard(board);
        System.Console.WriteLine();
        
        System.Console.Write("Player ");
        DisplayColoredPlayerChar(player);
        System.Console.WriteLine("'s turn");

        while (true)
        {
            System.Console.Write("Enter position (0-8), 'h' for history, or 'q' to quit: ");
            var input = System.Console.ReadLine()?.Trim().ToLower();
            
            if (input == "q")
            {
                return null;
            }
            
            if (input == "h")
            {
                DisplayMoveHistory(gameState);
                System.Console.WriteLine();
                continue; // Stay in the loop to get another input
            }
            
            if (int.TryParse(input, out var position) && position >= 0 && position <= 8)
            {
                return position;
            }

            ShowError("Invalid input. Please enter a number between 0 and 8, 'h' for history, or 'q' to quit.");
        }
    }

    private static void DisplayMoveHistory(GameState state)
    {
        if (state.MoveHistory.Count == 0)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("No moves have been made yet.");
            System.Console.ResetColor();
            return;
        }

        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("Move History:");
        System.Console.ResetColor();
        
        for (int i = 0; i < state.MoveHistory.Count; i++)
        {
            var move = state.MoveHistory[i];
            System.Console.Write($"  {i + 1}. ");
            DisplayColoredPlayerChar(move.Player);
            System.Console.WriteLine($" → Position {move.Position} at {move.Timestamp:HH:mm:ss}");
        }
    }

    public void ShowError(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"Error: {message}");
        System.Console.ResetColor();
    }

    public void ShowGameResult(Player? winner)
    {
        System.Console.WriteLine();
        System.Console.WriteLine("═══════════════════════════════════════");
        
        if (winner.HasValue)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write("🎉 GAME OVER - Player ");
            System.Console.ResetColor();
            DisplayColoredPlayerChar(winner.Value);
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(" WINS! 🎉");
            System.Console.ResetColor();
        }
        else
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("🤝 GAME OVER - IT'S A TIE! 🤝");
            System.Console.ResetColor();
        }
        
        System.Console.WriteLine("═══════════════════════════════════════");
    }

    public void ShowGameRules()
    {
        System.Console.Clear();
        ShowHeader();
        
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
        System.Console.WriteLine("• Press 'h' during your turn to view move history");
        System.Console.WriteLine("• See game state and timing information");
        System.Console.WriteLine("• Position validation with clear error messages");
        
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("Press 'q' to return to main menu or any other key to wait...");
        System.Console.ResetColor();
        
        var key = System.Console.ReadKey(true);
        if (key.KeyChar == 'q' || key.KeyChar == 'Q')
        {
            return;
        }
        
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("Returning to main menu...");
        System.Console.ResetColor();
        System.Threading.Thread.Sleep(2000); // Give time to read the rules
    }

    public void ShowInvalidChoice()
    {
        ShowError("Invalid choice. Please select 1, 2, or q.");
    }

    public void ShowReturnToMenu()
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("Returning to main menu...");
        System.Console.ResetColor();
        System.Threading.Thread.Sleep(1500); // Brief pause to let players see the result
    }

    public static void ShowHeader()
    {
        System.Console.WriteLine("╔══════════════════════════════════════╗");
        System.Console.WriteLine("║          TIC TAC TOE GAME            ║");
        System.Console.WriteLine("╚══════════════════════════════════════╝");
        System.Console.WriteLine();
    }

    private static void DisplayPositionsAndCurrentBoard(int[] board)
    {
        System.Console.WriteLine("Positions:     Current:");
        
        // Row 1
        System.Console.Write("0 | 1 | 2      ");
        DisplayColoredChar(board[0], 0);
        System.Console.Write(" | ");
        DisplayColoredChar(board[1], 1);
        System.Console.Write(" | ");
        DisplayColoredChar(board[2], 2);
        System.Console.WriteLine();
        
        // Separator line
        System.Console.WriteLine("----------     ----------");
        
        // Row 2
        System.Console.Write("3 | 4 | 5      ");
        DisplayColoredChar(board[3], 3);
        System.Console.Write(" | ");
        DisplayColoredChar(board[4], 4);
        System.Console.Write(" | ");
        DisplayColoredChar(board[5], 5);
        System.Console.WriteLine();
        
        // Separator line
        System.Console.WriteLine("----------     ----------");
        
        // Row 3
        System.Console.Write("6 | 7 | 8      ");
        DisplayColoredChar(board[6], 6);
        System.Console.Write(" | ");
        DisplayColoredChar(board[7], 7);
        System.Console.Write(" | ");
        DisplayColoredChar(board[8], 8);
        System.Console.WriteLine();
    }
}

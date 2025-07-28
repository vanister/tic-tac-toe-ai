using TicTacToe.Engine;
using TicTacToe.Training;
using TicTacToe.Training.Players;
using TicTacToe.Training.Metrics;

namespace TicTacToe.Training.Console;

public class TrainingConsole
{
    private readonly TrainingLoop _trainingLoop;

    public TrainingConsole()
    {
        _trainingLoop = new TrainingLoop();
    }

    public void Run()
    {
        ShowWelcome();

        while (true)
        {
            var choice = ShowMainMenu();
            
            switch (choice.ToLower())
            {
                case "1":
                    RunRandomVsRandomTraining();
                    break;
                case "2":
                    ShowTrainingStats();
                    break;
                case "q":
                    return;
                default:
                    ShowInvalidChoice();
                    break;
            }
        }
    }

    private void ShowWelcome()
    {
        System.Console.Clear();
        System.Console.WriteLine("=================================");
        System.Console.WriteLine("  Tic Tac Toe AI Training System");
        System.Console.WriteLine("=================================");
        System.Console.WriteLine();
    }

    private string ShowMainMenu()
    {
        System.Console.WriteLine("Training Options:");
        System.Console.WriteLine("1. Run Random vs Random Training");
        System.Console.WriteLine("2. Show Last Training Stats");
        System.Console.WriteLine("q. Quit");
        System.Console.WriteLine();
        System.Console.Write("Choose option: ");
        
        return System.Console.ReadLine()?.Trim() ?? "";
    }

    private void RunRandomVsRandomTraining()
    {
        System.Console.WriteLine("\nStarting Random vs Random Training...");
        System.Console.Write("Number of games (default 1000): ");
        
        var input = System.Console.ReadLine()?.Trim();
        var numberOfGames = int.TryParse(input, out var games) ? games : 1000;

        var playerX = new RandomAiPlayer(seed: 42);
        var playerO = new RandomAiPlayer(seed: 123);

        System.Console.WriteLine($"\nRunning {numberOfGames} games...");
        System.Console.WriteLine("Progress: ");

        var progress = new Progress<int>(gamesCompleted =>
        {
            // For small numbers, report more frequently
            var reportInterval = Math.Min(100, Math.Max(1, numberOfGames / 10));
            
            if (gamesCompleted % reportInterval == 0 || gamesCompleted == numberOfGames)
            {
                var percentage = (double)gamesCompleted / numberOfGames * 100;
                System.Console.Write($"\r{gamesCompleted}/{numberOfGames} ({percentage:F1}%)");
            }
        });

        var startTime = DateTime.Now;
        var metrics = _trainingLoop.RunTraining(playerX, playerO, numberOfGames, progress);
        var endTime = DateTime.Now;

        System.Console.WriteLine("\n\nTraining Complete!");
        ShowMetrics(metrics);
        
        System.Console.WriteLine("\nPress any key to continue...");
        System.Console.ReadKey();
    }

    private void ShowTrainingStats()
    {
        var results = _trainingLoop.GetResults();
        
        if (results.Count == 0)
        {
            System.Console.WriteLine("\nNo training data available. Run training first.");
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
            return;
        }

        System.Console.WriteLine("\n=== Last Training Session ===");
        ShowDetailedStats(results);
        
        System.Console.WriteLine("\nPress any key to continue...");
        System.Console.ReadKey();
    }

    private void ShowMetrics(TrainingMetrics metrics)
    {
        System.Console.WriteLine("\n=== Training Results ===");
        System.Console.WriteLine($"Total Games: {metrics.TotalGames}");
        System.Console.WriteLine($"X Wins: {metrics.XWins} ({metrics.XWinRate:P1})");
        System.Console.WriteLine($"O Wins: {metrics.OWins} ({metrics.OWinRate:P1})");
        System.Console.WriteLine($"Draws: {metrics.Draws} ({metrics.DrawRate:P1})");
        System.Console.WriteLine($"Average Game Length: {metrics.AverageGameLength:F1} moves");
        System.Console.WriteLine($"Total Duration: {metrics.TotalDuration:mm\\:ss\\.ff}");
    }

    private void ShowDetailedStats(IReadOnlyList<GameResult> results)
    {
        var totalGames = results.Count;
        var xWins = results.Count(r => r.Winner == Player.X);
        var oWins = results.Count(r => r.Winner == Player.O);
        var draws = results.Count(r => r.Winner == null);
        
        System.Console.WriteLine($"Total Games: {totalGames}");
        System.Console.WriteLine($"X Wins: {xWins} ({(double)xWins/totalGames:P1})");
        System.Console.WriteLine($"O Wins: {oWins} ({(double)oWins/totalGames:P1})");
        System.Console.WriteLine($"Draws: {draws} ({(double)draws/totalGames:P1})");
        
        var averageLength = results.Average(r => r.MoveCount);
        var minLength = results.Min(r => r.MoveCount);
        var maxLength = results.Max(r => r.MoveCount);
        
        System.Console.WriteLine($"Game Length - Avg: {averageLength:F1}, Min: {minLength}, Max: {maxLength}");
        
        // Ask if user wants to see sample games
        System.Console.Write("\nShow sample games? (y/n): ");
        var showSamples = System.Console.ReadLine()?.Trim().ToLower();
        
        if (showSamples == "y" || showSamples == "yes")
        {
            System.Console.WriteLine("\nSample Games:");
            var sampleGames = results.Take(5);
            foreach (var game in sampleGames)
            {
                var result = game.Winner?.ToString() ?? "Draw";
                System.Console.WriteLine($"  Game {game.GameId[..8]}: {result} in {game.MoveCount} moves");
            }
        }
    }

    private void ShowInvalidChoice()
    {
        System.Console.WriteLine("Invalid choice. Please try again.");
        System.Console.WriteLine();
    }
}

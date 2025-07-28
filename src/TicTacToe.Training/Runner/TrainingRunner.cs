using TicTacToe.Training;
using TicTacToe.Training.Players;
using TicTacToe.Training.Services;

namespace TicTacToe.Training.Runner;

public class TrainingRunner
{
    public static async Task Run(string[] args)
    {
        System.Console.WriteLine("=== Tic Tac Toe AI Training Runner ===");
        System.Console.WriteLine();

        var trainingLoop = new TrainingLoop();
        
        // Create two random AI players with different seeds
        var playerX = new RandomAiPlayer(seed: 42);
        var playerO = new RandomAiPlayer(seed: 123);

        // Parse arguments
        var numberOfGames = 1000;
        var showSamples = false;
        string? outputFile = null;
        
        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            
            if (int.TryParse(arg, out var games))
            {
                numberOfGames = games;
            }
            else if (arg.Equals("--samples", StringComparison.OrdinalIgnoreCase) || 
                     arg.Equals("-s", StringComparison.OrdinalIgnoreCase))
            {
                showSamples = true;
            }
            else if (arg.Equals("--output", StringComparison.OrdinalIgnoreCase) || 
                     arg.Equals("-o", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 < args.Length)
                {
                    outputFile = args[i + 1];
                    i++; // Skip the next argument since we consumed it
                }
                else
                {
                    System.Console.WriteLine("Error: --output requires a file path");
                    return;
                }
            }
        }

        System.Console.WriteLine($"Running {numberOfGames} games: {playerX.Name} vs {playerO.Name}");
        System.Console.WriteLine("Progress: ");

        var progress = new Progress<int>(gamesCompleted =>
        {
            // For small numbers of games, report every completion
            // For larger numbers, report every 10%
            var reportInterval = Math.Max(1, numberOfGames / 10);
            
            if (gamesCompleted % reportInterval == 0 || gamesCompleted == numberOfGames)
            {
                var percentage = (double)gamesCompleted / numberOfGames * 100;
                System.Console.WriteLine($"{gamesCompleted}/{numberOfGames} ({percentage:F1}%)");
            }
        });

        var startTime = DateTime.Now;
        var metrics = trainingLoop.RunTraining(playerX, playerO, numberOfGames, progress);
        var endTime = DateTime.Now;

        System.Console.WriteLine();
        System.Console.WriteLine("=== Training Results ===");
        System.Console.WriteLine($"Total Games: {metrics.TotalGames}");
        System.Console.WriteLine($"X Wins: {metrics.XWins} ({metrics.XWinRate:P1})");
        System.Console.WriteLine($"O Wins: {metrics.OWins} ({metrics.OWinRate:P1})");
        System.Console.WriteLine($"Draws: {metrics.Draws} ({metrics.DrawRate:P1})");
        System.Console.WriteLine($"Average Game Length: {metrics.AverageGameLength:F1} moves");
        System.Console.WriteLine($"Total Duration: {metrics.TotalDuration:mm\\:ss\\.fff}");
        System.Console.WriteLine($"Games per second: {numberOfGames / metrics.TotalDuration.TotalSeconds:F0}");

        // Save training data if output file specified
        if (outputFile != null || numberOfGames >= 100) // Auto-save for larger runs
        {
            var filePath = outputFile ?? TrainingDataExporter.GenerateDefaultFilePath();
            try
            {
                await TrainingDataExporter.SaveToJsonLinesAsync(
                    filePath, 
                    metrics, 
                    trainingLoop.GetResults(),
                    playerX.Name,
                    playerO.Name);
                
                System.Console.WriteLine($"Training data saved to: {filePath}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Warning: Failed to save training data: {ex.Message}");
            }
        }

        if (showSamples)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("=== Sample Games ===");
            var results = trainingLoop.GetResults();
            var sampleGames = results.Take(5);
            foreach (var game in sampleGames)
            {
                var result = game.Winner?.ToString() ?? "Draw";
                System.Console.WriteLine($"Game {game.GameId[..8]}: {result} in {game.MoveCount} moves ({game.Duration.TotalMilliseconds:F0}ms)");
            }
        }

        System.Console.WriteLine();
        System.Console.WriteLine("Training run complete!");
    }
}

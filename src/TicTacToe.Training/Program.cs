using TicTacToe.Training.Console;
using TicTacToe.Training.Runner;

namespace TicTacToe.Training;

public class Program
{
    public static void Main(string[] args)
    {
        // If any arguments are provided, run the automated runner
        if (args.Length > 0)
        {
            TrainingRunner.Run(args);
        }
        else
        {
            // Otherwise run the interactive console
            var console = new TrainingConsole();
            console.Run();
        }
    }
}

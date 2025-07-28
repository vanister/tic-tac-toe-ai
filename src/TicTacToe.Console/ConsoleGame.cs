using TicTacToe.Engine;

namespace TicTacToe.Console;

public class ConsoleGame
{
    private readonly GameEngine _engine;
    private readonly ConsoleGameRenderer _renderer;

    public ConsoleGame()
    {
        _engine = new GameEngine();
        _renderer = new ConsoleGameRenderer();
    }

    public void Run()
    {
        ConsoleGameRenderer.ShowWelcome();

        while (true)
        {
            var choice = ConsoleGameRenderer.ShowMainMenu();
            
            switch (choice)
            {
                case "1":
                    PlayGame();
                    break;
                case "2":
                    ShowGameRules();
                    break;
                case "3":
                    return;
                default:
                    _renderer.ShowInvalidChoice();
                    break;
            }
        }
    }

    private void PlayGame()
    {
        var startingPlayer = ConsoleGameRenderer.GetStartingPlayer();
        if (startingPlayer == null)
        {
            return; // User cancelled
        }

        _engine.StartGame(startingPlayer.Value);
        
        while (!_engine.IsGameFinished())
        {
            _renderer.DisplayGameState(_engine.GetState());
            
            var currentPlayer = _engine.GetCurrentPlayer();
            var gameState = _engine.GetState();
            var position = _renderer.GetPlayerMove(currentPlayer, gameState.Board, gameState);
            
            if (position == null)
            {
                // User wants to quit the game
                break;
            }

            if (!_engine.TryMakeMove(currentPlayer, position.Value, out var error))
            {
                // Show error after clearing screen and redisplaying game state
                _renderer.DisplayGameState(_engine.GetState());
                ConsoleGameRenderer.ShowError(error!);
                System.Console.WriteLine(); // Add some spacing after error
            }
        }

        if (_engine.IsGameFinished())
        {
            _renderer.DisplayGameState(_engine.GetState());
            ConsoleGameRenderer.ShowGameResult(_engine.GetWinner());
            ConsoleGameRenderer.ShowReturnToMenu();
        }
    }

    private void ShowGameRules()
    {
        ConsoleGameRenderer.ShowGameRules();
    }
}

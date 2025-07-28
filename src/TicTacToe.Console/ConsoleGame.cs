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
            
            switch (choice.ToLower())
            {
                case "1":
                    PlayGame();
                    break;
                case "2":
                    ShowGameRules();
                    break;
                case "q":
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
            
            // Get player input and validate moves in a loop
            while (true)
            {
                var position = _renderer.GetPlayerMove(currentPlayer, gameState.Board, gameState);
                
                if (position == null)
                {
                    // User wants to quit the game
                    return;
                }

                // Validate the move with the engine
                if (_engine.IsValidMove(currentPlayer, position.Value, out var error))
                {
                    // Valid move - make it and break out of input loop
                    _engine.MakeMove(currentPlayer, position.Value);
                    break;
                }
                else
                {
                    // Invalid move - show error and get input again
                    ConsoleGameRenderer.ShowError(error!);
                    Thread.Sleep(1500); // Brief pause to show error
                }
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

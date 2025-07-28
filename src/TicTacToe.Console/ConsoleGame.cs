using TicTacToe.Engine;

namespace TicTacToe.Console;

public class ConsoleGame(IGameRenderer renderer)
{
    private readonly GameEngine _engine = new GameEngine();
    private readonly IGameRenderer _renderer = renderer;

    public ConsoleGame() : this(new ConsoleGameRenderer())
    {
    }

    public void Run()
    {
        _renderer.ShowWelcome();

        while (true)
        {
            var choice = _renderer.ShowMainMenu();
            
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
        var startingPlayer = _renderer.GetStartingPlayer();
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
                    _renderer.ShowError(error!);
                    _renderer.WaitForKeyPress();
                }
            }
        }

        if (_engine.IsGameFinished())
        {
            _renderer.DisplayGameState(_engine.GetState());
            _renderer.ShowGameResult(_engine.GetWinner());
            _renderer.ShowReturnToMenu();
        }
    }

    private void ShowGameRules()
    {
        _renderer.ShowGameRules();
    }
}

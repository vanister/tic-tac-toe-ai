using TicTacToe.Engine;

namespace TicTacToe.Console;

/// <summary>
/// A simple test renderer that can be used for automated testing or debugging
/// </summary>
public class TestRenderer : IGameRenderer
{
    private readonly Queue<string> _inputs;
    private readonly List<string> _outputs;

    public TestRenderer(params string[] inputs)
    {
        _inputs = new Queue<string>(inputs);
        _outputs = new List<string>();
    }

    public IReadOnlyList<string> Outputs => _outputs.AsReadOnly();

    public void ShowWelcome()
    {
        _outputs.Add("WELCOME");
    }

    public string ShowMainMenu()
    {
        _outputs.Add("MAIN_MENU");
        return _inputs.Count > 0 ? _inputs.Dequeue() : "q";
    }

    public Player? GetStartingPlayer()
    {
        _outputs.Add("GET_STARTING_PLAYER");
        var input = _inputs.Count > 0 ? _inputs.Dequeue() : "q";
        return input switch
        {
            "1" => Player.X,
            "2" => Player.O,
            _ => null
        };
    }

    public void DisplayGameState(GameState state)
    {
        _outputs.Add($"GAME_STATE: Moves={state.MoveHistory.Count}, Status={state.Status}");
    }

    public int? GetPlayerMove(Player player, int[] board, GameState gameState)
    {
        _outputs.Add($"GET_MOVE: Player={player}");
        var input = _inputs.Count > 0 ? _inputs.Dequeue() : "q";
        
        if (input == "q") return null;
        if (input == "h") 
        {
            _outputs.Add("SHOW_HISTORY");
            // For test purposes, just return the next input after showing history
            return _inputs.Count > 0 && int.TryParse(_inputs.Dequeue(), out var pos) ? pos : null;
        }
        
        return int.TryParse(input, out var position) ? position : null;
    }

    public void ShowError(string message)
    {
        _outputs.Add($"ERROR: {message}");
    }

    public void ShowGameResult(Player? winner)
    {
        _outputs.Add($"RESULT: Winner={winner?.ToString() ?? "Tie"}");
    }

    public void ShowGameRules()
    {
        _outputs.Add("SHOW_RULES");
    }

    public void ShowInvalidChoice()
    {
        _outputs.Add("INVALID_CHOICE");
    }

    public void ShowReturnToMenu()
    {
        _outputs.Add("RETURN_TO_MENU");
    }
}

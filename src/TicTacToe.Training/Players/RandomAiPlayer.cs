using TicTacToe.Engine;
using TicTacToe.Engine.Selectors;

namespace TicTacToe.Training.Players;

public class RandomAiPlayer : IAiPlayer
{
    private readonly Random _random;

    public string Name => "Random AI";

    public RandomAiPlayer()
    {
        _random = new Random();
    }

    public RandomAiPlayer(int seed)
    {
        _random = new Random(seed);
    }

    public int SelectMove(GameState gameState, Player player)
    {
        var availablePositions = GameSelectors.GetAvailablePositions(gameState);
        
        if (availablePositions.Count == 0)
        {
            throw new InvalidOperationException("No available moves");
        }

        var randomIndex = _random.Next(availablePositions.Count);
        return availablePositions[randomIndex];
    }
}

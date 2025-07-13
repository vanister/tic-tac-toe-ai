namespace TicTacToe.Engine;

public enum GameStatus
{
    Playing,
    XWins,
    OWins,
    Draw
}

public static class GameStatusExtensions
{
    public static bool IsFinished(this GameStatus status)
    {
        return status != GameStatus.Playing;
    }
    
    public static Player? GetWinner(this GameStatus status)
    {
        return status switch
        {
            GameStatus.XWins => Player.X,
            GameStatus.OWins => Player.O,
            _ => null
        };
    }
}

namespace TicTacToe.Engine;

public enum Player
{
    X = 1,
    O = -1
}

public static class PlayerExtensions
{
    public static Player Opponent(this Player player)
    {
        return player == Player.X ? Player.O : Player.X;
    }
    
    public static char ToChar(this Player player)
    {
        return player == Player.X ? 'X' : 'O';
    }
}

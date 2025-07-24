using TicTacToe.Engine.States;

namespace TicTacToe.Engine.Selectors;

public static class GameSelectors
{
    public static Player? GetWinner(GameState state)
    {
        return state.Status.GetWinner();
    }
    
    public static bool IsFinished(GameState state)
    {
        return state.Status.IsFinished();
    }
    
    public static int GetMoveCount(GameState state)
    {
        return state.MoveHistory.Count;
    }
    
    public static TimeSpan GetDuration(GameState state)
    {
        return DateTime.UtcNow - state.StartTime;
    }
    
    public static bool IsPositionEmpty(GameState state, int position)
    {
        if (position < 0 || position >= 9)
            return false;
            
        return state.Board[position] == 0;
    }
    
    public static IReadOnlyList<int> GetAvailablePositions(GameState state)
    {
        var available = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (state.Board[i] == 0)
                available.Add(i);
        }
        return available;
    }
    
    public static Player? GetPlayerAt(GameState state, int position)
    {
        if (position < 0 || position >= 9)
            return null;
            
        return state.Board[position] switch
        {
            1 => Player.X,
            -1 => Player.O,
            _ => null
        };
    }
    
    public static string FormatBoard(GameState state)
    {
        var chars = state.Board.Select(cell => cell switch
        {
            1 => 'X',
            -1 => 'O',
            _ => ' '
        }).ToArray();
        
        return $"""
               {chars[0]} | {chars[1]} | {chars[2]}
               ---------
               {chars[3]} | {chars[4]} | {chars[5]}
               ---------
               {chars[6]} | {chars[7]} | {chars[8]}
               """;
    }
}

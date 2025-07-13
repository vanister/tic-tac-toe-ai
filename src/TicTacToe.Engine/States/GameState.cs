namespace TicTacToe.Engine;

public record GameState(
    int[] Board,
    Player CurrentPlayer,
    GameStatus Status,
    IReadOnlyList<Move> MoveHistory,
    string GameId,
    DateTime StartTime)
{
    public static GameState CreateNew(Player startingPlayer, string? gameId = null)
    {
        return new GameState(
            Board: new int[9], // All zeros (empty)
            CurrentPlayer: startingPlayer,
            Status: GameStatus.Playing,
            MoveHistory: new List<Move>(),
            GameId: gameId ?? Guid.NewGuid().ToString(),
            StartTime: DateTime.UtcNow
        );
    }
    
    public Player? Winner => Status.GetWinner();
    
    public bool IsFinished => Status.IsFinished();
    
    public int MoveCount => MoveHistory.Count;
    
    public TimeSpan Duration => DateTime.UtcNow - StartTime;
    
    public bool IsPositionEmpty(int position)
    {
        if (position < 0 || position >= 9)
            return false;
            
        return Board[position] == 0;
    }
    
    public IReadOnlyList<int> GetAvailablePositions()
    {
        var available = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (Board[i] == 0)
                available.Add(i);
        }
        return available;
    }
    
    public Player? GetPlayerAt(int position)
    {
        if (position < 0 || position >= 9)
            return null;
            
        return Board[position] switch
        {
            1 => Player.X,
            -1 => Player.O,
            _ => null
        };
    }
    
    public override string ToString()
    {
        var chars = Board.Select(cell => cell switch
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

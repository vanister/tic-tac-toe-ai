namespace TicTacToe.Engine;

public record Move(
    Player Player, 
    int Position, 
    DateTime Timestamp)
{
    public static Move Create(Player player, int position)
    {
        return new Move(player, position, DateTime.UtcNow);
    }
    
    public (int Row, int Column) GetCoordinates()
    {
        return (Position / 3, Position % 3);
    }
    
    public override string ToString()
    {
        var (row, col) = GetCoordinates();
        var positionName = Position switch
        {
            4 => "center",
            0 or 2 or 6 or 8 => "corner",
            _ => "edge"
        };
        
        return $"{Player.ToChar()} at position {Position} ({positionName}, row {row + 1}, col {col + 1})";
    }
}

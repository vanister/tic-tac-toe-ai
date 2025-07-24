namespace TicTacToe.Engine.Actions;

public abstract record GameAction;

public record StartGameAction(Player StartingPlayer, string? GameId = null) : GameAction;

public record MakeMoveAction(Player Player, int Position) : GameAction;

public record ResetGameAction : GameAction;

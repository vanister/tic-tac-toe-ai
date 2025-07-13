namespace TicTacToe.Engine.States;

public static class GameUpdater
{
    private static readonly int[][] WinningCombinations = 
    [
        [0, 1, 2], [3, 4, 5], [6, 7, 8], // rows
        [0, 3, 6], [1, 4, 7], [2, 5, 8], // columns
        [0, 4, 8], [2, 4, 6]             // diagonals
    ];

    public static GameState StartGame(StartGameAction action)
    {
        return GameState.CreateNew(action.StartingPlayer, action.GameId);
    }

    public static GameState MakeMove(GameState state, MakeMoveAction action)
    {
        var newBoard = (int[])state.Board.Clone();
        newBoard[action.Position] = (int)action.Player;

        var newMove = Move.Create(action.Player, action.Position);
        var newMoveHistory = new List<Move>(state.MoveHistory) { newMove };

        var newStatus = DetermineGameStatus(newBoard);
        var newCurrentPlayer = newStatus == GameStatus.Playing ? action.Player.Opponent() : state.CurrentPlayer;

        return state with
        {
            Board = newBoard,
            CurrentPlayer = newCurrentPlayer,
            Status = newStatus,
            MoveHistory = newMoveHistory
        };
    }

    public static GameState ResetGame(GameState state)
    {
        return GameState.CreateNew(Player.X, state.GameId);
    }

    private static GameStatus DetermineGameStatus(int[] board)
    {
        foreach (var combination in WinningCombinations)
        {
            var sum = combination.Sum(pos => board[pos]);
            
            if (sum == 3)
            {
                return GameStatus.XWins;
            }
                
            if (sum == -3)
            {
                return GameStatus.OWins;
            }
        }

        if (board.All(cell => cell != 0))
        {
            return GameStatus.Draw;
        }
            
        return GameStatus.Playing;
    }
}

namespace Game.Checkers
{
    public enum Piece
    {
        None = 0,
        WhitePawn = 1,
        BlackPawn = 2,
        WhiteCrowned = 3,
        BlackCrowned = 4,
        Captured = 5,
    }
    public enum Player
    {
        White = 0,
        Black = 1,
    }

    public class CheckersState : IEquatable<CheckersState>
    {
        public static int BOARD_ROWS = 8;
        public static int BOARD_COLS = 8;
        //private static Dictionary<(int, int), List<(int, int)>> Neighbours = new Dictionary<(int, int), List<(int, int)>> {
        //    new KeyValuePair<(int, int),List<(int, int)>>((0,0), new List<(int, int)> { (1,1) }),
        //};
        private Piece[,] board;
        public Player CurrentPlayer { get; private set; }
        public CheckersState()
        {
            board = new Piece[,]
            {
                { Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.WhitePawn },
                { Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn },
                { Piece.BlackPawn, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn, Piece.None},
                { Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn },
            };

            CurrentPlayer = Player.White;
        }
        public CheckersState(Piece[,] board, Player currentPlayer)
        {
            this.board = board;
            CurrentPlayer = currentPlayer;
        }
        public CheckersState(CheckersState state)
        {
            board = new Piece[BOARD_COLS, BOARD_ROWS];
            Array.Copy(state.board, board, state.board.Length);
            CurrentPlayer = state.CurrentPlayer;
        }

        public Piece GetPieceAt((int x, int y) position)
        {
            return board[position.x, position.y];
        }

        public void SetPieceAt(int x, int y, Piece piece)
        {
            board[x, y] = piece;
        }

        public bool Equals(CheckersState? other)
        {
            if (other == null) return false;
            if (CurrentPlayer != other.CurrentPlayer) return false;
            for (int x = 0; x < BOARD_ROWS; x++)
            {
                for (int y = 0; y < BOARD_COLS; y++)
                {
                    if (GetPieceAt((x, y)) != other.GetPieceAt((x, y))) return false;
                }
            }
            return true;
        }
    }
}
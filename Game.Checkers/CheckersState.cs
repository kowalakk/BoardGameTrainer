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
        //private static Dictionary<(int, int), (int, int)[] Neighbours = new Dictionary<(int, int), List<(int, int)>> {
        //    new KeyValuePair<(int, int),List<(int, int)>>((0,0), new List<(int, int)> { (1,1) }),
        //};
        private Piece[,] board;
        public Player CurrentPlayer { get; private set; }
        private CheckersState(Piece[,] board, Player currentPlayer)
        {
            this.board = new Piece[BOARD_COLS, BOARD_ROWS];
            Array.Copy(board, this.board, board.Length);
            CurrentPlayer = currentPlayer;
        }
        public CheckersState(CheckersState state)
        {
            board = new Piece[BOARD_COLS, BOARD_ROWS];
            Array.Copy(state.board, board, state.board.Length);
            CurrentPlayer = state.CurrentPlayer;
        }
        public bool Equals(CheckersState? other)
        {
            if (other == null) return false;
            if (CurrentPlayer != other.CurrentPlayer) return false;
            for (int x = 0; x < BOARD_ROWS; x++)
            {
                for (int y = 0; y < BOARD_COLS; y++)
                {
                    if (GetPieceAt(x, y) != other.GetPieceAt(x, y)) return false;
                }
            }
            return true;
        }
        public IEnumerable<Field> GetFields()
        {
            for (int col = 0; col < BOARD_ROWS; col++)
            {
                for (int row = col % 2; row < BOARD_COLS; row += 2)
                {
                    yield return new Field(col, row);
                }
            }
        }
        public IEnumerable<Field> GetNeighbours(Field field)
        {
            int x = field.Col;
            int y = field.Row;
            List<Field> neighbours = new();
            if (y < BOARD_ROWS - 1)
            {
                if (x < BOARD_COLS - 1)
                    neighbours.Add(new Field(x + 1, y + 1));
                if (x > 0)
                    neighbours.Add(new Field(x - 1, y + 1));
            }
            if (y > 0)
            {
                if (x < BOARD_COLS - 1)
                    neighbours.Add(new Field(x + 1, y - 1));
                if (x > 0)
                    neighbours.Add(new Field(x - 1, y - 1));
            }
            return neighbours;
        }

        public Piece GetPieceAt(int col, int row)
        {
            return board[col, row];
        }

        public Piece GetPieceAt(Field field)
        {
            return GetPieceAt(field.Col, field.Row);
        }

        public void SetPieceAt(int col, int row, Piece piece)
        {
            board[col, row] = piece;
        }
        public void SetPieceAt(Field field, Piece piece)
        {
            SetPieceAt(field.Col, field.Row, piece);
        }
        public void SetPieceAt(Field field, Piece piece, bool possiblePromotion)
        {
            if (possiblePromotion)
            {
                if (piece == Piece.WhitePawn && field.Row == BOARD_ROWS - 1)
                {
                    SetPieceAt(field.Col, field.Row, Piece.WhiteCrowned);
                    return;
                }
                if (piece == Piece.BlackPawn && field.Row == 0)
                {
                    SetPieceAt(field.Col, field.Row, Piece.BlackCrowned);
                    return;
                }
            }
            SetPieceAt(field.Col, field.Row, piece);
        }
        public void SetPieceAt(string field, Piece piece)
        {
            int col = field[0] - 'A';
            int row = field[1] - '1';
            SetPieceAt(col, row, piece);
        }
        public static CheckersState GetInitialState()
        {
            Piece[,] board = new Piece[,]
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

            return new CheckersState(board, Player.White);
        }
        public static CheckersState GetEmptyBoardState(Player player = Player.White)
        {
            Piece[,] emptyBoard = new Piece[,]
            {
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
            };

            return new CheckersState(emptyBoard, player);
        }

    }
}
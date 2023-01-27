using Game.IGame;

namespace Game.Checkers
{
    public enum Piece
    {
        None = 0,
        WhitePawn = Player.One,
        BlackPawn = Player.Two,
        WhiteCrowned = Player.One + 2,
        BlackCrowned = Player.Two + 2,
        CapturedPiece = 5,
    }

    public class CheckersState : IEquatable<CheckersState>
    {
        public static int boardSize = 8;
        //private static Dictionary<(int, int), (int, int)[] Neighbours = new Dictionary<(int, int), List<(int, int)>> {
        //    new KeyValuePair<(int, int),List<(int, int)>>((0,0), new List<(int, int)> { (1,1) }),
        //};
        private readonly Piece[,] board;

        public Player CurrentPlayer { get; set; }

        public ICheckersAction? LastAction { get; set; } // for drawing purposes

        private CheckersState(Piece[,] board, Player currentPlayer, ICheckersAction? lastAction = null)
        {
            this.board = new Piece[boardSize, boardSize];
            Array.Copy(board, this.board, board.Length);
            CurrentPlayer = currentPlayer;
            LastAction = lastAction;
        }

        public CheckersState(CheckersState state)
        {
            board = new Piece[boardSize, boardSize];
            Array.Copy(state.board, board, state.board.Length);
            CurrentPlayer = state.CurrentPlayer;
            LastAction = state.LastAction;
        }

        public bool Equals(CheckersState? other)
        {
            if (other == null) return false;
            if (CurrentPlayer != other.CurrentPlayer) return false;
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (GetPieceAt(x, y) != other.GetPieceAt(x, y)) return false;
                }
            }
            return true;
        }
        public IEnumerable<Field> GetFields()
        {
            for (int col = 0; col < boardSize; col++)
            {
                for (int row = col % 2; row < boardSize; row += 2)
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
            if (y < boardSize - 1)
            {
                if (x < boardSize - 1)
                    neighbours.Add(new Field(x + 1, y + 1));
                if (x > 0)
                    neighbours.Add(new Field(x - 1, y + 1));
            }
            if (y > 0)
            {
                if (x < boardSize - 1)
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
        public void SetPieceAtWithPossiblePromotion(Field field, Piece piece)
        {

            if (piece == Piece.WhitePawn && field.Row == boardSize - 1)
            {
                SetPieceAt(field.Col, field.Row, Piece.WhiteCrowned);
                return;
            }
            if (piece == Piece.BlackPawn && field.Row == 0)
            {
                SetPieceAt(field.Col, field.Row, Piece.BlackCrowned);
                return;
            }
            SetPieceAt(field.Col, field.Row, piece);
        }
        /// <summary>
        /// Method for writing tests only
        /// </summary>
        /// <param name="field">Field is described by two-letter string (A-H)(1-8)</param>
        /// <param name="piece"></param>
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
                { Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn },
                { Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn },
                { Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn },
                { Piece.WhitePawn, Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None, Piece.BlackPawn },
            };

            return new CheckersState(board, Player.One);
        }
        public static CheckersState GetEmptyBoardState(Player player = Player.One)
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
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
        public static int BOARD_SIZE = 8;
        //private static Dictionary<(int, int), (int, int)[] Neighbours = new Dictionary<(int, int), List<(int, int)>> {
        //    new KeyValuePair<(int, int),List<(int, int)>>((0,0), new List<(int, int)> { (1,1) }),
        //};
        private readonly Piece[,] board;
        public Player CurrentPlayer { get; set; }

        private CheckersState(Piece[,] board, Player currentPlayer)
        {
            this.board = new Piece[BOARD_SIZE, BOARD_SIZE];
            Array.Copy(board, this.board, board.Length);
            CurrentPlayer = currentPlayer;
        }

        public CheckersState(CheckersState state)
        {
            board = new Piece[BOARD_SIZE, BOARD_SIZE];
            Array.Copy(state.board, board, state.board.Length);
            CurrentPlayer = state.CurrentPlayer;
        }

        public bool Equals(CheckersState? other)
        {
            if (other == null) return false;
            if (CurrentPlayer != other.CurrentPlayer) return false;
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    if (GetPieceAt(x, y) != other.GetPieceAt(x, y)) return false;
                }
            }
            return true;
        }
        public IEnumerable<Field> GetFields()
        {
            for (int col = 0; col < BOARD_SIZE; col++)
            {
                for (int row = col % 2; row < BOARD_SIZE; row += 2)
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
            if (y < BOARD_SIZE - 1)
            {
                if (x < BOARD_SIZE - 1)
                    neighbours.Add(new Field(x + 1, y + 1));
                if (x > 0)
                    neighbours.Add(new Field(x - 1, y + 1));
            }
            if (y > 0)
            {
                if (x < BOARD_SIZE - 1)
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

            if (piece == Piece.WhitePawn && field.Row == BOARD_SIZE - 1)
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
                { Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn },
                { Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn },
                { Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn },
                { Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn, Piece.None },
                { Piece.None, Piece.WhitePawn, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.BlackPawn },
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
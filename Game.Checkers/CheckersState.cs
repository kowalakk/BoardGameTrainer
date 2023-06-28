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
        public static int BoardSize { get; } = 8;
        public static int FieldCount { get; } = BoardSize * BoardSize / 2;
        public static int InsignificantActionsToDraw { get; } = 15;
        public static Func<int, int?>[] Neighbours { get; } = new Func<int, int?>[]
        {
            field => field > 4 && (field - 4) % 8 != 0 ? field - 4 - (field / 4) % 2 : null, // upLeft
            field => field > 3 && (field - 3) % 8 != 0 ? field - 3 - (field / 4) % 2 : null, // upRight
            field => field < 28 && (field - 4) % 8 != 0 ? field + 4 - (field / 4) % 2 : null, // downLeft
            field => field < 27 && (field - 3) % 8 != 0 ? field + 5 - (field / 4) % 2 : null, // downRight
        };
        public Player CurrentPlayer { get; set; }
        public Player CurrentOpponent => CurrentPlayer == Player.One ? Player.Two : Player.One;
        public int InsignificantActions { get; set; }
        public ICheckersAction? LastAction { get; set; } // for drawing purposes

        private Piece[] Board { get; }

        private CheckersState(Piece[] board, Player currentPlayer, ICheckersAction? lastAction = null, int insignificantActions = 0)
        {
            Board = (Piece[])board.Clone();
            CurrentPlayer = currentPlayer;
            LastAction = lastAction;
            InsignificantActions = insignificantActions;
        }

        public CheckersState(CheckersState state)
        {
            Board = (Piece[])state.Board.Clone();
            CurrentPlayer = state.CurrentPlayer;
            LastAction = state.LastAction;
            InsignificantActions = state.InsignificantActions;
        }

        public Piece GetPieceAt(int index)
        {
            return Board[index];
        }

        public void SetPieceAt(int index, Piece piece)
        {
            Board[index] = piece;
        }

        public void SetPieceAtWithPossiblePromotion(int index, Piece piece)
        {

            if (piece == Piece.WhitePawn && index < BoardSize / 2)
            {
                SetPieceAt(index, Piece.WhiteCrowned);
                return;
            }
            if (piece == Piece.BlackPawn && index >= FieldCount - BoardSize / 2)
            {
                SetPieceAt(index, Piece.BlackCrowned);
                return;
            }
            SetPieceAt(index, piece);
        }
        public static CheckersState GetInitialState()
        {
            Piece[] board = new Piece[32];
            for (int i = 0; i < 12; i++)
                board[i] = Piece.BlackPawn;
            for (int i = 12; i < 20; i++)
                board[i] = Piece.None;
            for (int i = 20; i < 32; i++)
                board[i] = Piece.WhitePawn;

            return new CheckersState(board, Player.One);
        }

        public static CheckersState GetEmptyBoardState(Player player = Player.One)
        {
            Piece[] emptyBoard = new Piece[FieldCount];
            for (int i = 0; i < emptyBoard.Length; i++)
                emptyBoard[i] = Piece.None;

            return new CheckersState(emptyBoard, player);
        }

        public bool Equals(CheckersState? other)
        {
            if (other == null) return false;
            if (CurrentPlayer != other.CurrentPlayer) return false;
            return Board.SequenceEqual(other.Board);
        }
    }
}
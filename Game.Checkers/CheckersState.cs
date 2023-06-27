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

        public static int?[][] Neighbours { get; } = new int?[][]
        {
            new int?[] { null, null,    5,    4 },
            new int?[] { null, null,    6,    5 },
            new int?[] { null, null,    7,    6 },
            new int?[] { null, null, null,    7 },

            new int?[] { null,    0,    8, null },
            new int?[] {    0,    1,    9,    8 },
            new int?[] {    1,    2,   10,    9 },
            new int?[] {    2,    3,   11,   10 },

            new int?[] {    4,    5,   13,   12 },
            new int?[] {    5,    6,   14,   13 },
            new int?[] {    6,    7,   15,   14 },
            new int?[] {    7, null, null,   15 },

            new int?[] { null,    8,   16, null },
            new int?[] {    8,    9,   17,   16 },
            new int?[] {    9,   10,   18,   17 },
            new int?[] {   10,   11,   19,   18 },

            new int?[] {   12,   13,   21,   20 },
            new int?[] {   13,   14,   22,   21 },
            new int?[] {   14,   15,   23,   22 },
            new int?[] {   15, null, null,   23 },

            new int?[] { null,   16,   24, null },
            new int?[] {   16,   17,   25,   24 },
            new int?[] {   17,   18,   26,   25 },
            new int?[] {   18,   19,   27,   26 },

            new int?[] {   20,   21,   29,   28 },
            new int?[] {   21,   22,   30,   29 },
            new int?[] {   22,   23,   31,   30 },
            new int?[] {   23, null, null,   31 },

            new int?[] { null,   24, null, null },
            new int?[] {   24,   25, null, null },
            new int?[] {   25,   26, null, null },
            new int?[] {   26,   27, null, null },
        };
        public Player CurrentPlayer { get; set; }
        public Player CurrentOpponent => CurrentPlayer == Player.One? Player.Two : Player.One;
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
            Piece[] board = new Piece[]
            {
                Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn,
                Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn,
                Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn,
                Piece.None,      Piece.None,      Piece.None,      Piece.None,
                Piece.None,      Piece.None,      Piece.None,      Piece.None,
                Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn,
                Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn,
                Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn
            };

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
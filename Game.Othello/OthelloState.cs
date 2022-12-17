namespace Game.Othello
{
    public class OthelloState : IEquatable<OthelloState>
    {
        private const int boardSize = 8;
        private const int maxHandSize = 30;
        public enum Field {White, Black, Empty}
        public Field[,] board { get; set; }
        public int WhiteHandCount { get; set; }
        public int BlackHandCount { get; set; }
        public bool BlacksTurn { get; set; }
        public OthelloState(Field[,] board, int whiteHandCount, int blackHandCount, bool blacksTurn)
        {
            this.board = board;
            WhiteHandCount = whiteHandCount;
            BlackHandCount = blackHandCount;
            BlacksTurn = blacksTurn;
        }

        public bool Equals(OthelloState? other)
        {
            if (other == null)
                return false;
            if (this.WhiteHandCount != other.WhiteHandCount)
                return false;
            if (this.BlackHandCount != other.BlackHandCount)
                return false;
            if (this.BlacksTurn != other.BlacksTurn)
                return false;
            for (int i = 0; i < boardSize; i++)
                for(int j= 0; j < boardSize; j++)
                    if (this.board[i, j] != other.board[i, j])
                        return false;
            return true;
        }

        public static OthelloState GenerateInitialOthelloState()
        {
            Field[,] board = new Field[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    board[i, j] = Field.Empty;
            board[3, 3] = board[4, 4] = Field.White;
            board[3, 4] = board[4, 3] = Field.Black;

            return new OthelloState(board, 30, 30, true);
        }
    }
}
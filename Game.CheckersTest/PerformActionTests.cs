namespace Game.Checkers.Tests
{
    public class PerformActionTests
    {
        readonly Checkers checkers = new();
        [Fact]
        public void StateAfterMoveActionIsCorrect()
        {
            CheckersState actual = CheckersState.GetEmptyBoardState();
            actual.SetPieceAt(17, Piece.WhitePawn);
            MoveAction action = new(17, 14);
            actual = checkers.PerformAction(action, actual);
            CheckersState expected = CheckersState.GetEmptyBoardState(IGame.Player.Two);
            expected.SetPieceAt(14, Piece.WhitePawn);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void StateAfterCaptureActionIsCorrect()
        {
            CheckersState actual = CheckersState.GetEmptyBoardState();
            actual.SetPieceAt(17, Piece.WhitePawn);
            actual.SetPieceAt(14, Piece.BlackPawn);
            CaptureAction action = new(17, 14, 10);
            actual = checkers.PerformAction(action, actual);
            CheckersState expected = CheckersState.GetEmptyBoardState(IGame.Player.Two);
            expected.SetPieceAt(10, Piece.WhitePawn);
            Assert.Equal(expected, actual);
        }
    }
}

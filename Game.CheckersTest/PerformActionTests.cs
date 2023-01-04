namespace Game.Checkers.Tests
{
    public class PerformActionTests
    {
        readonly Checkers checkers = new();
        [Fact]
        public void StateAfterMoveActionIsCorrect()
        {
            CheckersState actual = CheckersState.GetEmptyBoardState();
            actual.SetPieceAt("B4", Piece.WhitePawn);
            MoveAction action = new(new("B4"), new("C5"));
            actual = checkers.PerformAction(action, actual);
            CheckersState expected = CheckersState.GetEmptyBoardState(IGame.Player.PlayerTwo);
            expected.SetPieceAt("C5", Piece.WhitePawn);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void StateAfterCaptureActionIsCorrect()
        {
            CheckersState actual = CheckersState.GetEmptyBoardState();
            actual.SetPieceAt("B4", Piece.WhitePawn);
            actual.SetPieceAt("C5", Piece.BlackPawn);
            CaptureAction action = new(new("B4"), new("C5"),new("D6"));
            actual = checkers.PerformAction(action, actual);
            CheckersState expected = CheckersState.GetEmptyBoardState(IGame.Player.PlayerTwo);
            expected.SetPieceAt("D6", Piece.WhitePawn);
            Assert.Equal(expected, actual);
        }
    }
}

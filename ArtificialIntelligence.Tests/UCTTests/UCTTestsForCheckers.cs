using Game.Checkers;


namespace Ai.Tests.UCT
{
    public class UCTTestsForCheckers
    {
        private readonly Uct<ICheckersAction, CheckersState, ICheckersInputState> uct = new(1.414, new Checkers(), new IterationStopCondition(10));
        [Fact]
        public void MoveAssessmentReturns2WinningMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(16, Piece.WhitePawn);
            state.SetPieceAt(17, Piece.WhitePawn);
            state.SetPieceAt(13, Piece.BlackPawn);
            var assesments = uct.MoveAssessment(state);
            Assert.Equal(2, assesments.Count);
            CaptureAction expected = new(16, 13, 9);
            Assert.Contains((expected, 1), assesments);
            expected = new(17, 13, 8);
            Assert.Contains((expected, 1), assesments);
        }
        [Fact]
        public void MoveAssessmentReturns1WinningAnd1LosingMove()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(28, Piece.WhitePawn);
            state.SetPieceAt(17, Piece.BlackCrowned);
            state.SetPieceAt(14, Piece.WhitePawn);
            state.SetPieceAt(10, Piece.BlackCrowned);
            var assesments = uct.MoveAssessment(state);
            Assert.Equal(2, assesments.Count);
            CaptureAction expected = new(14, 17, 21);
            Assert.Contains((expected, 1), assesments);
            expected = new(14, 10, 7);
            Assert.Contains((expected, -1), assesments);
        }
    }
}

using Game.Checkers;


namespace ArtificialIntelligence.Tests.UCT
{
    public class MoveAssesmentForCheckers
    {
        private readonly UCT<CheckersAction, CheckersState, CheckersInputState> uct = new(1.414, new Checkers(), new IterationStopCondition(10));
        [Fact]
        public void MoveAssessmentReturns2WinningMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("B4", Piece.WhitePawn);
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("C5", Piece.BlackPawn);
            var assesments = uct.MoveAssessment(state);
            Assert.Equal(2, assesments.Count);
            CaptureAction expected = new(new("B4"),new("C5"),new("D6"));
            Assert.Contains((expected, 1), assesments);
            expected = new(new("D4"), new("C5"), new("B6"));
            Assert.Contains((expected, 1), assesments);
        }
        [Fact]
        public void MoveAssessmentReturns1WinningAnd1LosingMove()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("A1", Piece.WhitePawn);
            state.SetPieceAt("D4", Piece.BlackCrowned);
            state.SetPieceAt("E5", Piece.WhitePawn);
            state.SetPieceAt("F6", Piece.BlackCrowned);
            var assesments = uct.MoveAssessment(state);
            Assert.Equal(2, assesments.Count);
            CaptureAction expected = new(new("E5"), new("D4"), new("C3"));
            Assert.Contains((expected, 1), assesments);
            expected = new(new("E5"), new("F6"), new("G7"));
            Assert.Contains((expected, -1), assesments);
        }
    }
}

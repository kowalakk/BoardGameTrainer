using Game.Checkers;


namespace ArtificialIntelligence.Tests.UCT
{
    public class MoveAssesmentForCheckers
    {
        private UCT<CheckersAction, CheckersState, CheckersInputState> uct = new UCT<CheckersAction, CheckersState, CheckersInputState>(1.414, new Checkers(), new IterationStopCondition(10));
        [Fact]
        public void MoveAssessmentReturns2Moves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("B4", Piece.WhitePawn);
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("C5", Piece.BlackPawn);
            var assesments = uct.MoveAssessment(state);
            Assert.Equal(2, assesments.Count);
        }
    }
}

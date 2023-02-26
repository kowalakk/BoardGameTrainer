namespace Game.Checkers.Tests
{
    public class CrownedTests
    {
        readonly Checkers checkers = new();
        [Fact]
        public void PossibleActionsForBlockedCrownedShouldBeEmpty()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(28, Piece.WhiteCrowned);
            state.SetPieceAt(24, Piece.BlackPawn);
            state.SetPieceAt(21, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Empty(possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeMovesToAllOtherFieldsOnDiag()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(28, Piece.WhiteCrowned);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(7, possibleActions.Count());
            Assert.Contains(new MoveAction(28, 24), possibleActions);
            Assert.Contains(new MoveAction(28, 21), possibleActions);
            Assert.Contains(new MoveAction(28, 17), possibleActions);
            Assert.Contains(new MoveAction(28, 14), possibleActions);
            Assert.Contains(new MoveAction(28, 10), possibleActions);
            Assert.Contains(new MoveAction(28, 7), possibleActions);
            Assert.Contains(new MoveAction(28, 3), possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeMovesToAllOtherFieldsOnBothDiags()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(21, Piece.WhiteCrowned);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(11, possibleActions.Count());
            Assert.Contains(new MoveAction(21, 28), possibleActions);
            Assert.Contains(new MoveAction(21, 24), possibleActions);
            Assert.Contains(new MoveAction(21, 17), possibleActions);
            Assert.Contains(new MoveAction(21, 14), possibleActions);
            Assert.Contains(new MoveAction(21, 10), possibleActions);
            Assert.Contains(new MoveAction(21, 7), possibleActions);
            Assert.Contains(new MoveAction(21, 3), possibleActions);

            Assert.Contains(new MoveAction(21, 12), possibleActions);
            Assert.Contains(new MoveAction(21, 16), possibleActions);
            Assert.Contains(new MoveAction(21, 25), possibleActions);
            Assert.Contains(new MoveAction(21, 30), possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeASingleCapture()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(21, Piece.WhiteCrowned);
            state.SetPieceAt(24, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(21, 24, 28);
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoCapturesWithDifferentFinnishFields()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(17, Piece.WhiteCrowned);
            state.SetPieceAt(22, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            Assert.Contains(new CaptureAction(17, 22, 26), possibleActions);
            Assert.Contains(new CaptureAction(17, 22, 31), possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoCapturesFromDistanceWithDifferentFinnishFields()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(8, Piece.WhiteCrowned);
            state.SetPieceAt(22, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            Assert.Contains(new CaptureAction(8, 22, 26), possibleActions);
            Assert.Contains(new CaptureAction(8, 22, 31), possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeLongestCaptureOnly()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(8, Piece.WhiteCrowned);
            state.SetPieceAt(22, Piece.BlackPawn);
            state.SetPieceAt(23, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(26, 23, 19);
            action.CombineCapture(8, 22);
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void AlreadyCapturedPieceShouldBeUncapturableButNotRemovedTillEndOfMove()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(26, Piece.WhiteCrowned);
            state.SetPieceAt(22, Piece.BlackPawn);
            state.SetPieceAt(16, Piece.BlackCrowned);
            state.SetPieceAt(24, Piece.BlackCrowned);
            state.SetPieceAt(15, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(20, 24, 29);
            action.CombineCapture(13, 16);
            action.CombineCapture(26, 22);
            Assert.Contains(action, possibleActions);
        }
    }
}

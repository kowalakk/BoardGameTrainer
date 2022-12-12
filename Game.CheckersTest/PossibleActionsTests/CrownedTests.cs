namespace Game.Checkers.Test
{
    public class CrownedTests
    {
        readonly Checkers checkers = new();
        [Fact]
        public void PossibleActionsForBlockedCrownedShouldBeEmpty()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("A1", Piece.WhiteCrowned);
            state.SetPieceAt("B2", Piece.BlackPawn);
            state.SetPieceAt("C3", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Empty(possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeMovesToAllOtherFieldsOnDiag()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("A1", Piece.WhiteCrowned);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(7, possibleActions.Count());
            Assert.Contains(new MoveAction(new Field("A1"), new Field("B2")), possibleActions);
            Assert.Contains(new MoveAction(new Field("A1"), new Field("C3")), possibleActions);
            Assert.Contains(new MoveAction(new Field("A1"), new Field("D4")), possibleActions);
            Assert.Contains(new MoveAction(new Field("A1"), new Field("E5")), possibleActions);
            Assert.Contains(new MoveAction(new Field("A1"), new Field("F6")), possibleActions);
            Assert.Contains(new MoveAction(new Field("A1"), new Field("G7")), possibleActions);
            Assert.Contains(new MoveAction(new Field("A1"), new Field("H8")), possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeMovesToAllOtherFieldsOnBothDiags()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("C3", Piece.WhiteCrowned);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(11, possibleActions.Count());
            Assert.Contains(new MoveAction(new Field("C3"), new Field("A1")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("B2")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("D4")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("E5")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("F6")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("G7")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("H8")), possibleActions);

            Assert.Contains(new MoveAction(new Field("C3"), new Field("A5")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("B4")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("D2")), possibleActions);
            Assert.Contains(new MoveAction(new Field("C3"), new Field("E1")), possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeASingleCapture()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("C3", Piece.WhiteCrowned);
            state.SetPieceAt("B2", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(new Field("C3"), new Field("B2"), new Field("A1"));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoCapturesWithDifferentFinnishFields()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("D4", Piece.WhiteCrowned);
            state.SetPieceAt("E3", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            Assert.Contains(new CaptureAction(new Field("D4"), new Field("E3"), new Field("F2")), possibleActions);
            Assert.Contains(new CaptureAction(new Field("D4"), new Field("E3"), new Field("G1")), possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoCapturesFromDistanceWithDifferentFinnishFields()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("B6", Piece.WhiteCrowned);
            state.SetPieceAt("E3", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            Assert.Contains(new CaptureAction(new Field("B6"), new Field("E3"), new Field("F2")), possibleActions);
            Assert.Contains(new CaptureAction(new Field("B6"), new Field("E3"), new Field("G1")), possibleActions);
        }
    }
}

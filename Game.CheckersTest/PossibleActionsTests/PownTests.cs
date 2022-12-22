namespace Game.Checkers.Test
{
    public class PownsTests
    {
        readonly Checkers checkers = new();

        [Fact]
        public void PossibleActionsShouldBeSingleMove()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("A1", Piece.WhitePawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            MoveAction action = new(new Field("A1"), new Field("B2"));
            Assert.Equal(action, possibleActions.First());
        }
        [Fact]
        public void PossibleActionsForBlockedPownShouldBeEmpty()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("A1", Piece.WhitePawn);
            state.SetPieceAt("B2", Piece.BlackPawn);
            state.SetPieceAt("C3", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Empty(possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("C1", Piece.WhitePawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            MoveAction action = new(new Field("C1"), new Field("B2"));
            Assert.Contains(action, possibleActions);
            action = new(new Field("C1"), new Field("D2"));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeSingleCapture()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("C5", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(new Field("D4"), new Field("C5"), new Field("B6"));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoCaptures()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("C5", Piece.BlackPawn);
            state.SetPieceAt("E3", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            CaptureAction action = new(new Field("D4"), new Field("C5"), new Field("B6"));
            Assert.Contains(action, possibleActions);
            action = new(new Field("D4"), new Field("E3"), new Field("F2"));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeDoubleCaptureOnly()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("C5", Piece.BlackPawn); 
            state.SetPieceAt("E5", Piece.BlackPawn);
            state.SetPieceAt("G5", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(new Field("F6"), new Field("G5"), new Field("H4"));
            action.CombineCapture(new Field("D4"), new Field("E5"));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void AlreadyCapturedPieceShouldBeUncapturableButNotRemovedTillEndOfMove()
        {
            CheckersState state = CheckersState.GetEmptyBoardState(IGame.Player.PlayerTwo);
            state.SetPieceAt("B2", Piece.WhitePawn);
            state.SetPieceAt("B4", Piece.WhitePawn);
            state.SetPieceAt("D2", Piece.WhitePawn);
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("E3", Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            CaptureAction action = new(new Field("C1"), new Field("D2"), new Field("E3"));
            action.CombineCapture(new Field("A3"), new Field("B2"));
            action.CombineCapture(new Field("C5"), new Field("B4"));
            action.CombineCapture(new Field("E3"), new Field("D4"));
            Assert.Contains(action, possibleActions);
            action = new(new Field("C5"), new Field("D4"), new Field("E3"));
            action.CombineCapture(new Field("A3"), new Field("B4"));
            action.CombineCapture(new Field("C1"), new Field("B2"));
            action.CombineCapture(new Field("E3"), new Field("D2"));
            Assert.Contains(action, possibleActions);
        }
    }
}

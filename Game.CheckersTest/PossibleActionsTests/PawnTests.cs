namespace Game.Checkers.Tests
{
    public class PawnTests
    {
        readonly Checkers checkers = new();

        [Fact]
        public void PossibleActionsShouldBeSingleMove()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(28, Piece.WhitePawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            MoveAction action = new(28, 24);
            Assert.Equal(action, possibleActions.First());
        }
        [Fact]
        public void PossibleActionsForBlockedPawnShouldBeEmpty()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(28, Piece.WhitePawn);
            state.SetPieceAt(24, Piece.BlackPawn);
            state.SetPieceAt(21, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Empty(possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(30, Piece.WhitePawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            MoveAction action = new(30, 25);
            Assert.Contains(action, possibleActions);
            action = new(30, 26);
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeSingleCapture()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(18, Piece.WhitePawn);
            state.SetPieceAt(14, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(18, 14, 9);
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeTwoCaptures()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(18, Piece.WhitePawn);
            state.SetPieceAt(14, Piece.BlackPawn);
            state.SetPieceAt(23, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            CaptureAction action = new(18, 14, 9);
            Assert.Contains(action, possibleActions);
            action = new(18, 23, 27);
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsShouldBeDoubleCaptureOnly()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(17, Piece.WhitePawn);
            state.SetPieceAt(13, Piece.BlackPawn); 
            state.SetPieceAt(14, Piece.BlackPawn);
            state.SetPieceAt(15, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(10, 15, 19);
            action.CombineCapture(17, 14);
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void AlreadyCapturedPieceShouldBeUncapturableButNotRemovedTillEndOfMove()
        {
            CheckersState state = CheckersState.GetEmptyBoardState(IGame.Player.Two);
            state.SetPieceAt(25, Piece.WhitePawn);
            state.SetPieceAt(17, Piece.WhitePawn);
            state.SetPieceAt(26, Piece.WhitePawn);
            state.SetPieceAt(18, Piece.WhitePawn);
            state.SetPieceAt(23, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            CaptureAction action = new(30, 26, 23);
            action.CombineCapture(21, 25);
            action.CombineCapture(14, 17);
            action.CombineCapture(23, 18);
            Assert.Contains(action, possibleActions);
            action = new(14, 18, 23);
            action.CombineCapture(21, 17);
            action.CombineCapture(30, 25);
            action.CombineCapture(23, 26);
            Assert.Contains(action, possibleActions);
        }
    }
}

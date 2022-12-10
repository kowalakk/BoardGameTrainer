namespace Game.Checkers.Test
{
    public class PossibleActionsTests
    {
        readonly Checkers checkers = new();
        [Fact]
        public void PossibleActionsForEmptyBoardShouldBeEmpty()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            var possibleActions = checkers.PossibleActions(state);
            Assert.Empty(possibleActions);
        }
        [Fact]
        public void PossibleActionForA1PawnShouldBeB2()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(0, 0, Piece.WhitePawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            MoveAction action = new( new Field(0, 0), new Field(1, 1) );
            Assert.Equal(possibleActions.First(), action);
        }
        [Fact]
        public void PossibleActionsForBlockedA1PawnShouldBeEmpty()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(0, 0, Piece.WhitePawn);
            state.SetPieceAt(1, 1, Piece.BlackPawn);
            state.SetPieceAt(2, 2, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Empty(possibleActions);
        }
        [Fact]
        public void PossibleActionsForC1PawnShouldBeTwoMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(2, 0, Piece.WhitePawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            MoveAction action = new(new Field(2, 0), new Field(1, 1));
            Assert.Contains(action ,possibleActions);
            action = new(new Field(2, 0), new Field(3, 1));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionForA1PawnShouldBeCapture()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(0, 0, Piece.WhitePawn);
            state.SetPieceAt(1, 1, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(new Field(0, 0), new Field(1, 1), new Field(2, 2));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionForC3PawnShouldBeCapture()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(3, 3, Piece.WhitePawn);
            state.SetPieceAt(2, 4, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new(new Field(3, 3), new Field(2, 4), new Field(1, 5));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionsForC3PawnShouldBeTwoCaptures()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(3, 3, Piece.WhitePawn);
            state.SetPieceAt(2, 4, Piece.BlackPawn);
            state.SetPieceAt(4, 2, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Equal(2, possibleActions.Count());
            CaptureAction action = new(new Field(3, 3), new Field(2, 4), new Field(1, 5));
            Assert.Contains(action, possibleActions);
            action = new(new Field(3, 3), new Field(4, 2), new Field(5, 1));
            Assert.Contains(action, possibleActions);
        }
        [Fact]
        public void PossibleActionForC3PawnShouldBeDoubleCaptureOnly()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(3, 3, Piece.WhitePawn);
            state.SetPieceAt(2, 4, Piece.BlackPawn);
            state.SetPieceAt(4, 4, Piece.BlackPawn);
            state.SetPieceAt(6, 4, Piece.BlackPawn);
            var possibleActions = checkers.PossibleActions(state);
            Assert.Single(possibleActions);
            CaptureAction action = new( new Field(5, 5), new Field(6, 4), new Field(7, 3));
            action.CombineCapture(new Field(3, 3), new Field(4, 4));
            Assert.Contains(action, possibleActions);
        }
    }
}

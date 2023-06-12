using Game.Checkers;
using Game.IGame;

namespace Ai.Tests.Uct
{
    public class TestsForCheckers
    {
        private const int iterations = 10;

        private readonly Uct<ICheckersAction, CheckersState, ICheckersInputState> uct
            = new(1.414, new Checkers(), new IterationStopCondition(iterations));

        [Fact]
        public void MoveAssessmentReturns2WinningMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(16, Piece.WhitePawn);
            state.SetPieceAt(17, Piece.WhitePawn);
            state.SetPieceAt(13, Piece.BlackPawn);
            var assesments = uct.MoveAssessment(new GameTree<ICheckersAction, CheckersState>(state));
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
            var assesments = uct.MoveAssessment(new GameTree<ICheckersAction, CheckersState>(state));
            Assert.Equal(2, assesments.Count);
            CaptureAction expected = new(14, 17, 21);
            Assert.Contains((expected, 1), assesments);
            expected = new(14, 10, 7);
            Assert.Contains((expected, -1), assesments);
        }
        [Fact]
        public void MoveAssessmentAddsNumberOfNewNodesToGameTree()
        {
            CheckersState state = CheckersState.GetInitialState();
            GameTree<ICheckersAction, CheckersState> gameTree = new(state);
            _ = uct.MoveAssessment(gameTree);
            int nodesCount = CountTreeNodes(gameTree.Root);
            Assert.Equal(iterations + 1, nodesCount);
            _ = uct.MoveAssessment(gameTree);
            nodesCount = CountTreeNodes(gameTree.Root);
            Assert.Equal(2 * iterations + 1, nodesCount);
        }

        private int CountTreeNodes(Node<ICheckersAction, CheckersState> root)
        {
            int count = 1;
            foreach (Node<ICheckersAction, CheckersState> node in root.ExpandedChildren)
            {
                count += CountTreeNodes(node);
            }
            return count;
        }
    }
}

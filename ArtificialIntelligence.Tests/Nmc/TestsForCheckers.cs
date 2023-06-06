using Game.Checkers;
using Game.IGame;

namespace Ai.Tests.Nmcs
{
    public class TestsForCheckers
    {
        private const int iterations = 10;
        private const int depth = 3;

        private readonly Nmcs<ICheckersAction, CheckersState, ICheckersInputState> nmcs
            = new(depth, new Checkers(), new IterationStopCondition(iterations));

        [Fact]
        public void MoveAssessmentReturns2WinningMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(16, Piece.WhitePawn);
            state.SetPieceAt(17, Piece.WhitePawn);
            state.SetPieceAt(13, Piece.BlackPawn);
            var assesments = nmcs.MoveAssessment(new GameTree<ICheckersAction, CheckersState>(state));
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
            var assesments = nmcs.MoveAssessment(new GameTree<ICheckersAction, CheckersState>(state));
            Assert.Equal(2, assesments.Count);
            CaptureAction expected = new(14, 17, 21);
            Assert.Contains((expected, 1), assesments);
            expected = new(14, 10, 7);
            Assert.Contains((expected, -1), assesments);
        }
        [Fact]
        public void MoveAssessmentAddsLayerOfNewNodesToGameTree()
        {
            CheckersState state = CheckersState.GetInitialState();
            GameTree<ICheckersAction, CheckersState> gameTree = new(state);
            ICheckersAction action = nmcs.ChooseAction(gameTree);
            Assert.Equal(depth, TreeDepth(gameTree.Root));

            gameTree.SelectChildNode(action);
            _ = nmcs.MoveAssessment(gameTree);
            Assert.Equal(depth + 1, TreeDepth(gameTree.Root));
        }

        private int TreeDepth(Node<ICheckersAction, CheckersState> root)
        {
            int depth = 0;
            foreach (Node<ICheckersAction, CheckersState> node in root.ExpandedChildren)
            {
                depth = Math.Max(TreeDepth(node), depth);
            }
            return depth + 1;
        }
    }
}

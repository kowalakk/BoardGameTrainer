using Game.Checkers;
using Game.IGame;
using Gdk;

namespace Ai.Tests.Uct
{
    public class TestsForCheckers
    {
        private const int iterations = 10;

        private static readonly Checkers checkers = new();

        private readonly Uct<ICheckersAction, CheckersState, ICheckersInputState> uct
            = new(1.414, checkers, new IterationStopCondition(iterations));

        private readonly CancellationToken token = new CancellationTokenSource().Token;

        [Fact]
        public void MoveAssessmentReturns2WinningMoves()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(16, Piece.WhitePawn);
            state.SetPieceAt(17, Piece.WhitePawn);
            state.SetPieceAt(13, Piece.BlackPawn);
            var assesments = uct.MoveAssessment(new GameTree<ICheckersAction, CheckersState>(state), token);
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
            var assesments = uct.MoveAssessment(new GameTree<ICheckersAction, CheckersState>(state), token);
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
            _ = uct.MoveAssessment(gameTree, token);
            int nodesCount = CountTreeNodes(gameTree.Root);
            Assert.Equal(iterations + 1, nodesCount);
            _ = uct.MoveAssessment(gameTree, token);
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
        [Fact]
        public void BoardWithOnlyCrownedPiecesLeadsToADraw()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(0, Piece.WhiteCrowned);
            state.SetPieceAt(1, Piece.WhiteCrowned);
            state.SetPieceAt(2, Piece.BlackCrowned);
            state.SetPieceAt(3, Piece.BlackCrowned);

            CancellationToken token = new();
            GameTree<ICheckersAction, CheckersState> gameTree = new(state);
            GameResult gameResult = GameResult.InProgress;
            while (gameResult == GameResult.InProgress)
            {
                ICheckersAction nextAction = uct.ChooseAction(gameTree, token);
                uct.MoveGameToNextState(gameTree, nextAction);
                state = gameTree.SelectedNode.CorespondingState;
                gameResult = checkers.Result(state);
            }

            Assert.Equal(GameResult.Draw, gameResult);
        }
    }
}

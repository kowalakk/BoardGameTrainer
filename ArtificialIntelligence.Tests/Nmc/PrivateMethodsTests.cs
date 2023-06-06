using Game.Checkers;
using Game.IGame;
using System.Reflection;
using Assert = Xunit.Assert;

namespace Ai.Tests.Nmcs
{
    public class PrivateMethodsTests
    {
        private const int iterations = 10;
        private const int depth = 3;

        private readonly Nmcs<ICheckersAction, CheckersState, ICheckersInputState> nmcs
            = new(depth, new Checkers(), new IterationStopCondition(iterations));
        [Fact]
        public void NestingTest()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt(16, Piece.WhitePawn);
            state.SetPieceAt(17, Piece.WhitePawn);
            state.SetPieceAt(13, Piece.BlackPawn);

            Node<ICheckersAction, CheckersState> root = new(state);
            List<Node<ICheckersAction, CheckersState>> leaves = new();
            MethodInfo method = nmcs.GetType().GetMethod("Nesting", BindingFlags.NonPublic | BindingFlags.Instance)!;
            method.Invoke(nmcs, new object[] { depth, root, leaves });

            Assert.Equal(2, leaves.Count);
        }
        [Fact]
        public void DefaultPolicyTest()
        {
            CheckersState state = CheckersState.GetInitialState();
            MethodInfo method = nmcs.GetType().GetMethod("DefaultPolicy", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var retVal = method.Invoke(nmcs, new object[] { state });
            Assert.IsAssignableFrom<GameResult>(retVal);
        }
        [Fact]
        public void BackupTest()
        {
            Node<ICheckersAction, CheckersState> root = new(CheckersState.GetEmptyBoardState())
            {
                VisitCount = 0,
                SuccessCount = 0
            };
            Node<ICheckersAction, CheckersState> child1 = new(
                new MoveAction(29, 24), CheckersState.GetEmptyBoardState(Player.Two), root)
            {
                VisitCount = 0,
                SuccessCount = 0
            };
            root.ExpandedChildren.Add(child1);
            Node<ICheckersAction, CheckersState> child2 = new(
                new MoveAction(29, 24), CheckersState.GetEmptyBoardState(Player.Two), root)
            {
                VisitCount = 3,
                SuccessCount = -1
            };
            root.ExpandedChildren.Add(child2);
            Node<ICheckersAction, CheckersState> grandchild = new(
                new MoveAction(29, 25), CheckersState.GetEmptyBoardState(), child1)
            {
                VisitCount = 7,
                SuccessCount = 5
            };
            child1.ExpandedChildren.Add(grandchild);

            List<Node<ICheckersAction, CheckersState>> leaves = new() { child2, grandchild };

            MethodInfo method = nmcs.GetType().GetMethod("Backup", BindingFlags.NonPublic | BindingFlags.Instance)!;
            method.Invoke(nmcs, new object[] { leaves });
            Assert.Equal(10, root.VisitCount);
            Assert.Equal(7, child1.VisitCount);
            Assert.Equal(6, root.SuccessCount);
            Assert.Equal(-5, child1.SuccessCount);
        }
    }
}

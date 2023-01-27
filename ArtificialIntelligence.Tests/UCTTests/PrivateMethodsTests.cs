using Game.Checkers;
using Game.IGame;
using System.Reflection;
using Assert = Xunit.Assert;

namespace Ai.Tests.UCT
{
    public class PrivateMethodsTests
    {
        private readonly Uct<ICheckersAction, CheckersState, ICheckersInputState> uct = new(1.414, new Checkers(), new IterationStopCondition(10));
        [Fact]
        public void TreePolicyTest()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("B4", Piece.WhitePawn);
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("C5", Piece.BlackPawn);

            Node<ICheckersAction, CheckersState> root = new(state);
            MethodInfo method = uct.GetType().GetMethod("TreePolicy", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var retVal = method.Invoke(uct, new object[] { root });
            Assert.IsAssignableFrom<Node<ICheckersAction, CheckersState>>(retVal);

            retVal = method.Invoke(uct, new object[] { root });
            Assert.IsAssignableFrom<Node<ICheckersAction, CheckersState>>(retVal);

            retVal = method.Invoke(uct, new object[] { root });
            var node1 = Assert.IsAssignableFrom<Node<ICheckersAction, CheckersState>>(retVal);

            retVal = method.Invoke(uct, new object[] { root });
            var node2 = Assert.IsAssignableFrom<Node<ICheckersAction, CheckersState>>(retVal);

            Assert.Equal(node1, node2);

        }
        [Fact]
        public void ExpandTest()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("B4", Piece.WhitePawn);

            Node<ICheckersAction, CheckersState> root = new(state);
            MethodInfo method = uct.GetType().GetMethod("Expand", BindingFlags.NonPublic | BindingFlags.Instance)!;

            method.Invoke(uct, new object[] { root });

            Assert.NotNull(root.UnexpandedChildren);
            Assert.NotNull(root.ExpandedChildren);
            Assert.Equal(2, root.UnexpandedChildren!.Count);
            Assert.Empty(root.ExpandedChildren);
        }
        [Fact]
        public void DefaultPolicyTest()
        {
            CheckersState state = CheckersState.GetInitialState();
            MethodInfo method = uct.GetType().GetMethod("DefaultPolicy", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var retVal = method.Invoke(uct, new object[] { state });
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
            Node<ICheckersAction, CheckersState> child = new(
                new MoveAction(new("C1"), new("B2")), CheckersState.GetEmptyBoardState(Player.Two), root)
            {
                VisitCount = 0,
                SuccessCount = 0
            };
            root.ExpandedChildren.Add(child);
            Node<ICheckersAction, CheckersState> grandchild = new(
                new MoveAction(new("C1"), new("D2")), CheckersState.GetEmptyBoardState(), child)
            {
                VisitCount = 0,
                SuccessCount = 0
            };
            child.ExpandedChildren.Add(grandchild);


            MethodInfo method = uct.GetType().GetMethod("Backup", BindingFlags.NonPublic | BindingFlags.Instance)!;
            method.Invoke(uct, new object[] { grandchild, GameResult.Draw });
            Assert.Equal(1, root.VisitCount);
            Assert.Equal(1, child.VisitCount);
            Assert.Equal(1, grandchild.VisitCount);
            method.Invoke(uct, new object[] { grandchild, GameResult.PlayerTwoWins });
            Assert.Equal(2, root.VisitCount);
            Assert.Equal(2, child.VisitCount);
            Assert.Equal(2, grandchild.VisitCount);
            Assert.Equal(-1, root.SuccessCount);
            Assert.Equal(1, child.SuccessCount);
            Assert.Equal(-1, grandchild.SuccessCount);
        }

        [Fact]
        public void BestChildTest()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            Node<ICheckersAction, CheckersState> root = new(state)
            {
                VisitCount = 10,
                SuccessCount = -4
            };
            Node<ICheckersAction, CheckersState> child1 = new(new MoveAction(new("C1"), new("B2")), state, root)
            {
                VisitCount = 5,
                SuccessCount = 3
            };
            root.ExpandedChildren.Add(child1);
            Node<ICheckersAction, CheckersState> child2 = new(new MoveAction(new("C1"), new("D2")), state, root)
            {
                VisitCount = 5,
                SuccessCount = 1
            };
            root.ExpandedChildren.Add(child2);
            MethodInfo method = uct.GetType().GetMethod("BestChild", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var retVal = method.Invoke(uct, new object[] { root });
            var node = Assert.IsAssignableFrom<Node<ICheckersAction, CheckersState>>(retVal);
            Assert.Equal(child1, node);
        }
    }
}

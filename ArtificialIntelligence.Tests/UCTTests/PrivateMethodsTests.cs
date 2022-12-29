using Game.Checkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace AI.Tests.UCTTests
{
    public class PrivateMethodsTests
    {
        private readonly PrivateObject obj = new(new UCT<CheckersAction, CheckersState, CheckersInputState>(1.414, new Checkers(), new IterationStopCondition(2)));
        
        [Fact]
        public void Test1()
        {
            var retVal = obj.Invoke("PrivateMethod");
            //Assert.Equal(expectedVal, retVal);

            CheckersState state = CheckersState.GetEmptyBoardState();
            state.SetPieceAt("B4", Piece.WhitePawn);
            state.SetPieceAt("D4", Piece.WhitePawn);
            state.SetPieceAt("C5", Piece.BlackPawn);
            //var assesments = uct.MoveAssessment(state);
            //Assert.Equal(2, assesments.Count);
            //CaptureAction expected = new(new("B4"), new("C5"), new("D6"));
            //Assert.Contains((expected, 1), assesments);
            //expected = new(new("D4"), new("C5"), new("B6"));
            //Assert.Contains((expected, 1), assesments);
        }
    }
}

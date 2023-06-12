using Game.IGame;
using Game.Othello;

namespace Ai.Tests.Nmcs
{
    public class TestsForOthello
    {
        private Nmcs<IOthelloAction, OthelloState, LanguageExt.Unit> nmcs = new(3, new Othello(), new IterationStopCondition(10));

        [Fact]
        public void MoveAssessmentReturns4Moves()
        {
            var assesments = nmcs.MoveAssessment(new GameTree<IOthelloAction, OthelloState>(OthelloState.GenerateInitialOthelloState()));
            Assert.Equal(4, assesments.Count);
        }
    }
}
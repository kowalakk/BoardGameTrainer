using Game.IGame;
using Game.Othello;

namespace Ai.Tests.Nmcs
{
    public class TestsForOthello
    {
        private readonly Nmcs<IOthelloAction, OthelloState, LanguageExt.Unit> nmcs = new(3, new Othello(), new IterationStopCondition(10));

        private readonly CancellationToken token = new CancellationTokenSource().Token;

        [Fact]
        public void MoveAssessmentReturns4Moves()
        {
            var assesments = nmcs.MoveAssessment(
                new GameTree<IOthelloAction, 
                OthelloState>(OthelloState.GenerateInitialOthelloState()),
                token);
            Assert.Equal(4, assesments.Count);
        }
    }
}
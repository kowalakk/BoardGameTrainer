using Game.IGame;
using Game.Othello;

namespace Ai.Tests.Uct
{
    public class TestsForOthello
    {
        private Uct<IOthelloAction, OthelloState, LanguageExt.Unit> uct = new Uct<IOthelloAction, OthelloState, LanguageExt.Unit>(1.414, new Othello(), new IterationStopCondition(10));

        [Fact]
        public void MoveAssessmentReturns4Moves()
        {
            var assesments = uct.MoveAssessment(new GameTree<IOthelloAction, OthelloState>(OthelloState.GenerateInitialOthelloState()));
            Assert.Equal(4, assesments.Count);
        }

        [Fact]
        public void ChooseMoveReturnsAnObviouslyBestMove()
        {
            var assesments = uct.MoveAssessment(new GameTree<IOthelloAction, OthelloState>(OthelloState.GenerateInitialOthelloState()));
            var bestMove = assesments.MaxBy(action => { return action.Item2; }).Item1;
            Assert.Equal(bestMove, uct.ChooseAction(new GameTree<IOthelloAction, OthelloState>(OthelloState.GenerateInitialOthelloState())));
        }


    }
}
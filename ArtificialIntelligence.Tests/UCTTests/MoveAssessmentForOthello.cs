using Game.Othello;

namespace AI.Tests.UCT
{
    public class MoveAssessmentForOthello
    {
        private UCT<OthelloAction, OthelloState, LanguageExt.Unit> uct = new UCT<OthelloAction, OthelloState, LanguageExt.Unit>(1.414, new Othello(), new IterationStopCondition(10));

        [Fact]
        public void MoveAssessmentReturns4Moves()
        {
            var assesments = uct.MoveAssessment(OthelloState.GenerateInitialOthelloState());
            Assert.Equal(4, assesments.Count);
        }

        [Fact]
        public void ChooseMoveReturnsAnObviouslyBestMove()
        {
            var assesments = uct.MoveAssessment(OthelloState.GenerateInitialOthelloState());
            var bestMove = assesments.MaxBy(action => { return action.Item2; }).Item1;
            Assert.Equal(bestMove, uct.ChooseMove(OthelloState.GenerateInitialOthelloState()));
        }


    }
}
using Game.IGame;
using Game.Othello;

namespace Ai.Tests.Uct
{
    public class TestsForOthello
    {
        private readonly Uct<IOthelloAction, OthelloState, LanguageExt.Unit> uct = new(1.414, new Othello(), new IterationStopCondition(10));
        
        private readonly CancellationToken token = new CancellationTokenSource().Token;

        [Fact]
        public void MoveAssessmentReturns4Moves()
        {
            var assesments = uct.MoveAssessment(
                new GameTree<IOthelloAction, 
                OthelloState>(OthelloState.GenerateInitialOthelloState()), 
                token);
            Assert.Equal(4, assesments.Count);
        }

        [Fact]
        public void ChooseMoveReturnsAnObviouslyBestMove()
        {
            var assesments = uct.MoveAssessment(
                new GameTree<IOthelloAction, 
                OthelloState>(OthelloState.GenerateInitialOthelloState()), 
                token);
            var bestMove = assesments.MaxBy(action => { return action.Item2; }).Item1;
            Assert.Equal(bestMove, uct.ChooseAction(
                new GameTree<IOthelloAction, 
                OthelloState>(OthelloState.GenerateInitialOthelloState()), 
                token));
        }


    }
}
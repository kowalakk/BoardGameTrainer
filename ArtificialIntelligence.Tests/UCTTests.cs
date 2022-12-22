using Game.Othello;
using Gtk;

namespace ArtificialIntelligence.Tests
{
    public class UCTTests
    {
        private UCT<OthelloAction, OthelloState, LanguageExt.Unit> uct = new UCT<OthelloAction, OthelloState, LanguageExt.Unit>(1.414, new Othello());

        [Fact]
        public void MoveAssessmentReturnsCorrectEstimateValues()
        {
            
        }

        [Fact]
        public void ChooseMoveReturnsAnObviouslyBestMove()
        {

        }


    }
}
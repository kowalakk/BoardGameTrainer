using Game.IGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Othello.Tests
{
    public class OthelloTests
    {
        Othello othello = new Othello();
        [Fact]
        void GameResultShouldBeInProgressForInitialState()
        {
            Assert.Equal(GameResults.InProgress, othello.GameResult(OthelloState.GenerateInitialOthelloState()));
        }

        [Fact]
        void PerformActionShouldReturnCorrectNewState()
        {

        }

        [Fact]
        void PerformActionWithNullActionParameter()
        {
            OthelloState state = OthelloState.GenerateInitialOthelloState();
            state.BlacksTurn = false;
            Assert.Equal(state, othello.PerformAction(new OthelloEmptyAction(), OthelloState.GenerateInitialOthelloState()));
        }
    }
}

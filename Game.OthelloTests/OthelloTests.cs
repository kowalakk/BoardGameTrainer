using Game.IGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Game.Othello.OthelloState;

namespace Game.Othello.Tests
{
    public class OthelloTests
    {
        int boardSize = 8;
        Othello othello = new Othello();
        [Fact]
        void GameResultShouldBeInProgressForInitialState()
        {
            Assert.Equal(GameResults.InProgress, othello.GameResult(OthelloState.GenerateInitialOthelloState()));
        }

        [Fact]
        void PerformActionShouldReturnCorrectNewState()
        {
            Field[,] board = new Field[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    board[i, j] = Field.Empty;
            board[2, 3] = board[3,3] = board[3, 4] = board[4, 3] = Field.Black;
            board[4, 4] = Field.White;
            Assert.Equal(new OthelloState(board, 30, 29, false), othello.PerformAction(new OthelloFullAction((2, 3), Field.Black), OthelloState.GenerateInitialOthelloState()));

        }

        [Fact]
        void PerformEmptyActionShouldReturnCorrectNewState()
        {
            OthelloState state = OthelloState.GenerateInitialOthelloState();
            state.BlacksTurn = false;
            Assert.Equal(state, othello.PerformAction(new OthelloEmptyAction(), OthelloState.GenerateInitialOthelloState()));
        }

        [Fact]
        void PossibleActionsFromInitialState()
        {
            List<OthelloAction> actions = new List<OthelloAction>();
            actions.Add(new OthelloFullAction((2, 3), Field.Black));
            actions.Add(new OthelloFullAction((3, 2), Field.Black));
            actions.Add(new OthelloFullAction((4, 5), Field.Black));
            actions.Add(new OthelloFullAction((5, 4), Field.Black));

            Assert.Equivalent(actions, othello.PossibleActions(OthelloState.GenerateInitialOthelloState()));
        }

        [Fact]
        void PossibleActionsShouldReturnEmptyList()
        {
            Field[,] board = new Field[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    board[i, j] = Field.Empty;
            board[0, 0] = Field.White;
            OthelloState state = new OthelloState(board, 23, 22, false);
            Assert.Equal(new List<OthelloFullAction>(), othello.PossibleActions(state));
        }
    }
}

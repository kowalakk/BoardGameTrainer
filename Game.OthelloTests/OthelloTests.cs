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
            Assert.Equal(GameResult.InProgress, othello.Result(OthelloState.GenerateInitialOthelloState()));
        }

        [Fact]
        void GameResultShouldReturnCorrectWinner()
        {
            Field[,] white = new Field[boardSize, boardSize];
            Field[,] black = new Field[boardSize, boardSize];
            Field[,] half = new Field[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    white[i, j] = Field.White;
                    black[i, j] = Field.Black;
                }
            for (int i = 0; i < boardSize / 2; i++)
                for (int j = 0; j < boardSize; j++)
                    half[i, j] = Field.White;
            for (int i = boardSize / 2; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    half[i, j] = Field.Black;
            Assert.Equal(GameResult.PlayerTwoWins, othello.Result(new OthelloState(white, 0, 0, true)));
            Assert.Equal(GameResult.PlayerOneWins, othello.Result(new OthelloState(black, 0, 0, true)));
            Assert.Equal(GameResult.Draw, othello.Result(new OthelloState(half, 0, 0, true)));
        }

        [Fact]
        void PerformActionShouldReturnCorrectNewState()
        {
            Field[,] board = new Field[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    board[i, j] = Field.Empty;
            board[2, 3] = board[3, 3] = board[3, 4] = board[4, 3] = Field.Black;
            board[4, 4] = Field.White;
            OthelloState expected = new OthelloState(board, 30, 29, false);
            OthelloState actual = othello.PerformAction(new OthelloFullAction((2, 3), Field.Black, 0, 1, 0, 0, 0, 0, 0, 0), OthelloState.GenerateInitialOthelloState());
            Assert.Equal(expected, actual);

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
            List<IOthelloAction> actions = new List<IOthelloAction>();
            actions.Add(new OthelloFullAction((2, 3), Field.Black, 0, 1, 0, 0, 0, 0, 0, 0));
            actions.Add(new OthelloFullAction((3, 2), Field.Black, 0, 0, 0, 1, 0, 0, 0, 0));
            actions.Add(new OthelloFullAction((4, 5), Field.Black, 0, 0, 1, 0, 0, 0, 0, 0));
            actions.Add(new OthelloFullAction((5, 4), Field.Black, 1, 0, 0, 0, 0, 0, 0, 0));

            Assert.Equivalent(actions, othello.PossibleActions(OthelloState.GenerateInitialOthelloState()));
        }

        [Fact]
        void PossibleActionsShouldReturnOthelloEmptyAction()
        {
            Field[,] board = new Field[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    board[i, j] = Field.Empty;
            board[0, 0] = Field.White;
            OthelloState state = new OthelloState(board, 23, 22, false);
            Assert.Equal(new List<IOthelloAction>() { new OthelloEmptyAction() }, othello.PossibleActions(state));
        }

        [Fact]
        void PossibleActionsShouldReturnListContainingOneOthelloEmptyActionIfCurrentPlayerHasNoPieces()
        {
            List<OthelloAction> actionsEmpty = new List<OthelloAction>
            {
                new OthelloEmptyAction()
            };
            OthelloState state = GenerateInitialOthelloState();
            state.BlackHandCount = 0;
            Assert.Equal(actionsEmpty, othello.PossibleActions(state));
            state.BlacksTurn = false;
            state.BlackHandCount = 1;
            state.WhiteHandCount = 0;
            Assert.Equal(actionsEmpty, othello.PossibleActions(state));
        }

        [Fact]
        void FilterByInputStateShouldntChangeAnything()
        {
            List<OthelloFullAction> actions = new();
            actions.Add(new OthelloFullAction((0, 2), Field.Black, 1, 2, 1, 0, 0, 0, 0, 0));
            actions.Add(new OthelloFullAction((0, 2), Field.Black, 1, 3, 0, 0, 0, 0, 0, 0));
            actions.Add(new OthelloFullAction((1, 4), Field.Black, 4, 1, 1, 1, 0, 0, 0, 0));
            actions.Add(new OthelloFullAction((0, 7), Field.White, 0, 0, 1, 0, 0, 0, 0, 0));
            Assert.Equal(actions, othello.FilterByInputState(actions, new LanguageExt.Unit()));
        }


    }
}

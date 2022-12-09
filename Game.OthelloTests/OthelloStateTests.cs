using Game.Othello;
using static Game.Othello.OthelloState;

namespace Game.OthelloTests
{
    public class OthelloStateTests
    {
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            OthelloState othelloState = OthelloState.GenerateInitialOthelloState();
            Assert.False(othelloState.Equals(null));
        }

        [Fact]
        void EqualsTwoIdenticalStatesShouldBeTrue()
        {
            Field[,] board1 = new Field[8, 8];
            Field[,] board2= new Field[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    board1[i, j] = Field.White;
                    board2[i, j] = Field.White;
                }
                    
            int hand = 10;
            OthelloState state1 = new OthelloState(board1, hand, hand, true);
            OthelloState state2 = new OthelloState(board2, hand, hand, true);
            Assert.True(state1.Equals(state2));
        }

        [Fact]
        void EqualsOfStatesWithDifferentBoardsShouldBeFalse()
        {
            Field[,] board1 = new Field[8, 8];
            Field[,] board2 = new Field[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    board1[i, j] = Field.White;
                    board2[i, j] = Field.Black;
                }
                    
            int hand = 10;
            OthelloState state1 = new OthelloState(board1, hand, hand, true);
            OthelloState state2 = new OthelloState(board2, hand, hand, true);
            Assert.False(state1.Equals(state2));
        }

        [Fact]
        void EqualsOfStatesWithDifferentHandsShouldBeFalse()
        {
            Field[,] board = new Field[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j] = Field.White;
            int hand = 10;
            OthelloState state1 = new OthelloState(board, hand, hand, true);
            OthelloState state2 = new OthelloState(board, hand, hand + 1, true);
            Assert.False(state1.Equals(state2));
        }

        [Fact]
        void EqualsOfStatesWithDifferentPleyersTurnShouldBeFalse()
        {
            Field[,] board = new Field[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j] = Field.White;
            int hand = 10;
            OthelloState state1 = new OthelloState(board, hand, hand, true);
            OthelloState state2 = new OthelloState(board, hand, hand, false);
            Assert.False(state1.Equals(state2));
        }

        [Fact]
        void CreationOfStateWithInvalidParametersShouldThrowAnException()
        {
            Assert.Throws<ArgumentException>(() => new OthelloState(new Field[8, 8], -1, 0, true));
            Assert.Throws<ArgumentException>(() => new OthelloState(new Field[1, 8], 30, 30, true));
        }
    }
}
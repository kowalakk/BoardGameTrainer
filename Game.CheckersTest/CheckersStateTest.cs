using Game.Checkers;
namespace Game.CheckersTest
{
    public class OthelloStateTests
    {
        private Piece[,] emptyBoard = new Piece[,]
            {
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
                { Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None },
            };
        [Fact]
        public void EqualsShouldBeTrue()
        {
            CheckersState state1 = new CheckersState();
            CheckersState state2 = new CheckersState(state1);
            Assert.True(state1.Equals(state2));
            Assert.True(state2.Equals(state1));
        }
        [Fact]
        public void NotEqualPlayerShouldBeFalse()
        {
            CheckersState state1 = new CheckersState(emptyBoard, Player.White);
            CheckersState state2 = new CheckersState(emptyBoard, Player.Black);
            Assert.False(state1.Equals(state2));
            Assert.False(state2.Equals(state1));
        }
        [Fact]
        public void NotEqualBoardsShouldBeFalse()
        {
            CheckersState state1 = new CheckersState();
            CheckersState state2 = new CheckersState(emptyBoard, Player.White);
            Assert.False(state1.Equals(state2));
            Assert.False(state2.Equals(state1));
        }
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            CheckersState state = new CheckersState();
            Assert.False(state.Equals(null));
        }
    }
}
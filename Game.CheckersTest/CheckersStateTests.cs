using Game.Checkers;

namespace Game.CheckersTest
{
    public class CheckersStateTests
    {
        [Fact]
        public void EqualsShouldBeTrue()
        {
            CheckersState state1 = CheckersState.GetInitialState();
            CheckersState state2 = new CheckersState(state1);
            Assert.True(state1.Equals(state2));
            Assert.True(state2.Equals(state1));
        }
        [Fact]
        public void NotEqualPlayersShouldBeFalse()
        {
            CheckersState state1 = CheckersState.GetEmptyBoardState();
            CheckersState state2 = CheckersState.GetEmptyBoardState(Player.Black);
            Assert.False(state1.Equals(state2));
            Assert.False(state2.Equals(state1));
        }
        [Fact]
        public void NotEqualBoardsShouldBeFalse()
        {
            CheckersState state1 = CheckersState.GetInitialState();
            CheckersState state2 = CheckersState.GetEmptyBoardState();
            Assert.False(state1.Equals(state2));
            Assert.False(state2.Equals(state1));
        }
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            CheckersState state = CheckersState.GetInitialState();
            Assert.False(state.Equals(null));
        }
        [Fact]
        public void DeepCopyOfBoard()
        {
            CheckersState state1 = CheckersState.GetEmptyBoardState();
            state1.SetPieceAt(0,0,Piece.WhiteCrowned);
            CheckersState state2 = CheckersState.GetEmptyBoardState();
            Assert.False(state1.Equals(state2));
        }
    }
}
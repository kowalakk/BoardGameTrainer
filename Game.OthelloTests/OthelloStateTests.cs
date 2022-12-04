using Game.Othello;
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
    }
}
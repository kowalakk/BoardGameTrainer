namespace Game.Checkers.Tests
{
    public class MoveActionTests
    {
        [Fact]
        public void EqualsShouldBeTrue()
        {
            MoveAction action1 = new(28, 24);
            MoveAction action2 = new(28, 24);
            Assert.Equal(action1, action2);
        }
        [Fact]
        public void NotEqualsShouldBeFalse()
        {
            MoveAction action1 = new(29, 24);
            MoveAction action2 = new(29, 25);
            Assert.NotEqual(action1, action2);
        }
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            MoveAction action = new(29, 24);
            Assert.NotNull(action);
        }
    }
}

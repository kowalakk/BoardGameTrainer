namespace Game.Checkers.Test
{
    public class MoveActionTests
    {
        [Fact]
        public void EqualsShouldBeTrue()
        {
            Field start = new Field(0, 0);
            Field end = new Field(1, 1);
            MoveAction action1 = new(start, end);
            MoveAction action2 = new(start, end);
            Assert.Equal(action1, action2);
        }
        [Fact]
        public void NotEqualsShouldBeFalse()
        {
            MoveAction action1 = new(new Field(2, 0), new Field(1, 1));
            MoveAction action2 = new(new Field(2, 0), new Field(3, 1));
            Assert.NotEqual(action1, action2);
        }
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            MoveAction action = new(new Field(2, 0), new Field(1, 1));
            Assert.NotNull(action);
        }
    }
}

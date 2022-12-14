namespace Game.Checkers.Test
{
    public class CaptureActionTests
    {
        [Fact]
        public void EqualsShouldBeTrue()
        {
            CaptureAction action1 = new(new Field(0, 0), new Field(1, 1), new Field(2, 2));
            CaptureAction action2 = new(new Field(0, 0), new Field(1, 1), new Field(2, 2));
            Assert.Equal(action1, action2);
        }
        [Fact]
        public void NotEqualsShouldBeFalse()
        {
            CaptureAction action1 = new(new Field(0, 0), new Field(1, 1), new Field(2, 2));
            CaptureAction action2 = new(new Field(2, 2), new Field(1, 1), new Field(0, 0));
            Assert.NotEqual(action1, action2);
        }
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            CaptureAction action = new(new Field(0, 0), new Field(1, 1), new Field(2, 2));
            Assert.NotNull(action);
        }
        [Fact]
        public void CombineCaptureTest()
        {
            CaptureAction action = new(new Field(2, 2), new Field(1, 1), new Field(0, 0));
            action.CombineCapture(new Field(4, 0), new Field(3, 1));
            Assert.Equal(new Field(4, 0), action.Start);
            Assert.Equal(2, action.CapturesCount);
            LinkedList<SimpleCapture> captures = new();
            captures.AddLast(new SimpleCapture(new Field(3, 1), new Field(2, 2)));
            captures.AddLast(new SimpleCapture(new Field(1, 1), new Field(0, 0)));
            Assert.Equal(action.Captures, captures);
        }
    }
}

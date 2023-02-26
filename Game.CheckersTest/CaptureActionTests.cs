namespace Game.Checkers.Tests
{
    public class CaptureActionTests
    {
        [Fact]
        public void EqualsShouldBeTrue()
        {
            CaptureAction action1 = new(29, 25, 22);
            CaptureAction action2 = new(29, 25, 22);
            Assert.Equal(action1, action2);
        }
        [Fact]
        public void NotEqualsShouldBeFalse()
        {
            CaptureAction action1 = new(29, 25, 22);
            CaptureAction action2 = new(22, 25, 29);
            Assert.NotEqual(action1, action2);
        }
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            CaptureAction action = new(29, 25, 22);
            Assert.NotNull(action);
        }
        [Fact]
        public void CombineCaptureTest()
        {
            CaptureAction action = new(22, 25, 29);
            action.CombineCapture(31, 26);
            Assert.Equal(31, action.Start);
            Assert.Equal(2, action.CapturesCount);
            LinkedList<SimpleCapture> captures = new();
            captures.AddLast(new SimpleCapture(31, 26, 22));
            captures.AddLast(new SimpleCapture(22, 25, 29));
            Assert.Equal(action.Captures, captures);
        }
    }
}

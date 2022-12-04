using Game.Checkers;

namespace Game.CheckersTest
{
    public class CheckersActionTests
    {
        [Fact]
        public void EqualsShouldBeTrue()
        {
            List<Field> fields = new List<Field> { new Field(0,0,Piece.WhitePawn), new Field(1, 1, Piece.BlackPawn) };
            CheckersAction action1 = new CheckersAction(fields);
            CheckersAction action2 = new CheckersAction(fields);
            Assert.True(action1.Equals(action2));
            Assert.True(action2.Equals(action1));
        }
        [Fact]
        public void NotEqualsShouldBeFalse()
        {
            List<Field> fields1 = new() { new Field(0, 0, Piece.WhitePawn), new Field(1, 1, Piece.BlackPawn) };
            List<Field> fields2 = new() { new Field(1, 0, Piece.WhiteCrowned), new Field(0, 1, Piece.BlackPawn) };
            CheckersAction action1 = new CheckersAction(fields1);
            CheckersAction action2 = new CheckersAction(fields2);
            Assert.False(action1.Equals(action2));
            Assert.False(action2.Equals(action1));
        }
        [Fact]
        public void DeepCopyOfFields()
        {
            List<Field> fields = new() { new Field(0, 0, Piece.WhitePawn), new Field(1, 1, Piece.BlackPawn) };
            CheckersAction action1 = new CheckersAction(fields);
            fields.Insert(0, new Field(0, 0, Piece.None));
            CheckersAction action2 = new CheckersAction(fields);
            Assert.False(action1.Equals(action2));
        }
        [Fact]
        public void EqualsWithNullShouldBeFalse()
        {
            List<Field> fields = new() { new Field(0, 0, Piece.WhitePawn), new Field(1, 1, Piece.BlackPawn) };
            CheckersAction action = new CheckersAction(fields);
            Assert.False(action.Equals(null));
        }
    }
}

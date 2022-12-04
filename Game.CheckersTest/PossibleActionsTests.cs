using Game.Checkers;

namespace Game.CheckersTest
{
    public class PossibleActionsTests
    {
        [Fact]
        public void PossibleActionsForEmptyBoardShouldBeEmpty()
        {
            CheckersState state = new CheckersState(CheckersStateTests.emptyBoard, Player.White);
            var possibleActions = PossibleActions(state);
            Assert.Empty(possibleActions);
        }
    }
}

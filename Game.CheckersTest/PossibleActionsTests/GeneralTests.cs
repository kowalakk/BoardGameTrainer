namespace Game.Checkers.Tests
{
    public class GeneralTests
    {
        readonly Checkers checkers = new();
        [Fact]
        public void PossibleActionsForEmptyBoardShouldBeEmpty()
        {
            CheckersState state = CheckersState.GetEmptyBoardState();
            var possibleActions = checkers.PossibleActions(state);
            Assert.Empty(possibleActions);
        }
    }
}

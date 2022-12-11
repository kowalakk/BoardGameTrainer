using Game.Othello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Othello.Tests
{
    public class OthelloActionTests
    {
        [Fact]
        void EqualsWithEmptyActionShouldBeFalse()
        {
            OthelloAction action = new OthelloFullAction((0, 1), OthelloState.Field.White);
            Assert.False(action.Equals(new OthelloEmptyAction()));
        }

        [Fact]
        void EqualsTwoIdenticalActionsShouldBeTrue()
        {
            OthelloAction action1 = new OthelloFullAction((0, 1), OthelloState.Field.White);
            OthelloAction action2 = new OthelloFullAction((0, 1), OthelloState.Field.White);
            Assert.True(action1.Equals(action2));
        }

        [Fact]
        void EqualsOfActionsWithDifferentFieldContentShouldBeFalse()
        {
            OthelloAction action1 = new OthelloFullAction((1, 3), OthelloState.Field.White);
            OthelloAction action2 = new OthelloFullAction((1, 3), OthelloState.Field.Black);
            Assert.False(action1.Equals(action2));
        }

        [Fact]
        void EqualsOfActionsWithDifferentPositionsShouldBeFalse()
        {
            OthelloAction action1 = new OthelloFullAction((0, 1), OthelloState.Field.White);
            OthelloAction action2 = new OthelloFullAction((0, 4), OthelloState.Field.White);
            Assert.False(action1.Equals(action2));
        }

        [Fact]
        void CreationOfActionWithInvalidPositionShouldThrowAnException()
        {
            Assert.Throws<ArgumentException>(() => new OthelloFullAction((1, 8), OthelloState.Field.Black));
            Assert.Throws<ArgumentException>(() => new OthelloFullAction((-1, 3), OthelloState.Field.Black));
        }

        [Fact]
        void CreationOfActionWithCorrectArgumentsShouldBeSuccessful()
        {
            Assert.NotNull(new OthelloFullAction((1, 4), OthelloState.Field.Black));
        }
    }
}

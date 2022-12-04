using Game.Othello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OthelloTests
{
    public class OthelloActionTests
    {
        [Fact]
        void EqualsWithNullShouldBeFalse()
        {
            OthelloAction action = new OthelloAction((0, 1), OthelloState.Field.White);
            Assert.False(action.Equals(null));
        }

        [Fact]
        void EqualsTwoIdenticalActionsShouldBeTrue()
        {
            OthelloAction action1 = new OthelloAction((0, 1), OthelloState.Field.White);
            OthelloAction action2 = new OthelloAction((0, 1), OthelloState.Field.White);
            Assert.True(action1.Equals(action2));
        }

        [Fact]
        void EqualsOfActionsWithDifferentFieldContentShouldBeFalse()
        {
            OthelloAction action1 = new OthelloAction((1, 3), OthelloState.Field.White);
            OthelloAction action2 = new OthelloAction((1, 3), OthelloState.Field.Black);
            Assert.False(action1.Equals(action2));
        }

        [Fact]
        void EqualsOfActionsWithDifferentPositionsShouldBeFalse()
        {
            OthelloAction action1 = new OthelloAction((0, 1), OthelloState.Field.White);
            OthelloAction action2 = new OthelloAction((0, 4), OthelloState.Field.White);
            Assert.False(action1.Equals(action2));
        }

        [Fact]
        void CreationOfActionWithInvalidPositionShouldThrowAnException()
        {
            Assert.Throws<ArgumentException>(() => new OthelloAction((1, 8), OthelloState.Field.Black));
            Assert.Throws<ArgumentException>(() => new OthelloAction((-1, 3), OthelloState.Field.Black));
        }

        [Fact]
        void CreationOfActionWithCorrectArgumentsShouldBeSuccessful()
        {
            Assert.NotNull(new OthelloAction((1, 4), OthelloState.Field.Black));
        }
    }
}

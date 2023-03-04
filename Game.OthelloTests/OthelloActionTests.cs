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
            IOthelloAction action = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 0, 0, 3);
            Assert.NotEqual(action, new OthelloEmptyAction());
        }

        [Fact]
        void EqualsTwoIdenticalActionsShouldBeTrue()
        {
            IOthelloAction action1 = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 2, 0, 0);
            IOthelloAction action2 = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 2, 0, 0);
            Assert.Equal(action1, action2);
        }

        [Fact]
        void EqualsOfActionsWithDifferentFieldContentShouldBeFalse()
        {
            IOthelloAction action1 = new OthelloFullAction((1, 3), OthelloState.Field.White, 1, 1, 0, 4);
            IOthelloAction action2 = new OthelloFullAction((1, 3), OthelloState.Field.Black, 1, 1, 0, 4);
            Assert.NotEqual(action1, action2);
        }

        [Fact]
        void EqualsOfActionsWithDifferentPositionsShouldBeFalse()
        {
            IOthelloAction action1 = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 1, 0, 4);
            IOthelloAction action2 = new OthelloFullAction((0, 4), OthelloState.Field.White, 1, 1, 0, 4);
            Assert.NotEqual(action1, action2);
        }

        [Fact]
        void CreationOfActionWithCorrectArgumentsShouldBeSuccessful()
        {
            Assert.NotNull(new OthelloFullAction((1, 4), OthelloState.Field.Black, 1, 1, 0, 4));
        }
    }
}

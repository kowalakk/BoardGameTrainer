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
            OthelloAction action = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 0, 0, 3);
            Assert.NotEqual(action, new OthelloEmptyAction());
        }

        [Fact]
        void EqualsTwoIdenticalActionsShouldBeTrue()
        {
            OthelloAction action1 = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 2, 0, 0);
            OthelloAction action2 = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 2, 0, 0);
            Assert.Equal(action1, action2);
        }

        [Fact]
        void EqualsOfActionsWithDifferentFieldContentShouldBeFalse()
        {
            OthelloAction action1 = new OthelloFullAction((1, 3), OthelloState.Field.White, 1, 1, 0, 4);
            OthelloAction action2 = new OthelloFullAction((1, 3), OthelloState.Field.Black, 1, 1, 0, 4);
            Assert.NotEqual(action1, action2);
        }

        [Fact]
        void EqualsOfActionsWithDifferentPositionsShouldBeFalse()
        {
            OthelloAction action1 = new OthelloFullAction((0, 1), OthelloState.Field.White, 1, 1, 0, 4);
            OthelloAction action2 = new OthelloFullAction((0, 4), OthelloState.Field.White, 1, 1, 0, 4);
            Assert.NotEqual(action1, action2);
        }

        [Fact]
        void CreationOfActionWithCorrectArgumentsShouldBeSuccessful()
        {
            Assert.NotNull(new OthelloFullAction((1, 4), OthelloState.Field.Black, 1, 1, 0, 4));
        }
    }
}

using ArtificialInteligence;
using Game.IGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialIntelligence
{
    internal class NMCS<Action, State, InputState> : IArtificialIntelligence<Action, State, InputState, NMCSModuleData<Action, State>>
        where Action : IEquatable<Action>
        where State : IEquatable<State>
    {
        public Action ChooseMove(IGame<Action, State, InputState> game, State state)
        {
            throw new NotImplementedException();
        }

        public List<(Action, double)> MoveAssessment(IGame<Action, State, InputState> game, State state, ArtificialIntelligence.NMCSModuleData<Action, State> moduleData, IStopCondition condition)
        {
            throw new NotImplementedException();
        }

        void NMCSearch(Node<Action, State> root, int level, int timeInterval)
        {
            throw new NotImplementedException();
        }
        List<Action> Nested(Node<Action, State> node, int level)
        {
            throw new NotImplementedException();
        }
    }
}

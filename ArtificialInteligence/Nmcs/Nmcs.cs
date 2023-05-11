using Game.IGame;

namespace Ai
{
    internal class Nmcs<Action, State, InputState> : IAi<Action, State, InputState>
    {
        private static Dictionary<Player, Func<double, double, double>> TurnFunction = new()
        {
            { Player.One, double.Min },
            { Player.Two, double.Max },
        };
        private IStopCondition StopCondition { get; set; }
        private int Nesting { get; }
        private IGame<Action, State, InputState> Game { get; }

        public Nmcs(int nesting, IGame<Action, State, InputState> game, IStopCondition condition)
        {
            Nesting = nesting;
            Game = game;
            StopCondition = condition;
        }
        public List<(Action, double)> MoveAssessment(GameTree<Action, State> gameTree)
        {
            throw new NotImplementedException();
        }

        public Action ChooseAction(GameTree<Action, State> gameTree)
        {
            throw new NotImplementedException();
        }

        void NmcSearch(Node<Action, State> root, int level, int timeInterval)
        {
            var watch = new System.Diagnostics.Stopwatch();
            int iterations = 0;
            watch.Start();

            while (!StopCondition.StopConditionOccured())
            {
                Nested(Nesting, root, 0, 1.0);
                iterations++;
            }

            watch.Stop();
            Console.WriteLine($"UCT search execution time: {watch.ElapsedMilliseconds} ms" +
                $" - {(double)watch.ElapsedMilliseconds / iterations} ms/iteration");
        }

        double Nested(int nesting, Node<Action, State> node, int depth, double bound)
        {
            while (Game.Result(node.CorespondingState) == GameResult.InProgress)
            {
                Node<Action, State> bestChild = node.ExpandedChildren.RandomElement();//TODO


            }
            throw new NotImplementedException();
        }

        public void MoveGameToNextState(GameTree<Action, State> gameTree, Action action)
        {
            throw new NotImplementedException();
        }

        public void MoveGameToPreviousState(GameTree<Action, State> gameTree, Action action)
        {
            throw new NotImplementedException();
        }
    }
}

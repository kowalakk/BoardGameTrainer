﻿using Game.IGame;

namespace Game.Checkers
{
    public class CheckersManagerFactory : IGameManagerFactory
    {
        public IGameManager CreateGameManager(IAiFactory aiFactory, IStopCondition stopCondition)
        {
            return new GameManager<ICheckersAction, CheckersState, ICheckersInputState>(
                new Checkers(), 
                aiFactory,
                stopCondition);
        }
    }
}

﻿using Game.IGame;

namespace Game.Othello
{
    public class OthelloManagerFactory : IGameManagerFactory
    {
        public IGameManager Create(IAiFactory aiFactory, IStopCondition stopCondition)
        {
            return new GameManager<IOthelloAction, OthelloState, LanguageExt.Unit>(new Othello(), aiFactory, stopCondition);
        }
    }
}

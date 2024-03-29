﻿using Game.IGame;

namespace Game.Othello
{
    public class OthelloManagerFactory : IGameManagerFactory
    {
        public string Name => "Othello";

        public IGameManager Create(
            IAiFactory aiFactory,
            IStopCondition stopCondition,
            Dictionary<Player, bool> humanPlayers,
            Dictionary<Player, bool> showHints)
        {
            return new GameManager<IOthelloAction, OthelloState, LanguageExt.Unit>(
                new Othello(),
                aiFactory,
                stopCondition,
                humanPlayers,
                showHints);
        }
    }
}

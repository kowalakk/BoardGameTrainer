﻿using Cairo;

namespace Game.IGame
{
    public interface IGameManager
    {
        public void DrawBoard(Context context, (int, int) numberOfHints);
        public GameResult HandleInput(double x, double y, bool isPlayer2Ai);
    }
}
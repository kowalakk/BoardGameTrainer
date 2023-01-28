﻿using Ai;
using Game.Checkers;
using Game.IGame;
using Game.Othello;
using Gtk;

namespace BoardGameTrainer
{
    public class GameTrainerApplication : Application
    {
        public IGameManager gameManager = new DefaultGameManager(GameResult.InProgress);
        public string[] games = new string[] { "Checkers", "Othello" };
        public int gameNum = -1;
        public bool showHintsForPlayer1 = true;
        public bool showHintsForPlayer2 = false;
        public bool isPlayer2Ai = true;
        public bool isAImoduleOne = true;
        public double computationTime;
        public int iterationsNumber = 1000;

        public GameTrainerApplication() : base("x.y.z", GLib.ApplicationFlags.None)
        {
            this.Register(GLib.Cancellable.Current);

            MainWindow mainWindow = new(this);
            mainWindow.Show();
            mainWindow.DeleteEvent += (sender, args) => Quit();

            this.AddWindow(mainWindow);
        }

        public void CreateNewGame()
        {
            if (gameNum == 0)
            {
                gameManager = new CheckersManagerFactory()
                    .CreateGameManager(new UctFactory(1.41), new IterationStopCondition(iterationsNumber));
            }
            if (gameNum == 1)
            {
                gameManager = new OthelloManagerFactory()
                    .CreateGameManager(new UctFactory(1.41), new IterationStopCondition(iterationsNumber));
            }
        }
    }
}
﻿using Ai;
using Game.IGame;
using Game.Othello;
using Gtk;

namespace BoardGameTrainer
{
    internal class ConfigWindow : Window
    {
        public Dictionary<string, IGameManagerFactory> GameFactories { get; } = new()
        {
            { "Othello", new OthelloManagerFactory() }
        };

        private static readonly double uctConstant = 1.41;
        private static readonly int nmcsDepth = 5;
        private static readonly Dictionary<string, IAiFactory> aiFactories = new()
        {
            { "Upper Confidence Bounds for Trees", new UctFactory(uctConstant) },
            { "Nested Monte Carlo Search", new NmcsFactory(nmcsDepth) }
        };
        private static readonly Dictionary<string, IStopConditionFactory> stopConditions = new()
        {
            { "Time limit", new TimeStopConditionFactory() },
            { "Iterations limit", new IterationStopConditionFactory() }
        };
        private static readonly Dictionary<Player, bool> humanPlayers = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };
        private static readonly Dictionary<Player, bool> showHints = new()
        {
            { Player.One, true },
            { Player.Two, true }
        };

        private readonly DropDownFrame<IGameManagerFactory> gameFrame;

        private IGameManagerFactory? currentManagerFactory = null;
        private IAiFactory currentAiFactory = aiFactories.FirstOrDefault().Value;
        private IStopConditionFactory currentStopConditionFactory = stopConditions.FirstOrDefault().Value;
        private int stopConditionParam = 1000;

        public ConfigWindow(MainWindow mainWindow) : base(WindowType.Toplevel)
        {
            currentManagerFactory = GameFactories.FirstOrDefault().Value;

            gameFrame = new("Game", GameFactories);
            gameFrame.Changed += (sender, args) => { currentManagerFactory = gameFrame.Active; };
            gameFrame.Show();

            DropDownFrame<IAiFactory> aiFrame = new("AI Module", aiFactories);
            aiFrame.Changed += (sender, args) => { currentAiFactory = aiFrame.Active; };
            aiFrame.Show();

            PlayersBox playersBox = new(humanPlayers, showHints);
            playersBox.Show();

            StopConditionFrame stopConditionFrame = new(stopConditions);
            stopConditionFrame.Changed += (sender, args) => { currentStopConditionFactory = stopConditionFrame.Active; };
            stopConditionFrame.ParamChanged += (sender, args) => { stopConditionParam = stopConditionFrame.Param; };
            stopConditionFrame.Show();

            Button newGameButton = new("Start new game");
            newGameButton.Clicked += (sender, args) =>
            {
                if (currentManagerFactory is not null)
                {
                    mainWindow.InitiateNewGame(
                        currentManagerFactory, 
                        currentAiFactory, 
                        currentStopConditionFactory.Create(stopConditionParam),
                        new Dictionary<Player, bool>(humanPlayers),
                        new Dictionary<Player, bool>(showHints));
                    Hide();
                }
            };
            newGameButton.Show();

            VBox contentVbox = new()
            {
                MarginStart = MainWindow.CustomMargin,
                MarginEnd = MainWindow.CustomMargin
            };
            contentVbox.PackStart(gameFrame, false, false, MainWindow.CustomMargin);
            contentVbox.PackStart(aiFrame, false, false, MainWindow.CustomMargin);
            contentVbox.PackStart(playersBox, false, false, MainWindow.CustomMargin);
            contentVbox.PackStart(stopConditionFrame, false, false, MainWindow.CustomMargin);
            contentVbox.PackStart(newGameButton, false, false, MainWindow.CustomMargin);
            contentVbox.Show();
            Add(contentVbox);

            DeleteEvent += (sender, args) =>
            {
                args.RetVal = true; // prevents closing
                Hide();
            };
        }
        public void UpdateGameFactoryDict(IGameManagerFactory factory)
        {
            GameFactories.Add(factory.Name, factory);
            gameFrame.AddEntry(factory.Name);
        }
    }

}

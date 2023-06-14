﻿using Cairo;
using Game.IGame;
using Gdk;
using Gtk;
using System.Collections.Concurrent;
using Action = System.Action;

namespace BoardGameTrainer
{
    enum WindowState
    {
        Idle, ProcessMovement, ComputeHints
    }
    internal class MainWindow : Gtk.Window
    {
        CancellationTokenSource tokenSource;
        Thread handleEventThread;
        BlockingCollection<Action> eventsQueue = new(new ConcurrentQueue<Action>());
        WindowState windowState;
        DrawingArea boardImage;
        GameTrainerApplication application;
        double x;
        double y;

        public MainWindow(GameTrainerApplication application) : base(Gtk.WindowType.Toplevel)
        {
            this.application = application;
            DefaultSize = new Gdk.Size(700, 500);

            tokenSource = new CancellationTokenSource();

            handleEventThread = new Thread(new ThreadStart(ThreadHandleEvents));
            handleEventThread.Start();

            Button newGameButton = new("New Game");
            newGameButton.Clicked += (s, e) =>
            {
                application.AddWindow(new ConfigWindow(application));
                windowState = WindowState.Idle;
            };
            newGameButton.Show();

            Button restartButton = new("Restart");
            restartButton.Clicked += (s, e) =>
            {
                windowState = WindowState.Idle;
                application.RestartGame();
            };
            restartButton.Show();

            HBox panelHbox = new();
            panelHbox.PackStart(newGameButton, false, false, 0);
            panelHbox.PackStart(restartButton, false, false, 0);
            panelHbox.Show();

            boardImage = new DrawingArea();
            boardImage.Drawn += (sender, args) =>
            {
                Context context = args.Cr;

                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                context.Translate(xOffset, yOffset);
                context.Scale(minDimention, minDimention);

                application.GameManager?.DrawBoard(context, application.ShowHints, application.NumberOfHints);

            };
            boardImage.AddEvents((int)EventMask.ButtonPressMask);
            boardImage.ButtonPressEvent += BoardImageClickHandler;
            boardImage.Show();

            HBox contentHBox = new();
            contentHBox.PackStart(boardImage, true, true, 0);
            contentHBox.Show();

            Label mainTitle = new("New Game");
            mainTitle.Show();

            VBox titleAndContentVBox = new();
            titleAndContentVBox.PackStart(mainTitle, false, false, 0);
            titleAndContentVBox.PackStart(contentHBox, true, true, 0);
            titleAndContentVBox.Show();

            VBox mainVBox = new();
            mainVBox.PackStart(panelHbox, false, false, 0);
            mainVBox.PackStart(titleAndContentVBox, true, true, 0);
            mainVBox.Show();
            Add(mainVBox);
        }

        private void BoardImageClickHandler(object sender, ButtonPressEventArgs args)
        {
            if (windowState != WindowState.ProcessMovement)
            {
                windowState = WindowState.ProcessMovement;
                int minDimention = Math.Min(boardImage.AllocatedWidth, boardImage.AllocatedHeight);
                int xOffset = (boardImage.AllocatedWidth - minDimention) / 2;
                int yOffset = (boardImage.AllocatedHeight - minDimention) / 2;
                x = (args.Event.X - xOffset) / minDimention;
                y = (args.Event.Y - yOffset) / minDimention;

                eventsQueue.Add(PerformMovement);
            }
        }

        private void PerformMovement()
        {
            //Player player = application.GameManager!.CurrentPlayer();
            //if (application.HumanPlayers[player])
            //{
            CancellationToken token = ResetToken();
            (GameResult gameResult, bool isActionPerformed) = application.GameManager!.HandleMovement(x, y);
            Gtk.Application.Invoke(delegate
            {
                boardImage.QueueDraw();
            });
            Player opponent = application.GameManager!.CurrentPlayer();
            if (!application.HumanPlayers[opponent])
            {
                PerformAiMovement();
            }
            else if (isActionPerformed)
            {
                windowState = WindowState.ComputeHints;
                eventsQueue.Add(ComputeHints);
            }
            else
            {
                windowState = WindowState.Idle;
            }
            Gtk.Application.Invoke(delegate
            {
                boardImage.QueueDraw();
            });
        }
        //else
        //{
        private void PerformAiMovement()
        {
            GameResult gameResult = application.GameManager!.HandleAiMovement();
            Player opponent = application.GameManager!.CurrentPlayer();
            if (!application.HumanPlayers[opponent])
            {
                PerformAiMovement();
            }
            else if (gameResult == GameResult.InProgress)
            {
                windowState = WindowState.ComputeHints;
                eventsQueue.Add(ComputeHints);
            }
            else
            {
                windowState = WindowState.Idle;
            }
            Gtk.Application.Invoke(delegate
            {
                boardImage.QueueDraw();
            });
            //}
        }

        private void ComputeHints()
        {
            CancellationToken token = ResetToken();
            application.GameManager!.ComputeHints(token);
            Gtk.Application.Invoke(delegate
            {
                boardImage.QueueDraw();
                windowState = WindowState.Idle;
            });
        }

        private void ThreadHandleEvents()
        {
            Action job;
            while (true)
            {
                job = eventsQueue.Take();
                job();
            }
        }

        private CancellationToken ResetToken()
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }
    }
}

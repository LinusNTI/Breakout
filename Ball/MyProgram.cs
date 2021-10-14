using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Timers;

namespace Ball
{
    class MyProgram
    {
        public PointF ballPos = new PointF(0,0);
        public PointF ballVel = new PointF(1, 1.23f);

        public PointF oldBallPos = new PointF(0,0);
        public int oldPlaneX = Console.WindowWidth/2;

        public int updateRate = 25;

        public BlockbreakerGame _game = new BlockbreakerGame();

        private void UpdateFlipper(int x)
        {
            Console.SetCursorPosition(oldPlaneX - 2, _game.gameArea.Height - _game.gameArea.Y + 1);
            Console.Write("     ");
            Console.SetCursorPosition(x-2, _game.gameArea.Height - _game.gameArea.Y + 1);
            Console.Write("-----");

            oldPlaneX = x;
        }

        private void Render()
        {
            Console.SetCursorPosition((int)Math.Floor(oldBallPos.X), (int)Math.Floor(oldBallPos.Y));
            Console.Write(" ");
            Console.SetCursorPosition((int)Math.Floor(ballPos.X), (int)Math.Floor(ballPos.Y));
            Console.Write("@");

            UpdateFlipper((int)Math.Floor(ballPos.X));

            Console.SetCursorPosition(0, 0);
        }

        private void Update()
        {
            #region Compute physics
            Point endPoint = new Point(_game.gameArea.Width + _game.gameArea.X,
                                        _game.gameArea.Height + _game.gameArea.Y);

            //Reverse velocity if the ball is about to hit a wall/border
            if (Math.Floor(ballPos.X + ballVel.X) >= endPoint.X-1 || 
                Math.Floor(ballPos.X + ballVel.X) <= _game.gameArea.X)
            {
                ballVel.X *= -1;
            }
            if (Math.Floor(ballPos.Y + ballVel.Y) >= endPoint.Y || 
                Math.Floor(ballPos.Y + ballVel.Y) <= _game.gameArea.Y)
            {
                ballVel.Y *= -1;
            }

            if (_game.isPointBreakable(new PointF(ballPos.X + ballVel.X, ballPos.Y + ballVel.Y)))
            {
                _game.BreakBlock(new Point((int)Math.Floor(ballPos.X + ballVel.X), (int)Math.Floor(ballPos.Y + ballVel.Y)));
                ballVel.Y *= -1;
            }

            oldBallPos = ballPos;
            ballPos = new PointF(ballPos.X + ballVel.X, ballPos.Y + ballVel.Y);
            #endregion

            Render();
        }

        private void Start()
        {
            _game.gameArea = new Rectangle(3, 1, Console.WindowWidth - 7, Console.WindowHeight - 5);
            _game.updateRate = 1;

            _game.CreateBorder();
            _game.CreateBreakables(new Rectangle(4, 2, Console.WindowWidth - 9, 7));
        }

        public void Run()
        {
            Start();
            _game.ValidateAndCorrect();
            
            ballPos = new Point(Console.WindowWidth/2, Console.WindowHeight-20);

            bool runGame = true;
            while (runGame)
            {
                Update();
                Thread.Sleep(_game.updateRate);
            }
        }
    }
}

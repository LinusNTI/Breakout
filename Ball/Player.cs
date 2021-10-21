using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ball
{
    class Player
    {
        #region Vars
        public Rectangle gameArea;

        private Rectangle breakablesArea;
        private bool[,] breakables = { };

        #region Player Vars
        public PointF ballPos = new PointF(0, 0);
        public PointF ballVel = new PointF(1, 1.23f);

        public PointF oldBallPos = new PointF(0, 0);
        public int oldPlaneX = Console.WindowWidth / 2;
        #endregion

        #region Properties
        private PointF StartPoint;
        public PointF startPoint 
        {
            get { return StartPoint; }
            set {
                if (value.X < gameArea.X || value.Y < gameArea.Y)
                {
                    StartPoint = new Point(gameArea.X, gameArea.Y);
                }
                else
                {
                    StartPoint = value;
                }
            }
        }
        #endregion
        #endregion

        #region General arena stuff
        public void ValidateAndCorrect()
        {
            if (startPoint.X < gameArea.X || startPoint.Y < gameArea.Y)
            {
                startPoint = new Point(gameArea.X, gameArea.Y);
            }
        }

        public void CreateBorder()
        {
            #region FML I hate hardcoding like this

            #region Create side border
            for (int i = 0; i < gameArea.Height-1; i++)
            {
                Console.SetCursorPosition(gameArea.X, gameArea.Y + i);
                Console.Write("#");
                Console.SetCursorPosition(gameArea.X + gameArea.Width - 1, gameArea.Y + i);
                Console.Write("#");
            }
            #endregion

            #region Create top border
            for (int i = 0; i < gameArea.Width; i++)
            {
                Console.SetCursorPosition(gameArea.X + i, gameArea.Y);
                Console.Write("#");
            }
            #endregion

            #endregion
        }

        public void CreateBreakables(Rectangle area)
        {
            breakablesArea = area;

            breakables = new bool[area.Height, area.Width];

            Console.SetCursorPosition(area.X, area.Y);
            for (int y = 0; y < area.Height; y++) 
            {
                for (int x = 0; x < area.Width; x++)
                {
                    Console.Write("X");
                    breakables[y, x] = true;
                }
                Console.SetCursorPosition(area.X, area.Y + y);
            }
        }

        public bool isPointBreakable(PointF ballPos) 
        {
            return false;
            if (ballPos.X < breakablesArea.X ||
                ballPos.X > breakablesArea.X + breakablesArea.Width)
                return false;
            if (ballPos.Y < breakablesArea.Y ||
                ballPos.Y > breakablesArea.Y + breakablesArea.Height)
                return false;
            int Y = (int)Math.Floor(ballPos.Y); //Fuck
            int X = (int)Math.Floor(ballPos.X); //My
            return breakables[Y-2, X-4]; //Life
        }

        public bool BreakBlock(Point blockPos)
        {
            breakables[blockPos.Y - 2, blockPos.X - 4] = false;
            Console.SetCursorPosition(blockPos.X, blockPos.Y);
            Console.Write(" ");
            return true;
        }
        #endregion

        #region Player methods
        private void Physics()
        {
            #region Compute physics
            Point endPoint = new Point(gameArea.Width + gameArea.X,
                                        gameArea.Height + gameArea.Y);

            //Reverse velocity if the ball is about to hit a wall/border
            if (Math.Floor(ballPos.X + ballVel.X) >= endPoint.X - 1 ||
                Math.Floor(ballPos.X + ballVel.X) <= gameArea.X)
            {
                ballVel.X *= -1;
            }
            if (Math.Floor(ballPos.Y + ballVel.Y) >= endPoint.Y ||
                Math.Floor(ballPos.Y + ballVel.Y) <= gameArea.Y)
            {
                ballVel.Y *= -1;
            }

            if (isPointBreakable(new PointF(ballPos.X + ballVel.X, ballPos.Y + ballVel.Y)))
            {
                BreakBlock(new Point((int)Math.Floor(ballPos.X + ballVel.X), (int)Math.Floor(ballPos.Y + ballVel.Y)));
                ballVel.Y *= -1;
            }

            oldBallPos = ballPos;
            ballPos = new PointF(ballPos.X + ballVel.X, ballPos.Y + ballVel.Y);
            #endregion
        }

        private void UpdateFlipper(int x)
        {
            Console.SetCursorPosition(oldPlaneX - 2, gameArea.Height - gameArea.Y + 1);
            Console.Write("     ");
            Console.SetCursorPosition(x - 2, gameArea.Height - gameArea.Y + 1);
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
        #endregion

        #region General methods

        #region Constructors
        public Player(Rectangle arenaSize, Rectangle relativeBreakablesArea, PointF startPos, PointF startVel, int planeStartX) //Define default constructor for class
        {
            gameArea = arenaSize;
            startPoint = new PointF(startPos.X + arenaSize.X, startPos.Y + arenaSize.Y);
            ballVel = startVel;
            oldPlaneX = planeStartX + arenaSize.X;

            Start();
            CreateBreakables( new Rectangle(
                    relativeBreakablesArea.X + arenaSize.X + 1,
                    relativeBreakablesArea.Y + arenaSize.Y + 1,
                    Math.Clamp(relativeBreakablesArea.Width, 1, arenaSize.Width-1),
                    Math.Clamp(relativeBreakablesArea.Height, 1, arenaSize.Height-1)
                )
            );
        }
        #endregion

        public void Update()
        {
            Physics();
            Render();
        }

        public void Start()
        {
            ballPos = startPoint;

            CreateBorder();
            ValidateAndCorrect();
        }
        #endregion
    }
}

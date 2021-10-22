using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ball
{
    struct Brick
    {
        public short x, y; //Use a short to conserve memory (like it is an issue to begin with)
        public bool isBroken;
    }

    class Player
    {
        #region Vars
        public Rectangle gameArea;

        private Rectangle breakablesArea;
        private Brick[] breakables = { };

        private long lastExecutedPhysics = 0;
        private int updateRate = 25;

        public bool isDead = false;

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
            for (int i = 0; i < gameArea.Height - 1; i++)
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

            breakables = new Brick[area.Height * area.Width];

            Console.SetCursorPosition(area.X, area.Y);
            for (int y = 0; y < area.Height; y++)
            {
                for (int x = 0; x < area.Width; x++)
                {
                    Console.Write("X");
                    breakables[y * x] = new Brick() { x = (short)(x + area.X), y = (short)(y + area.Y), isBroken = false };
                }
                Console.SetCursorPosition(area.X, area.Y + y);
            }
        }

        private Brick? FindBrick(Point pos)
        {
            for (int i = 0; i < breakables.Length; i++)
            {
                if (breakables[i].x == pos.X && breakables[i].y == pos.Y)
                {
                    return breakables[i];
                }
            }
            return null;
        }

        public bool isPointBreakable(PointF ballPos)
        {
            if (Math.Floor(ballPos.X) < breakablesArea.X ||
                Math.Floor(ballPos.X) > breakablesArea.X + breakablesArea.Width)
            {
                return false;
            }
                
            if (Math.Floor(ballPos.Y) < breakablesArea.Y ||
                Math.Floor(ballPos.Y) > breakablesArea.Y + breakablesArea.Height)
            {
                return false;
            }

            Brick? foundBrick = FindBrick(new Point((int)Math.Floor(ballPos.X), (int)Math.Floor(ballPos.Y)));

            if (foundBrick == null)
            {
                return false;
            }

            //Return at the relative position of the ball
            return ((Brick)foundBrick).isBroken;
        }

        public bool BreakBlock(Point blockPos)
        {
            bool foundBlock = false;
            for (int i = 0; i < breakables.Length; i++)
            {
                if (breakables[i].x == blockPos.X && breakables[i].y == blockPos.Y)
                {
                    breakables[i].isBroken = true;
                    foundBlock = true;
                }
            }

            if (!foundBlock)
            {
                return false;
            }
            
            Console.SetCursorPosition(blockPos.X, blockPos.Y);
            Console.Write(" ");
            return true;
        }
        #endregion

        #region Player methods
        private void Physics()
        {
            Point endPoint = new Point(gameArea.Width + gameArea.X,
                                        gameArea.Height + gameArea.Y - 1);

            //Reverse velocity if the ball is about to hit a wall/border
            if (Math.Floor(ballPos.X + ballVel.X) >= endPoint.X - 1 ||
                Math.Floor(ballPos.X + ballVel.X) <= gameArea.X)
            {
                ballVel.X *= -1;
            }
            if (Math.Floor(ballPos.Y + ballVel.Y) >= endPoint.Y)
            {
                var s = Math.Abs((ballPos.X + ballVel.X) - oldPlaneX);
                if (s > 2)
                {
                    //Die
                    isDead = true;
                }
                ballVel.Y *= -1;
            }
            if (Math.Floor(ballPos.Y + ballVel.Y) <= gameArea.Y)
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
        }

        private void UpdateFlipper(int x)
        {
            Console.SetCursorPosition(oldPlaneX - 2, gameArea.Height - gameArea.Y + 1);
            Console.Write("     ");
            Console.SetCursorPosition(x - 2, gameArea.Height - gameArea.Y + 1);
            Console.Write("-----");

            oldPlaneX = x;
        }

        public void MoveFlipper(int x)
        {
            if (oldPlaneX - x + 2 < gameArea.X ||
                oldPlaneX - x + 2 > gameArea.X + gameArea.Width)
            {
                return;
            }
            UpdateFlipper(oldPlaneX + x);
        }

        public void Render()
        {
            Console.SetCursorPosition((int)Math.Floor(oldBallPos.X), (int)Math.Floor(oldBallPos.Y));
            Console.Write(" ");
            Console.SetCursorPosition((int)Math.Floor(ballPos.X), (int)Math.Floor(ballPos.Y));
            Console.Write("@");

            //UpdateFlipper((int)Math.Floor(ballPos.X));

            Console.SetCursorPosition(0, 0);
        }
        #endregion

        #region General methods

        #region Constructors
        public Player(Rectangle arenaSize, Rectangle relativeBreakablesArea, PointF startPos, PointF startVel, int planeStartX, int updateRate) //Define default constructor for class
        {
            gameArea = arenaSize;
            startPoint = new PointF(startPos.X + arenaSize.X, startPos.Y + arenaSize.Y);
            ballVel = startVel;
            oldPlaneX = planeStartX + arenaSize.X;
            this.updateRate = updateRate;

            Start();
            CreateBreakables(new Rectangle(
                    relativeBreakablesArea.X + arenaSize.X + 1,
                    relativeBreakablesArea.Y + arenaSize.Y + 1,
                    Math.Clamp(relativeBreakablesArea.Width, 1, arenaSize.Width - 2),
                    Math.Clamp(relativeBreakablesArea.Height, 1, arenaSize.Height - 2)
                )
            );
        }
        #endregion

        public void Update()
        {
            if (lastExecutedPhysics + updateRate <= Environment.TickCount64 && !isDead)
            {
                Physics();
                lastExecutedPhysics = Environment.TickCount64 + updateRate;
            }
                
            if (!isDead)
            {
                Render();
            }
        }

        public void Start()
        {
            ballPos = startPoint;

            CreateBorder();
            ValidateAndCorrect();
            UpdateFlipper(oldPlaneX);
        }
        #endregion
    }
}

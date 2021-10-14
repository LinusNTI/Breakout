using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ball
{
    class BlockbreakerGame
    {
        public Rectangle gameArea;
        private Rectangle breakablesArea;

        private bool[,] breakables = { };

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

        public int updateRate = 25;

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
    }
}

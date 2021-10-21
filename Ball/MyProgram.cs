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
        public List<Player> _players = new List<Player>();
        public int updateRate = 25;

        public void Run()
        {
            _players.Add(new Player(
                new Rectangle(1, 1, 30, 10), //Arena size
                new Rectangle(0, 0, 2, 1),   //Breakable area (RELATIVE TO ARENA SIZE)
                new PointF(15, 5),           //Start position
                new PointF(-1f, -1.25f),     //Ball velocity
                5                            //Plane start position
            ));

            _players.Add(new Player(
                new Rectangle(35, 1, 30, 10),//Arena size
                new Rectangle(0, 0, 99, 1),  //Breakable area (RELATIVE TO ARENA SIZE)
                new PointF(15, 5),           //Start position
                new PointF(-1f, -1.25f),     //Ball velocity
                5                            //Plane start position
            ));

            bool runGame = true;
            while (runGame)
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    _players[i].Update();
                }
                Thread.Sleep(updateRate);
            }
        }
    }
}

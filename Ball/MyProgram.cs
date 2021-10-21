using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Timers;

namespace Ball
{
    struct PlayerControls
    {
        public ConsoleKey left;
        public ConsoleKey right;
    }

    class MyProgram
    {
        public List<Player> _players = new List<Player>();
        public int updateRate = 100;

        public PlayerControls[] _playerControls = {
            new PlayerControls() { left = ConsoleKey.A, right = ConsoleKey.D },
            new PlayerControls() { left = ConsoleKey.LeftArrow, right = ConsoleKey.RightArrow }
        };

        public void Run()
        {
            _players.Add(new Player(
                new Rectangle(1, 1, 29, 9),  //Arena size
                new Rectangle(0, 0, 99, 3),  //Breakable area (RELATIVE TO ARENA SIZE)
                new PointF(15, 5),           //Start position
                new PointF(1f, 1.25f),       //Ball velocity
                5,                           //Plane start position
                updateRate                   //Update rate for physics
            ));

            _players.Add(new Player(
                new Rectangle(34, 1, 29, 9), //Arena size
                new Rectangle(0, 0, 99, 3),  //Breakable area (RELATIVE TO ARENA SIZE)
                new PointF(15, 5),           //Start position
                new PointF(-2f, -1.5f),      //Ball velocity
                5,                           //Plane start position
                updateRate                   //Update rate for physics
            ));

            bool runGame = true;
            while (runGame)
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        if (_playerControls[i].left == key)
                        {
                            _players[i].MoveFlipper(-1);
                        }
                        if (_playerControls[i].right == key)
                        {
                            _players[i].MoveFlipper(1);
                        }
                    }
                    _players[i].Update();
                }
                Thread.Sleep(1);
            }
        }
    }
}

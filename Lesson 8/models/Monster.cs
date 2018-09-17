<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class Monster
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; }
        public int Life { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }  //used for AI movement
        private DateTime _actionStart = new DateTime(0);
        private int[] PossibleMoves = new int[] { 0, -1, 1, 0, 0, 1, -1,0  };   //4 directions +

        public bool IsAlive
        {
            get { return (Life < 1 ? true : false); }
        }

        public Monster()
        {
            Life = 1;
            Symbol = 'M';
            Speed = 5;
        }

        public void Draw()
        {
            if(_actionStart.Ticks == 0)
            {
                _actionStart = DateTime.Now;
            }
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }

        public void PerformAI(Map map)
        {
            // wait an elapsed amount of time based on movement speed
            if((DateTime.Now - _actionStart).Seconds > Speed)
            {
                Console.SetCursorPosition(0, 21);
                Console.WriteLine("Monster is on the move!");

                //look for an open space possibly towards the player
                List<int> availMoves = new List<int>();
                for(int move = 0; move<PossibleMoves.Length; move+=2)
                {
                    if (map.Tiles[Y+PossibleMoves[move + 1], X+PossibleMoves[move]].IsWalkable)
                    {
                        availMoves.Add(move);
                    }
                }
                //if we have an option to move somwhere lets do it
                if(availMoves.Count>0)
                {
                    Random randgen = new Random();
                    int move = availMoves[randgen.Next(availMoves.Count)];
                    map.Tiles[Y, X].IsWalkable = true;  //because the monster is leaving this location
                    X = X + PossibleMoves[move];
                    Y = Y + PossibleMoves[move + 1];
                    map.Tiles[Y, X].IsWalkable = false; //because the monster is now in a new location
                }
                _actionStart = DateTime.Now; //reset timer
            }

        }

    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class Monster
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; }
        public int Life { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }  //used for AI movement
        private DateTime _actionStart = new DateTime(0);
        private int[] PossibleMoves = new int[] { 0, -1, 1, 0, 0, 1, -1,0  };   //4 directions +

        public bool IsAlive
        {
            get { return (Life < 1 ? true : false); }
        }

        public Monster()
        {
            Life = 1;
            Symbol = 'M';
            Speed = 5;
        }

        public void Draw()
        {
            if(_actionStart.Ticks == 0)
            {
                _actionStart = DateTime.Now;
            }
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }

        public void PerformAI(Map map)
        {
            // wait an elapsed amount of time based on movement speed
            if((DateTime.Now - _actionStart).Seconds > Speed)
            {
                Console.SetCursorPosition(0, 21);
                Console.WriteLine("Monster is on the move!");

                //look for an open space possibly towards the player
                List<int> availMoves = new List<int>();
                for(int move = 0; move<PossibleMoves.Length; move+=2)
                {
                    if (map.Tiles[Y+PossibleMoves[move + 1], X+PossibleMoves[move]].IsWalkable)
                    {
                        availMoves.Add(move);
                    }
                }
                //if we have an option to move somwhere lets do it
                if(availMoves.Count>0)
                {
                    Random randgen = new Random();
                    int move = availMoves[randgen.Next(availMoves.Count)];
                    map.Tiles[Y, X].IsWalkable = true;  //because the monster is leaving this location
                    X = X + PossibleMoves[move];
                    Y = Y + PossibleMoves[move + 1];
                    map.Tiles[Y, X].IsWalkable = false; //because the monster is now in a new location
                }
                _actionStart = DateTime.Now; //reset timer
            }

        }

    }
}
>>>>>>> 4c4388af791c5718fe4a26b8dff267c865bff09c

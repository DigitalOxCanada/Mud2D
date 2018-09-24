using System;
using System.Collections.Generic;

namespace Mud2D.models
{
    public class Monster
    {
        public bool Dirty { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        internal int LastX { get; set; }
        internal int LastY { get; set; }
        public char Symbol { get; set; }
        public int Life { get; set; }
        public int MaxLife { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }  //used for AI movement
        private DateTime _actionStart = new DateTime(0);
        private int[] PossibleMoves = new int[] { 0, -1, 1, 0, 0, 1, -1, 0 };   //4 directions +

        public bool IsAlive
        {
            get { return (Life < 1 ? true : false); }
        }

        public Monster(MonsterDBModel monsterDBModel)
        {
            Name = monsterDBModel.Name;
            Dirty = true;
            Life = monsterDBModel.MaxLife;
            MaxLife = Life;
            Symbol = monsterDBModel.Symbol;
            Speed = monsterDBModel.Speed;
            X = -1;
            Y = -1;
            LastX = X;
            LastY = Y;
        }

        internal void MoveTo(int newX, int newY)
        {
            LastX = X;
            LastY = Y;
            X = newX;
            Y = newY;
        }

        public void Update(GameEngine theGame)
        {
            if (X < 0 || Y < 0)
            {
                return;
            }

            if (Dirty)
            {
                if (LastX > -1 && LastY > -1)
                {
                    theGame.Tiles[LastY, LastX].Draw();
                }

                if (theGame.Tiles[Y, X].FOW < 1)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write(Symbol);
                }
                Dirty = false;
            }

            if (_actionStart.Ticks == 0)
            {
                _actionStart = DateTime.Now;
            }
            // wait an elapsed amount of time based on movement speed
            if ((DateTime.Now - _actionStart).Seconds > Speed)
            {
                theGame.MessageBrd.Add($"{Name} is on the move.");

                //look for an open space possibly towards the player
                List<int> availMoves = new List<int>();
                for (int move = 0; move < PossibleMoves.Length; move += 2)
                {
                    if (theGame.Tiles[Y + PossibleMoves[move + 1], X + PossibleMoves[move]].IsWalkable)
                    {
                        availMoves.Add(move);
                    }
                }
                //if we have an option to move somwhere lets do it
                if (availMoves.Count > 0)
                {
                    Random randgen = new Random();
                    int move = availMoves[randgen.Next(availMoves.Count)];
                    MoveTo(X + PossibleMoves[move], Y + PossibleMoves[move + 1]);

                    theGame.Tiles[LastY, LastX].IsWalkable = true;  //because the monster is leaving this location
                    if (theGame.Tiles[LastY, LastX].FOW < 1)
                    {
                        theGame.Tiles[LastY, LastX].Dirty = true;
                    }

                    theGame.Tiles[Y, X].IsWalkable = false; //because the monster is now in a new location
                    if (theGame.Tiles[Y, X].FOW < 1)
                    {
                        theGame.Tiles[Y, X].Dirty = true;
                    }
                    Dirty = true;
                }
                _actionStart = DateTime.Now; //reset timer
            }

        }

    }
}

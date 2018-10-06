using System;
using System.Collections.Generic;

namespace Mud2D.models
{
    public class DirectionMod
    {
        public int x;
        public int y;
    }

    public class Monster : ObjectBase
    {
        public bool Dirty { get; set; }
        public int Life { get; set; }
        public int MaxLife { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }  //used for AI movement
        public float Damage { get; set; }
        private DateTime _actionStart = new DateTime(0);

        //private int[] PossibleMoves = new int[] { 0, -1, 1, 0, 0, 1, -1, 0 };   //4 directions +

        //different monsters may have different moves, like diagonal or more than 1 square away
        public List<DirectionMod> PossibleDirections = new List<DirectionMod>() {
            new DirectionMod() { x = 0, y = -1 },
            new DirectionMod() { x = 1, y = 0 },
            new DirectionMod() { x = 0, y = 1 },
            new DirectionMod() { x = -1, y = 0 }
        };

        public bool IsAlive
        {
            get { return (Life > 0 ? true : false); }
        }

        public Monster(MonsterDBModel monsterDBModel)
        {
            Name = monsterDBModel.Name;
            Dirty = true;
            Life = monsterDBModel.MaxLife;
            MaxLife = Life;
            Symbol = monsterDBModel.Symbol;
            Speed = monsterDBModel.Speed;
            Damage = monsterDBModel.Damage;
        }



        public void Update()
        {
            if (X < 0 || Y < 0)
            {
                return;
            }

            if (Dirty)
            {
                if (LastX > -1 && LastY > -1)
                {
                    GameEngine.Tiles[LastY, LastX].Draw();
                }

                if (GameEngine.Tiles[Y, X].FOW < 1)
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

                //get a delta between player and monster
                var dx = GameEngine.ThePlayer.X - X;
                var dy = GameEngine.ThePlayer.Y - Y;

                List<DirectionMod> availMoves = new List<DirectionMod>();
                foreach (var dir in PossibleDirections)
                {
                    if (GameEngine.Tiles[Y + dir.y, X + dir.x].IsWalkable)
                    {
                        availMoves.Add(dir);
                    }
                }

                //if we have an option to move somwhere lets do it
                if (availMoves.Count > 0)
                {
                    Random randgen = new Random();

                    //if the monster is far away then just randomize movement
                    if (Math.Abs(dx) + Math.Abs(dy) > 6)
                    {
                        //get a random move
                        DirectionMod move = availMoves[randgen.Next(availMoves.Count)];
                        //move to that location
                        MoveTo(X + move.x, Y + move.y);
                        GameEngine.MessageBrd.Add($"{Name} is on the move.");

                    }
                    else
                    {
                        //the player is close, lets move towards it
                        GameEngine.MessageBrd.Add($"{Name} picked up your scent...run!!");

                        if (dx < 0)
                        {
                            // the monster is left of the player, so move right
                            foreach (var m in availMoves)
                            {
                                if (m.x < 0)
                                {
                                    MoveTo(X + m.x, Y + m.y);
                                }
                            }
                        }
                        else if (dx > 0)
                        {
                            // the monster is right of the player, so move left
                            foreach (var m in availMoves)
                            {
                                if (m.x > 0)
                                {
                                    MoveTo(X + m.x, Y + m.y);
                                }
                            }
                        }
                        else if (dy < 0)
                        {
                            // the monster is down of the player, so move up
                            foreach (var m in availMoves)
                            {
                                if (m.y < 0)
                                {
                                    MoveTo(X + m.x, Y + m.y);
                                }
                            }
                        }
                        else if (dy>0)
                        {
                            // the monster is above of the player, so move down
                            foreach (var m in availMoves)
                            {
                                if (m.y > 0)
                                {
                                    MoveTo(X + m.x, Y + m.y);
                                }
                            }
                        }

                    }


                    GameEngine.Tiles[LastY, LastX].IsWalkable = true;  //because the monster is leaving this location
                    if (GameEngine.Tiles[LastY, LastX].FOW < 1)
                    {
                        GameEngine.Tiles[LastY, LastX].Dirty = true;
                    }

                    GameEngine.Tiles[Y, X].IsWalkable = false; //because the monster is now in a new location
                    if (GameEngine.Tiles[Y, X].FOW < 1)
                    {
                        GameEngine.Tiles[Y, X].Dirty = true;
                    }
                    Dirty = true;
                }
                _actionStart = DateTime.Now; //reset timer
            }

        }

        internal void TakeDamage(int damage)
        {
            Life -= damage;
        }
    }
}

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
        public int MoveSpeed { get; set; }  //used for AI movement
        public int AttackSpeed { get; set; }
        public float Damage { get; set; }
        private DateTime _actionStart = new DateTime(0);
        private DateTime _attackStart = new DateTime(0);

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
            MoveSpeed = monsterDBModel.MoveSpeed;
            AttackSpeed = monsterDBModel.MoveSpeed;
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

            //we do this in case the monster could be beside multiple players in the future
            List<DirectionMod> availAttacks = new List<DirectionMod>();
            foreach (var dir in PossibleDirections)
            {
                if (GameEngine.ThePlayer.X == X+dir.x && GameEngine.ThePlayer.Y == Y+dir.y)
                {
                    availAttacks.Add(dir);
                }
            }

            if (availAttacks.Count>0)
            {
                //if the attack counter hasn't started lets start it from this point in time.
                if (_attackStart.Ticks == 0)
                {
                    _attackStart = DateTime.Now;
                }
                //if enough time has elapsed we perform an attack.
                if ((DateTime.Now - _attackStart).Seconds > AttackSpeed)
                {
                    string msg = "";
                    //the monster can now attack.  determine hit or miss.
                    Random rand = new Random();
                    int roll = rand.Next(0, 100);
                    if(roll>20)
                    {
                        //hit
                        msg = $"hit for {Damage} damage.";
                        GameEngine.ThePlayer.Life-= Convert.ToInt16(Damage);    //why is damage a float?  don't remember.
                        GameEngine.ScoreCard.Dirty = true;
                    }
                    else
                    {
                        //miss
                        msg = "missed";
                    }
                    GameEngine.MessageBrd.Add($"{Name} attacked you and {msg}.");
                    _attackStart = DateTime.Now;    //reset timer after action
                }
            }
            else {

                // wait an elapsed amount of time based on movement speed
                if ((DateTime.Now - _actionStart).Seconds > MoveSpeed)
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
                            else if (dy > 0)
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

                        //LastX, LastY on some monsters are off the page and move right away causing this to crash
                        //so we just ignore LastXY if its off.
                        if (LastX > -1 && LastY > -1)
                        {
                            GameEngine.Tiles[LastY, LastX].IsWalkable = true;  //because the monster is leaving this location
                            if (GameEngine.Tiles[LastY, LastX].FOW < 1)
                            {
                                GameEngine.Tiles[LastY, LastX].Dirty = true;
                            }
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

        }

        internal void TakeDamage(int damage)
        {
            Life -= damage;
        }
    }
}

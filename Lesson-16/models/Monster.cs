using System;
using System.Collections.Generic;

namespace Mud2D.models
{
    public class DirectionMod
    {
        public int x;
        public int y;
    }

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

                //get a delta between player and monster
                var dx = theGame.ThePlayer.X - X;
                var dy = theGame.ThePlayer.Y - Y;

                List<DirectionMod> availMoves = new List<DirectionMod>();
                foreach (var dir in PossibleDirections)
                {
                    if (theGame.Tiles[Y + dir.y, X + dir.x].IsWalkable)
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
                        theGame.MessageBrd.Add($"{Name} is on the move.");

                    }
                    else
                    {
                        //the player is close, lets move towards it
                        theGame.MessageBrd.Add($"{Name} picked up your scent...run!!");

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

//        if (player.hp< 1) {
//                // game over message
//                var gameOver = game.add.text(game.world.centerX, game.world.centerY, 'Game Over\nCtrl+r to restart', { fill: '#e22', align: "center" } );
//                gameOver.anchor.setTo(0.5,0.5);
//        }
using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    enum PlayerMovement
    {
        Up = 1,
        Down,
        Left,
        Right
    }

    public class Player
    {
        public bool Dirty { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Number { get; set; } //ie: 1, 2, 3, etc.
        public string Name { get; set; }
        public int Life { get; set; }
        public int MaxLife { get; set; }
        public long Gold { get; set; }
        public int LastX { get; internal set; }
        public int LastY { get; internal set; }

        public Player()
        {
            Dirty = true;
            X = -1;
            Y = -1;
            Number = 1;
            Name = "John Doe";
            Gold = 0;
            Life = 10;
            MaxLife = Life;
            LastX = X;
            LastY = Y;
        }

        public void Update(GameEngine gameEngine)
        {
            //only erase where it has been if it was a valid space
            if (X < 0 || Y < 0)
            {
                return;
            }

            if (Dirty)
            {
                if (LastX > -1 && LastY > -1)
                {
                    Console.SetCursorPosition(LastX, LastY);
                    gameEngine.Tiles[LastY, LastX].Draw(true);  //redraw the tile we moved off of, in case its stairs or an object we didn't pick up.
                    //Console.Write(' ');     //TODO fix, this causes a problem in scenarios where the place the user was standing wasn't blank, like stairs. this will erase the stairs graphic.
                }

                Console.SetCursorPosition(X, Y);
                Console.Write(Number);
                Console.SetCursorPosition(0, 0);    //move the cursor away from player so you don't see blinking cursor
                Dirty = false;
            }
        }

    }
}

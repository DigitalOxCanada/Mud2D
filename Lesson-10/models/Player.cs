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
        public bool NeedsRedrawing { get; set; }
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
            NeedsRedrawing = true;
            X = 0;
            Y = 0;
            Number = 1;
            Name = "John Doe";
            Gold = 0;
            Life = 10;
            MaxLife = Life;
            LastX = X;
            LastY = Y;
        }

        public void Update()
        {
            if (NeedsRedrawing)
            {
                Console.SetCursorPosition(LastX, LastY);
                Console.Write(' ');

                Console.SetCursorPosition(X, Y);
                Console.Write(Number);
                Console.SetCursorPosition(0, 0);    //move the cursor away from player so you don't see blinking cursor
                NeedsRedrawing = false;
            }
        }

    }
}

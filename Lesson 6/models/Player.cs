using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Number { get; set; } //ie: 1, 2, 3, etc.
        public string Name { get; set; }

        public Player()
        {
            X = 0;
            Y = 0;
            Number = 1;
            Name = "John Doe";
        }

        public void Draw()
        {
            Console.Write(Number);
        }
    }
}

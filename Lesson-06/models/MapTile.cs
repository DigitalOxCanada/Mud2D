using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; }

        internal void Draw()
        {
            Console.Write(Symbol);
        }
    }
}

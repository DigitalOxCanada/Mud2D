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
        public bool IsWalkable { get; set; }
        public int FOW { get; set; }
        public bool Dirty { get; internal set; }

        public MapTile()
        {
            FOW = 2;
            Dirty = true;
        }

        internal void Draw(bool force = false)
        {
            if (Dirty || force)
            {
                if (FOW > 0)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write('█'); //fog
                }
                else
                {
                    Console.SetCursorPosition(X, Y);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Symbol);
                }
                Dirty = false;
            }
        }

        internal void ClearFOG()
        {
            if(FOW>0)
            {
                FOW = 0;
                Dirty = true;
            }
        }
    }
}

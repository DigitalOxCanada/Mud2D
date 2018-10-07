using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class ObjectBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int LastX { get; set; }
        public int LastY { get; set; }
        public char Symbol { get; set; }

        public ObjectBase()
        {
            X = Y = LastX = LastY = -1;
        }


        public void MoveTo(int newX, int newY)
        {
            LastX = X;
            LastY = Y;
            X = newX;
            Y = newY;
        }

    }

}

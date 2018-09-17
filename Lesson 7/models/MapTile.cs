<<<<<<< HEAD
﻿using System;
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

        internal void Draw()
        {
            Console.Write(Symbol);
        }
    }
}
=======
﻿using System;
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

        internal void Draw()
        {
            Console.Write(Symbol);
        }
    }
}
>>>>>>> 4c4388af791c5718fe4a26b8dff267c865bff09c

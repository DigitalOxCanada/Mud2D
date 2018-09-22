using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileDoor : MapTile
    {
        public bool IsLocked { get; set; }

        public MapTileDoor()
        {
            Symbol = '╬';   //206
            IsLocked = false;
            IsWalkable = false;
        }
    }
}

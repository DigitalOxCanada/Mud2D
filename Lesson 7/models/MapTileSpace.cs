using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileSpace : MapTile
    {
        public MapTileSpace()
        {
            Symbol = ' ';
            IsWalkable = true;
        }
    }
}

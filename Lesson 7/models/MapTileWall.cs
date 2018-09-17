using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileWall : MapTile
    {
        public MapTileWall()
        {
            Symbol = '#';
            IsWalkable = false;
        }
    }
}

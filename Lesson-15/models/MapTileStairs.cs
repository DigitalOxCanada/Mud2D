using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileStairs : MapTile
    {
        /// <summary>
        /// The number of levels these stairs would take the player in the direction (-/+)
        /// ie: -1 => would take the player up one level
        /// ie: 1 => would take the player down one level
        /// </summary>
        public int Direction { get; set; }

        
        public MapTileStairs(int dir = -1)
        {
            Direction = dir;
            IsWalkable = true;
            if(dir<0)
            {
                //up = number gets lesser
                Symbol = '«';   //174
            }else
            {
                //down = number gets bigger
                Symbol = '»';   //175
            }
        }
    }
}

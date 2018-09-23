using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileStairs : MapTile
    {
        /// <summary>
        /// The number of levels these stairs would take the player in the direction (-/+)
        /// ie: 1 => would take the player up one level
        /// ie: -1 => would take the player down one level
        /// 
        /// default to -1
        /// </summary>
        public int Direction { get; set; } = -1;

        public MapTileStairs(int dir = -1)
        {
            IsWalkable = true;
            if(dir>0)
            {
                Symbol = '«';   //174
            }else if(dir<0)
            {
                Symbol = '»';   //175
            }
        }
    }
}

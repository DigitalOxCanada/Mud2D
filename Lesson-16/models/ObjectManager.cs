using System;
using System.Linq;

namespace Mud2D.models
{
    public class ObjectManager
    {
        public ObjectManager()
        {

        }

        internal void AddObjects(MapTile[,] tiles, int num)
        {
            for (int i = 0; i < num; i++)
            {
                MapTile t = GetRandomTileSpace(tiles);
                t.pickableObject = new ObjectGold();
            }
        }

        public MapTile GetRandomTileSpace(MapTile[,] tiles)
        {
            Random randgen = new Random();

            var blanks = (from t in tiles.Cast<MapTile>() where t.GetType() == typeof(MapTileSpace) && t.IsWalkable && t.pickableObject == null select t).ToArray();
            var blank = blanks[randgen.Next(blanks.Length)];

            return blank;
        }

    }
}

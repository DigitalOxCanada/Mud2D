using Mud2D.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mud2D.models
{
    public class MonsterManager
    {
        public List<Monster> Monsters { get; set; }
        private MonstersDB MonstersDB { get; set; }

        public MonsterManager()
        {
            Monsters = new List<Monster>();
            MonstersDB = new MonstersDB();
        }

        public void ClearMonsters()
        {
            Monsters = new List<Monster>();
        }

        internal void AddMonsters(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                //we look for blanks every time so that we don't put a monster on top of another monster
                MapTile blank = GetRandomTileSpace(GameEngine.Tiles);
                var newmonster = new Monster(MonstersDB.GetRandom());

                newmonster.MoveTo(blank.X, blank.Y);
                
                Monsters.Add(newmonster);

                GameEngine.Tiles[blank.Y, blank.X].IsWalkable = false;
            }
        }

        public MapTile GetRandomTileSpace(MapTile[,] tiles)
        {
            Random randgen = new Random();

            var blanks = (from t in tiles.Cast<MapTile>() where t.GetType() == typeof(MapTileSpace) && t.IsWalkable select t).ToArray();
            var blank = blanks[randgen.Next(blanks.Length)];

            return blank;
        }

        internal void UpdateAll()
        {
            foreach (var m in Monsters)
            {
                m.Update();
            }

        }
    }
}

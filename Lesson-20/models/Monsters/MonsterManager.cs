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

        internal Monster GetMonsterAt(int newx, int newy)
        {
            var m = from mon in Monsters where mon.X == newx && mon.Y == newy select mon;

            return m.FirstOrDefault();  //there should only ever be a single, but just in case just get the first monster in the list or null
        }

        internal void PruneDeadMonsters()
        {
            //must make it ToList()
            var deadmonsters = (from mon in Monsters where !mon.IsAlive select mon).ToList();

            foreach(var mon in deadmonsters)
            {
                GameEngine.Tiles[mon.Y, mon.X].IsWalkable = true;   //no more monster on there
                Monsters.Remove(mon);   //remove monster from the list
            }

        }
    }
}

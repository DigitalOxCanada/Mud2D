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

        internal void AddMonster(MapTile[,] tiles, int howMany)
        {
            Random randgen = new Random();

            for (int i = 0; i < howMany; i++)
            {
                //we look for blanks every time so that we don't put a monster on top of another monster
                var blanks = (from t in tiles.Cast<MapTile>() where t.GetType() == typeof(MapTileSpace) select t).ToArray();
                var blank = blanks[randgen.Next(blanks.Length)];
                var newmonster = new Monster(MonstersDB.GetRandom());

                newmonster.MoveTo(blank.X, blank.Y);

                Monsters.Add(newmonster);

                //Monsters.Add(new Monster() { Name = "Ogre", Life = 5, X = blank.X, Y = blank.Y, LastY = blank.X, LastX = blank.Y, Speed = randgen.Next(6)+2 });
                tiles[blank.Y, blank.X].IsWalkable = false;
            }
        }

        internal void UpdateAll(GameEngine map)
        {
            foreach (var m in Monsters)
            {
                m.Update(map);
            }

        }
    }
}

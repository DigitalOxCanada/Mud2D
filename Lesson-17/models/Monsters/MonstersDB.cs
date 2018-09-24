using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mud2D.models
{
    public class MonstersDB
    {
        public List<MonsterDBModel> MonsterData { get; set; }

        public MonstersDB()
        {
            //we could put this data in a data file to read in but it should be encrypted or handled on the server side so that in a multiplayer environment
            //the data doesn't get hacked from client to client.
            MonsterData = new List<MonsterDBModel>
            {
                new MonsterDBModel { Id = 1, Name = "Ogre", Symbol = 'O', MaxLife = 6, Speed = 5, Damage = 5f },
                new MonsterDBModel { Id = 2, Name = "Kracken", Symbol = 'K', MaxLife = 10, Speed = 2, Damage = 7f },
                new MonsterDBModel { Id = 3, Name = "Great Spider", Symbol = 'S', MaxLife = 4, Speed = 1, Damage = 3f },
                new MonsterDBModel { Id = 4, Name = "Zephyr Hound", Symbol = 'H', MaxLife = 2, Speed = 3, Damage = 1f },
                new MonsterDBModel { Id = 5, Name = "Golem", Symbol = 'G', MaxLife = 12, Speed = 9, Damage = 6f },
            };

        }

        public MonsterDBModel GetRandom()
        {
            Random randgen = new Random();

            return MonsterData.ElementAt(randgen.Next(0, MonsterData.Count()));
        }
    }

    public class MonsterDBModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public char Symbol { get; set; }
        public int MaxLife { get; set; }
        public int Speed { get; set; }
        public float Damage { get; set; }
    }
}

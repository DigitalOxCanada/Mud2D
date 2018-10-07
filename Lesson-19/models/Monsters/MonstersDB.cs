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
                new MonsterDBModel { Id = 1, Name = "Ogre", Symbol = 'O', MaxLife = 6, MoveSpeed = 5, AttackSpeed = 2, Damage = 5f },
                new MonsterDBModel { Id = 2, Name = "Kracken", Symbol = 'K', MaxLife = 10, MoveSpeed = 2, AttackSpeed = 2, Damage = 7f },
                new MonsterDBModel { Id = 3, Name = "Great Spider", Symbol = 'S', MaxLife = 4, MoveSpeed = 1, AttackSpeed = 2, Damage = 3f },
                new MonsterDBModel { Id = 4, Name = "Zephyr Hound", Symbol = 'H', MaxLife = 2, MoveSpeed = 3, AttackSpeed = 2, Damage = 1f },
                new MonsterDBModel { Id = 5, Name = "Golem", Symbol = 'G', MaxLife = 12, MoveSpeed = 9, AttackSpeed = 2, Damage = 6f },
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
        public int MoveSpeed { get; set; }
        public int AttackSpeed { get; set; }
        public float Damage { get; set; }
    }
}

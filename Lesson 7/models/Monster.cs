<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class Monster
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; }
        public int Life { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }  //used for AI movement
        private DateTime _actionStart = new DateTime(0);

        public bool IsAlive
        {
            get { return (Life < 1 ? true : false); }
        }

        public Monster()
        {
            Life = 1;
            Symbol = 'M';
            Speed = 5;
        }

        public void Draw()
        {
            if(_actionStart.Ticks == 0)
            {
                _actionStart = DateTime.Now;
            }
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }

        public void PerformAI()
        {
            //TODO wait an elapsed amount of time based on movement speed
            if((DateTime.Now - _actionStart).Seconds>Speed)
            {
                Console.SetCursorPosition(0, 21);
                Console.WriteLine("Monster can now move!");
                _actionStart = DateTime.Now; //reset timer
            }
            //TODO look for an open space possibly towards the player

        }

    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class Monster
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; }
        public int Life { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }  //used for AI movement
        private DateTime _actionStart = new DateTime(0);

        public bool IsAlive
        {
            get { return (Life < 1 ? true : false); }
        }

        public Monster()
        {
            Life = 1;
            Symbol = 'M';
            Speed = 5;
        }

        public void Draw()
        {
            if(_actionStart.Ticks == 0)
            {
                _actionStart = DateTime.Now;
            }
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }

        public void PerformAI()
        {
            //TODO wait an elapsed amount of time based on movement speed
            if((DateTime.Now - _actionStart).Seconds>Speed)
            {
                Console.SetCursorPosition(0, 21);
                Console.WriteLine("Monster can now move!");
                _actionStart = DateTime.Now; //reset timer
            }
            //TODO look for an open space possibly towards the player

        }

    }
}
>>>>>>> 4c4388af791c5718fe4a26b8dff267c865bff09c

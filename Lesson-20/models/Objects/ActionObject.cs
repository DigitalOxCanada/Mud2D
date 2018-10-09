using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class ActionObject : ObjectBase
    {
        internal void Draw()
        {
            Console.Write(Symbol);
        }

        public virtual void OnActionEnter() { }

    }

    public class ObjectTorch : ActionObject
    {
        public ObjectTorch()
        {
            Symbol = 't';
        }

        public override void OnActionEnter()
        {
            GameEngine.ThePlayer.Inventory.Add(new CollectableObject() { Name = "Torch" });
            GameEngine.ScoreCard.Dirty = true;
            GameEngine.MessageBrd.Add($"You found a torch. Vision is better.");
            GameEngine.Tiles[Y, X].ActionObject = null;
        }
    }

    public class ObjectSword : ActionObject
    {
        public ObjectSword()
        {
            Symbol = '•';
        }

        public override void OnActionEnter()
        {
            var sword = new CollectableObject() { Name = "Sword" };
            sword.Attrib["Damage"] = 6;
            GameEngine.ThePlayer.Inventory.Add(sword);
            GameEngine.ScoreCard.Dirty = true;
            GameEngine.MessageBrd.Add($"You found a sword.");
            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on
        }
    }

    public class ObjectKey : ActionObject
    {
        public ObjectKey()
        {
            Symbol = '•';
        }

        public override void OnActionEnter()
        {
            GameEngine.ThePlayer.Inventory.Add(new CollectableObject() { Name = "Key" });
            GameEngine.ScoreCard.Dirty = true;
            GameEngine.MessageBrd.Add($"You found a key.");
            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on
        }
    }

    public class ObjectTrap : ActionObject
    {
        public ObjectTrap()
        {
            Symbol = 'º';
        }

        public override void OnActionEnter()
        {
            //TODO randomly choose trap type
            //lost map,bomb,teleport,ceiling trap, floor trap,poison
            GameEngine.MessageBrd.Add($"You hit a trap.");
            Random rand = new Random();
            int trap = rand.Next(0, 2);
            int damage = rand.Next(1, 3);
            switch (trap)
            {
                case 0:
                    GameEngine.ThePlayer.Life-=damage;    //TODO move to method on ThePlayer and handle death if <1
                    GameEngine.MessageBrd.Add($"You suffered {damage} damage.");
                    break;
                case 1:
                    //toggle twice effectively clears
                    GameEngine.CheatToggleFOW();
                    GameEngine.CheatToggleFOW();
                    GameEngine.MessageBrd.Add($"You lost your map.");
                    break;
                default:
                    GameEngine.ThePlayer.Life -= damage;    //TODO move to method on ThePlayer and handle death if <1
                    GameEngine.MessageBrd.Add($"You suffered {damage} damage.");
                    break;
            }

            GameEngine.ScoreCard.Dirty = true;
            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on

        }
    }

    public class ObjectGold : ActionObject
    {
        public int Value { get; set; }

        public ObjectGold()
        {
            Random rand = new Random();
            Value = rand.Next(GameEngine.CurrentLevel.Level*8, GameEngine.CurrentLevel.Level*20);
            Symbol = 'ó'; //162
        }

        public override void OnActionEnter()
        {
            //TODO random amount of gold based on level of map

            //pick up some gold
            GameEngine.ThePlayer.Gold += Value;
            GameEngine.ScoreCard.Dirty = true;
            GameEngine.MessageBrd.Add($"You found {Value} gold.");

            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on
            //once returning up the stack this object will no longer be referenced so the garbage collector should dispose of it.
        }
    }

    public class ObjectHeal : ActionObject
    {
        public int Value { get; set; }

        public ObjectHeal()
        {
            Random rand = new Random();
            Value = rand.Next(GameEngine.CurrentLevel.Level * 3, GameEngine.CurrentLevel.Level * 6);
            Symbol = 'Ω'; //234
        }

        public override void OnActionEnter()
        {
            GameEngine.ThePlayer.Heal(Value);
            GameEngine.ScoreCard.Dirty = true;
            GameEngine.MessageBrd.Add($"You were healed by {Value}.");

            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on
            //once returning up the stack this object will no longer be referenced so the garbage collector should dispose of it.
        }
    }

    public class ObjectTeleport : ActionObject
    {
        public ObjectTeleport()
        {
            Symbol = 'T';
        }

        public override void OnActionEnter()
        {
            //this should be a onetime teleport so delete the object after usage.
            var blank = GameEngine.GetRandomTileSpace();
            GameEngine.Tiles[Y, X].Dirty = true;
            GameEngine.Tiles[Y, X].IsWalkable = true;
            GameEngine.Tiles[GameEngine.ThePlayer.LastY, GameEngine.ThePlayer.LastX].Dirty = true;
            GameEngine.Tiles[GameEngine.ThePlayer.LastY, GameEngine.ThePlayer.LastX].IsWalkable = true; //this is set false when a player lands on the tile
            GameEngine.ThePlayer.MoveTo(blank.X, blank.Y);
            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on
        }

    }


}

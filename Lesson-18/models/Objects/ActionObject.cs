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

    public class ObjectSword : ActionObject
    {
        public ObjectSword()
        {
            Symbol = '•';
        }

        public override void OnActionEnter()
        {
            GameEngine.ThePlayer.Inventory.Add(new CollectableObject() { Name = "Sword" });
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
            GameEngine.ThePlayer.Life--;    //TODO move to method on ThePlayer and handle death if <1
            GameEngine.ScoreCard.Dirty = true;
            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on

        }
    }

    public class ObjectGold : ActionObject
    {
        public int Value { get; set; }

        public ObjectGold()
        {
            Value = 10;
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

    public class ObjectTeleport : ActionObject
    {
        public ObjectTeleport()
        {
            Symbol = 'T';
        }

        public override void OnActionEnter()
        {
            //TODO find blank space and move player there.
            //this should be a onetime teleport so delete the object after usage.
            var blank = GameEngine.GetRandomTileSpace();
            GameEngine.Tiles[Y, X].Dirty = true;
            GameEngine.Tiles[Y, X].IsWalkable = true;
            GameEngine.Tiles[GameEngine.ThePlayer.LastY, GameEngine.ThePlayer.LastX].Dirty = true;
            GameEngine.Tiles[GameEngine.ThePlayer.LastY, GameEngine.ThePlayer.LastX].IsWalkable = true; //this is set false when a player lands on the tile
            GameEngine.ThePlayer.MoveTo(blank.X, blank.Y);
            GameEngine.Tiles[Y, X].ActionObject = null; //destroy the action object on the tile this object is on

            //GameEngine.ThePlayer.Dirty = true;
        }

    }


}

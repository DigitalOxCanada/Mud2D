using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileSpace : MapTile
    {

        public MapTileSpace()
        {
            Symbol = ' ';
            IsWalkable = true;
        }

        public override void OnActionEnter(GameEngine gameEngine)
        {
            if(pickableObject.GetType() == typeof(ObjectGold))
            {
                //pick up some gold
                gameEngine.ThePlayer.Gold += ((ObjectGold)pickableObject).Value;
                gameEngine.MessageBrd.Add($"You found {((ObjectGold)pickableObject).Value} gold.");
                pickableObject = null;
            }
        }

    }

}

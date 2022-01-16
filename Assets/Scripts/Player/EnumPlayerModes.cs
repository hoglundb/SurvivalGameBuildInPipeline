using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
        //Enumerates the various modes the player can be in. The PlayerControllerParent class uses the mode to enable/disable the appropriate components
        public enum PlayerModes
        {
            DEFAULT,
            CRAFTING,
            BUILDING
        }
}

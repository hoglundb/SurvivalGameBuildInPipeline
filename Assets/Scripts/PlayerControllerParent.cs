using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerControllerParent : MonoBehaviour
    {
        public PlayerModes playerMode = PlayerModes.DEFAULT;

        //References to child components. (not really a parent child relationship, but we treat it this way to keep things organized)
        public PlayerBaseBuildingController playerBaseBuildingComponent;
        public PlayerMovement playerMovementComponent;

        
        /*Singleton pattern so other components can globally access the player. Need to handle differently for implementing mutliplayer*/
        private static PlayerControllerParent _instance;
        private void Awake()
        {
            playerBaseBuildingComponent = GetComponent<PlayerBaseBuildingController>();
            playerMovementComponent = GetComponent<PlayerMovement>();
            _instance = this;
            Cursor.lockState = CursorLockMode.Confined;
        }
        public static PlayerControllerParent GetInstance()
        {
            return _instance;
        }

        private void Start()
        {
           // playerMovementComponent.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }

        // Update is called once per frame
        void Update()
        {
            //If player toggles base building mode, activate/deactivate the child components accordinly. 
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (playerMode != PlayerModes.BUILDING)
                {
                    playerMode = PlayerModes.BUILDING;
                    playerBaseBuildingComponent.enabled = true;
                    playerMovementComponent.enabled = false;                    
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (playerMode == PlayerModes.BUILDING)
                {
                    playerMode = PlayerModes.DEFAULT;
                    playerBaseBuildingComponent.enabled = false;
                    playerMovementComponent.enabled = true;
                }
            }
        }
    }


    public enum PlayerModes
    {
        DEFAULT,
        CRAFTING,
        BUILDING
    }

}
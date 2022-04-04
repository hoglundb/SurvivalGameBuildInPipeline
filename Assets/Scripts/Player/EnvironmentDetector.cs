using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    /// <summary>
    /// Component that is attched to the player prefab. Allows player detect and interact with the invironment.
    /// </summary>
    public class EnvironmentDetector : MonoBehaviour
    {
        /// <summary>
        /// Reference to the UI prompt for the player to pick up an item. Toggle based on if player is looking directly at an item tha has the 'InventoryItem' component attached.
        /// </summary>
        private GameObject _uiPromptToPickItemUp;

       /// <summary>
       /// Reference to the PlayerControllerParent component that is also attached to the player prefab object. Allows us to go through the parent to reference other comonents on the player.
       /// </summary>
        private PlayerControllerParent _playerControllerParentComponent;

        /// <summary>
        /// Each frame we update this to the game object the player is looking at. Null if no object in range. 
        /// </summary>
        private Transform _lookAtObject;

        /// <summary>
        /// References the main camera game object. 
        /// </summary>
        private Camera _mainCamera;


        public static EnvironmentDetector intance;
        
        /// <summary>
        /// Initialize reference to other game objects
        /// </summary>
        private void Awake()
        {
            intance = this;
            _uiPromptToPickItemUp = GameObject.Find("PickupItemPrompt");
            _uiPromptToPickItemUp.SetActive(false);
            _playerControllerParentComponent = GetComponent<PlayerControllerParent>();
            _mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        }


        /// <summary>
        /// Check the invironment arount the player and respond to any related player input.
        /// </summary>
        private void Update()
        {
            _lookAtObject = _LookAtEnvironment();
            if (_lookAtObject != null)
            {
                _RespondToLookAtObject();
            }
        }


        /// <summary>
        /// This gets called each frame to update the _lookAtObject variable based on what the player is looking at. Returns the gameobject if one is detected.  
        /// </summary>
        /// <returns></returns>
        private Transform _LookAtEnvironment()
        {
            //If player movement is disabled, than so is this
            if (!_playerControllerParentComponent.playerMovementComponent.IsMovementInabled()) return null;

            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
            {
                return hit.transform;
            }
            return null;
        }


        /// <summary>
        /// Perform any action/prompt needed when the player is looking at an object. Requires that _lookAtObject be set previously in the frame and not be null. 
        /// </summary>
        private void _RespondToLookAtObject()
        {
            //Determine if the object is one player can put into inventory
            InventoryItem inventoryItemComponent = _lookAtObject.GetComponent<InventoryItem>();
            if (inventoryItemComponent != null)
            {
                _uiPromptToPickItemUp.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InventoryController.instance.AddItemToInventory(inventoryItemComponent);
                }
                return;
            }
            _uiPromptToPickItemUp.SetActive(false);
        }


        /// <summary>
        /// Returns the _lookAtObject that the player is currently looking at. Called externally my weapon classes to determine what objects to apply damage to
        /// </summary>
        /// <returns>Returns the transform of the game object the player is looking directly at.</returns>
        public Transform GetLookAtGameObject()
        {
            return _lookAtObject;
        }
    }
}


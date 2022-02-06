using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Player
{
    public class EnvironmentDetector : MonoBehaviour
    {
        private GameObject _uiPromptToPickItemUp;
        private Inventory.InventoryUIPanelManager _inventoryManagerComponent;
        private PlayerControllerParent _playerControllerParentComponent;

        private Transform _lookAtObject; //Each frame we update this to the game object the player is looking at. Null if no object in range. 

        private void Awake()
        {
            _uiPromptToPickItemUp = GameObject.Find("PickupItemPrompt");
            _uiPromptToPickItemUp.SetActive(false);
            var foo = GameObject.Find("PlayerInventoryPanel");
            _inventoryManagerComponent = GameObject.Find("PlayerInventoryPanel").GetComponent<Inventory.InventoryUIPanelManager>();
            _playerControllerParentComponent = GetComponent<PlayerControllerParent>();
        }


        private void Update()
        {
            _lookAtObject = _LookAtEnvironment();
            if (_lookAtObject != null)
            {
                _RespondToLookAtObject();
            }
        }


        //This gets called each frame to update the _lookAtObject variable based on what the player is looking at. Returns the gameobject if one is detected. 
        private Transform _LookAtEnvironment()
        {
            //If player movement is disabled, than so is this
            if (!_playerControllerParentComponent.playerMovementComponent.IsMovementInabled()) return null;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
            {
                return hit.transform;
            }
            return null;
        }



        //Perform any action/prompt needed when the player is looking at an object. Requires that _lookAtObject be set previously in the frame and not be null. 
        private void _RespondToLookAtObject()
        {
            //show hide the "Pickup" UI prompt if looking at a game object tagged as a PickupableItem.
            if (_lookAtObject.gameObject.tag == "PickupableItem")
            {
                _uiPromptToPickItemUp.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _inventoryManagerComponent.AddItemToInventory(_lookAtObject.gameObject);
                }
                return;
            }
            _uiPromptToPickItemUp.SetActive(false);
        }


        //Returns the _lookAtObject that the player is currently looking at. Called externally my weapon classes to determine what objects to apply damage to
        public Transform GetLookAtGameObject()
        {
            return _lookAtObject;
        }
    }
}


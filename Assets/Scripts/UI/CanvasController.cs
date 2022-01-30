using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The parent (entry point) for controlling all things canvas related (Inventory, Crafting, Health, etc.). Uses the singleton pattern so player can get a static reference to it. 
namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        //reference the UI panels for crafting, building, inventory, etc. These get toggled on/off based on player actions. 
        private CraftingController _craftingControllerComponent;
        private InventoryController _inventoryControllerComponent;
        private BuildingController _buildingControllerComponent;

        //Singleton pattern since only once instance of this component in the game
        private static CanvasController _instance; 
        public static CanvasController GetInstance() { return _instance; }


        private void Awake()
        {
            _instance = this;
            _craftingControllerComponent = GetComponentInChildren<CraftingController>();
            _inventoryControllerComponent = GetComponentInChildren<InventoryController>();
            _buildingControllerComponent = GetComponentInChildren<BuildingController>();
        }


        //Get methods for the various UI controller components. 
        public CraftingController GetCraftingControllerComponent()  {  return _craftingControllerComponent; }
        public InventoryController GetInventoryControllerComponent() { return _inventoryControllerComponent; }
        public BuildingController GetBuidlingControllerComponent() { return _buildingControllerComponent; }


        private void Update()
        {
            _SetUIVisibilityFromKeyboardInput();
        }


        //Activates/deactivates the UI components based on keyboard input from the player. Comumicate with the player to enable/disable player functionallity accordinly.
        private void _SetUIVisibilityFromKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            { 
               //if(_inventoryControllerComponent)
            }
            ////ToggleInventory visability and interactivity
            //if (Input.GetKeyDown(KeyCode.Tab))
            //{
            //    _baseBuildingUIPanel.SetActive(false);
            //    bool isNowShowing = _inventoryManager.ToggleVisibility();
            //    _playerControllerParentComponent.SetMovementEnablement(!isNowShowing);
            //}
            //else if (Input.GetKeyDown(KeyCode.B))
            //{
            //    _baseBuildingUIPanel.GetComponent<RectTransform>().localScale = Vector3.one;
            //}
            //else if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    _baseBuildingUIPanel.SetActive(false);
            //    bool isShowing = _craftingManager.ToggleVisibility();
            //    _playerControllerParentComponent.SetMovementEnablement(!isShowing);
            //}
        }


        //Toggles visibility of the inventory and crafting UI. Reinitialize the crafting menu based on material availability in the inventory. 
        //public void ToggleInventoryCraftingUI(bool shouldShow)
        //{
        //    _inventoryControllerComponent.SetVisibility(shouldShow);
        //    if (shouldShow)
        //    {
        //        _craftingControllerComponent.ReInitailizeCraftingMenu();
        //    }
        //}


        ////Calls the inventory component to add an item to the inventory. Update the Crafting component so that it coorectly shows which items are currently craftable. 
        //public void AddItemToInventory(GameObject gameObj)
        //{
        //    _inventoryControllerComponent.AddItemToInventory(gameObj);
        //    _craftingControllerComponent.ReInitailizeCraftingMenu();
        //}

    }
}



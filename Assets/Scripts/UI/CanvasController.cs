using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The parent (entry point) for controlling all things canvas related (Inventory, Crafting, Health, etc.). Uses the singleton pattern so player can get a static reference to it. 
namespace Inventory
{
    public class CanvasController : MonoBehaviour
    {
        //reference the UI panels for crafting, building, inventory, etc. These get toggled on/off based on player actions. 
        private InventoryUIPanelManager _inventoryManagerComponent;
        private CraftingUIPanelManager _craftingManagerComponent;
        private BuildingUIPanelUIManager _buildingControllerComponent;

        //Reference to the player parent component. Need to communcicate with the player when player performs certain actions with the UI
        private Player.PlayerControllerParent _playerControllerParentComponent;

        //Singleton pattern since only once instance of this component in the game
        private static CanvasController _instance; 
        public static CanvasController GetInstance() { return _instance; }


        private void Awake()
        {
            _instance = this;
            _craftingManagerComponent = GetComponentInChildren<CraftingUIPanelManager>();
            _inventoryManagerComponent = GetComponentInChildren<InventoryUIPanelManager>();
            _buildingControllerComponent = GetComponentInChildren<BuildingUIPanelUIManager>();
        }


        private void Start()
        {
            _SetAllUIPanelsInvisible();
            _playerControllerParentComponent = Player.PlayerControllerParent.GetInstance();
        }

        //Get methods for the various UI controller components. 
        public CraftingUIPanelManager GetCraftingControllerComponent()  {  return _craftingManagerComponent; }
        public InventoryUIPanelManager GetInventoryControllerComponent() { return _inventoryManagerComponent; }
        public BuildingUIPanelUIManager GetBuidlingControllerComponent() { return _buildingControllerComponent; }


        private void Update()
        {
            _SetUIVisibilityFromKeyboardInput();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _SetAllUIPanelsInvisible();
            }
        }


        //Activates/deactivates the UI components based on keyboard input from the player. Comumicate with the player to enable/disable player functionallity accordinly.
        private void _SetUIVisibilityFromKeyboardInput()
        {
            //Toggle the inventory panel
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (_inventoryManagerComponent.IsVisible())
                {
                    _inventoryManagerComponent.Hide();
                }
                else
                {
                    _SetAllUIPanelsInvisible();
                    _inventoryManagerComponent.Show();
                }
            }

            //toggle the crafting panel
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_craftingManagerComponent.IsVisible())
                {
                    _craftingManagerComponent.SetVisibility(false);
                }
                else
                {
                    _SetAllUIPanelsInvisible();
                    _craftingManagerComponent.SetVisibility(true);
                }
            }

            //toggle the base buidling panel
            else if (Input.GetKeyDown(KeyCode.B))
            {
                if (_buildingControllerComponent.IsVisible())
                {
                    _buildingControllerComponent.SetVisibility(false);
                }
                else 
                {
                    _SetAllUIPanelsInvisible();
                    _buildingControllerComponent.SetVisibility(true);
                }
            }

            //request the player to enable/disable movement as needed
            if (_buildingControllerComponent.IsVisible() || _craftingManagerComponent.IsVisible() || _inventoryManagerComponent.IsVisible())
            {
                _playerControllerParentComponent.SetMovementEnablement(false);
            }
            else 
            {
                _playerControllerParentComponent.SetMovementEnablement(true);
            }
        }


        //Tells all the UI panels to make themselves invisible. 
        private void _SetAllUIPanelsInvisible()
        {
            _inventoryManagerComponent.Hide();
            _craftingManagerComponent.SetVisibility(false);
            _buildingControllerComponent.SetVisibility(false);
        }

    }
}



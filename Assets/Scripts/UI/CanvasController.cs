using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The parent (entry point) for controlling all things canvas related (Inventory, Crafting, Health, etc.). Uses the singleton pattern so player can get a static reference to it. 
public class CanvasController : MonoBehaviour
{
    internal CraftingController _craftingController; //Reference the crafting component on the child Crafting canvas GameObject
    internal InventorySlotUIController _inventoryController; //References the inventory component on the child Inventory canvas game object. 
    private static CanvasController _instance;  //Static refrence to self, allows other components to globally reference this class via a singleton pattern. 


    //Allows other components to globally reference this class via a singleton pattern. 
    public static CanvasController GetInstance()
    {
        return _instance;
    }


    private void Awake()
    {
        _instance = this;
        _craftingController = GetComponentInChildren<CraftingController>();
        _inventoryController = GetComponentInChildren<InventorySlotUIController>();
    }


    //Toggles visibility of the inventory and crafting UI. Reinitialize the crafting menu based on material availability in the inventory. 
    public void ToggleInventoryCraftingUI(bool shouldShow)
    {
        _craftingController.gameObject.SetActive(shouldShow);
        _inventoryController.SetVisibility(shouldShow);
        if (shouldShow)
        {
            _craftingController.ReInitailizeCraftingMenu();
        }       
    }

}



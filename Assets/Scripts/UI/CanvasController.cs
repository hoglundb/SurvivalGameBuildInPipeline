using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The parent (entry point) for controlling all things canvas related (Inventory, Crafting, Health, etc.). Uses the singleton pattern so player can get a static reference to it. 
public class CanvasController : MonoBehaviour
{
    internal CraftingController craftingController; //Reference the crafting component on the child Crafting canvas GameObject
    internal InventoryController inventoryController; //References the inventory component on the child Inventory canvas game object. 
    private static CanvasController _instance;  //Static refrence to self, allows other components to globally reference this class via a singleton pattern. 


    //Allows other components to globally reference this class via a singleton pattern. 
    public static CanvasController GetInstance()
    {
        return _instance;
    }


    private void Awake()
    {
        _instance = this;
        craftingController = GetComponentInChildren<CraftingController>();
        inventoryController = GetComponentInChildren<InventoryController>();
    }


    //Toggles visibility of the inventory and crafting UI. Reinitialize the crafting menu based on material availability in the inventory. 
    public void ToggleInventoryCraftingUI(bool shouldShow)
    {
        craftingController.gameObject.SetActive(shouldShow);
        inventoryController.SetVisibility(shouldShow);
        if (shouldShow)
        {
            craftingController.ReInitailizeCraftingMenu();
        }       
    }



    //Calls the inventory component to add an item to the inventory. Update the Crafting component so that it coorectly shows which items are currently craftable. 
    public void AddItemToInventory(GameObject gameObj)
    {
        inventoryController.AddItemToInventory(gameObj);
        craftingController.ReInitailizeCraftingMenu();
    }

}



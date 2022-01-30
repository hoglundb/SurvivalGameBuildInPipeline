//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


////Contains all the methods for managing the crafting based on player UI actions. 
//public class CraftingController : MonoBehaviour
//{
//    //List of craftable menu items defined in the Canvas UI. 
//    private List<CraftableMenuItem> _craftableMenuItems;


//    private void Awake()
//    {
//        //Set up reference to list of craftable menu items on this component. 
//        GameObject[] items = GameObject.FindGameObjectsWithTag("CraftableMenuItem");
//        _craftableMenuItems = new List<CraftableMenuItem>();
//        for (int i = 0; i < items.Length; i++)
//        {
//            _craftableMenuItems.Add(items[i].GetComponent<CraftableMenuItem>());
//        }
//    }


//    //Resets the crafting menu options based on contents of player enventory. Call this whenever an item is added/removed from player inventory. 
//    public void ReInitailizeCraftingMenu()
//    {
//        foreach (var item in _craftableMenuItems)
//        {
//            item.ReInitailizeMenuItem();
//        }
//    }
//}




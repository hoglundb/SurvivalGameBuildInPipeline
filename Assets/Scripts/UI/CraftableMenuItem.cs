using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Component that is attached to each craftable menu item in the crafting menu. 
public class CraftableMenuItem : MonoBehaviour
{
    private UI.CanvasController _canvasController;

    private Button _btnCraftButton; //The craft button on this game object. Will be disabled if not enough ingredients for player to craft this item. 

    [Header("Prefab to Spawn when item is crafted")]
    [SerializeField] private GameObject _craftedItemPrefab;

    [Header("Name of the inventory item this will create")]
    [SerializeField] private string _itemName;

    [Header("List of ingredents needed to craft this item")]
    [SerializeField] private List<CraftingIngredient> _ingredientsList;
    

    private void Awake()
    {       
        _btnCraftButton = GetComponentInChildren<Button>();
        _btnCraftButton.onClick.AddListener(OnCraftItemBtnClick);
        _btnCraftButton.interactable = false;
    }


    private void Start()
    {
        _canvasController = UI.CanvasController.GetInstance();
    }


    //Called by the inventory controller component. Will toggle button enablement, based on if their are suffecuent inventory items to craft this item. 
    public void ReInitailizeMenuItem()
    {
        _btnCraftButton.interactable = _HasIngredientsToCraftItem();
    }


    //Looks through the inventory to see if there are the required ingredients to craft this item. 
    private bool _HasIngredientsToCraftItem()
    {
        foreach (var i in _ingredientsList)
        { 
            int amountInInventory = _canvasController.GetInventoryControllerComponent().GetItemCount(i.ingredient);
            if (amountInInventory < i.quantity)
            {
                return false;
            }
        }
        return true;
    }


    //Called when the "Craft" button is clicked for this item. Only clickable if inventory contains the needed ingredients to craft this item. 
    public void OnCraftItemBtnClick()
    {
        //Craft the physical item from the ingredients in the inventory. 
        CraftThisItem();
        _btnCraftButton.interactable = false;

        //Check if we need to disable the button becuase we used up items needed to craft it
        ReInitailizeMenuItem();
    }


    //Crafts the specified item. Tells the inventory which items to use up in the process. New item will be placed in the inventory if their is room.
    public void CraftThisItem()
    {
        //Use up the required inventory items. 
        foreach (var item in _ingredientsList)
        {
            bool wasSuccess = _canvasController.GetInventoryControllerComponent().UseAndDestroyItems(item.ingredient, item.quantity);
            if (!wasSuccess)
            {
                Debug.LogError("unable to destroy " + item.quantity + " of item" + item.ingredient);
                return;
            }
        }

        //Spawn the newly crafted game object. TODO: use object pooling to manage this in the future. 
        GameObject craftedItem = Instantiate(_craftedItemPrefab);
        _canvasController.GetInventoryControllerComponent().AddItemToInventory(craftedItem);
    }

}

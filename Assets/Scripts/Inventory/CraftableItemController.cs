using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**********************************************************************************************
This component is attached to each CraftingUISlotPrefab game object which represents an item in the 
crafting menu. It contains the methods needed for the player to craft the specific item based on materials
in the player's inventory.
***********************************************************************************************/
public class CraftableItemController : MonoBehaviour
{
    [Header("Reference to the parent panel")]
    [SerializeField] private GameObject _playerCraftingPanel;

    [Header("The name and description item")]
    [SerializeField] public string itemName;
    [SerializeField] [Multiline] public string itemDescription;

    [Header("The game object spawned")]
    [SerializeField] public GameObject _craftablePrefab;

    [SerializeField] private GameObject _craftBtnTransform;
    [SerializeField] private GameObject _selectItemBtnTransform;

    [Header("List of item/quanity ingredients")]
    [SerializeField] public List<CraftingIngredient> ingredients;

    private GameObject _playerCraftingDescriptionPanel;
    private InventoryUI.InventoryManager _inventoryManagerComponent;

    private void Awake()
    {
        //Init the event handler for the craft button
        _craftBtnTransform.GetComponent<Button>().onClick.AddListener(_OnCraftingBtnClick);
        _selectItemBtnTransform.GetComponent<Button>().onClick.AddListener(_OnSelectItemBtnClick);

        _playerCraftingDescriptionPanel = GameObject.Find("CraftingDescriptionPanel");
        _playerCraftingDescriptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);

        _inventoryManagerComponent = GameObject.Find("PlayerInventoryPanel").GetComponent<InventoryUI.InventoryManager>();
    }


    //Called when player clicks the craft btn on this item. Btn will not be clickable if there are not enough ingredients for the player to craft this item.
    private void _OnCraftingBtnClick()
    {
        //Remove the ingredients from the player's inventory
        foreach (var ingredient in ingredients)
        {
            _inventoryManagerComponent.RemoveItemQuantityByType(ingredient.ingredient, ingredient.quantity);
        }

        //Instanciate the crafted game object and add it to the player's inventory. 
        GameObject craftedItem = Instantiate(_craftablePrefab);
        _inventoryManagerComponent.AddItemToInventory(craftedItem);

        //Update the craftability of each item since items were removed from the inventory to craft this. 
        _playerCraftingPanel.GetComponent<InventoryUI.CraftingManager>().UpdateCraftabilityForItems();
    }


    //Called when a player clicks on this item. We update the UI to show the details pertaining to this item, such as the list of items it takes to craft this. 
    private void _OnSelectItemBtnClick()
    {
        //Ensure the text box for the item ingredients list is showing. 
        _playerCraftingDescriptionPanel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        //Build the ingredients list string from the ingredients object. 
        string ingredientsList = "";
        foreach (var item in ingredients)
        {
            ingredientsList += item.quantity.ToString() + " " + item.ingredient + "\n";
        }

        //Tell the UI text box for item ingredients to populate with the text we built out in the previous step. 
        _playerCraftingPanel.GetComponent<InventoryUI.CraftingManager>().UpdateItemIngredientsTextbox(ingredientsList);
    }


    //Called by the Crafting Manager to enable/disable the "craft" button based on if the inventory contains the needed ingredients to craft this item. 
    public void SetCraftability(bool setAsCraftable)
    {
        _craftBtnTransform.GetComponent<Button>().interactable = setAsCraftable;
    }
}



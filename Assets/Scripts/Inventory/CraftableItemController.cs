using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This component is on each crafting UI panel game object. 
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

    private void Awake()
    {
        //Init the event handler for the craft button
        _craftBtnTransform.GetComponent<Button>().onClick.AddListener(_OnCraftingBtnClick);
        _selectItemBtnTransform.GetComponent<Button>().onClick.AddListener(_OnSelectItemBtnClick);

        _playerCraftingDescriptionPanel = GameObject.Find("CraftingDescriptionPanel");
        _playerCraftingDescriptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
    }


    private void _OnCraftingBtnClick()
    {

      
    }


    private void _OnSelectItemBtnClick()
    {
        _playerCraftingDescriptionPanel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        string ingredientsList = "";
        foreach (var item in ingredients)
        {
            ingredientsList += item.quantity.ToString() + " " + item.ingredient + "\n";
        }
        _playerCraftingPanel.GetComponent<Inventory.CraftingManager>().UpdateItemIngredientsTextbox(ingredientsList);
    }


    //TODO: Searches the player's inventory to determine if there are enough ingredients to craft this item. If not, disable the craft button
    private void _HasIngredientsToCraftThisItem()
    { 
       
    }
}



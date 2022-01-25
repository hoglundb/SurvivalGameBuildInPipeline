using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This component is on each crafting UI panel game object. 
public class CraftableItemController : MonoBehaviour
{
    [Header("The name and description item")]
    [SerializeField] public string itemName;
    [SerializeField] [Multiline] public string itemDescription;

    [Header("The game object spawned")]
    [SerializeField] public GameObject _craftablePrefab;

    [SerializeField] private GameObject _craftBtnTransform;

    [Header("List of item/quanity ingredients")]
    [SerializeField] public List<CraftingIngredient> ingredients;

 

    private void Awake()
    {
        //Init the event handler for the craft button
        _craftBtnTransform.GetComponent<Button>().onClick.AddListener(_OnCraftingBtnClick);
    }


    private void _OnCraftingBtnClick()
    {
        Debug.LogError("clicked me");
    }


    //TODO: Searches the player's inventory to determine if there are enough ingredients to craft this item. If not, disable the craft button
    private void _HasIngredientsToCraftThisItem()
    { 
       
    }
}



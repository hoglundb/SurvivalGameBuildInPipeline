using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*****************************************************************************************************************
 This component lives on each UI element in the BuildingUI scroll view. 
 ****************************************************************************************************************/
public class BuildingBlockUISlotManager : MonoBehaviour
{
    [Header("The item this will spawn")]
    [SerializeField] private GameObject _buildingBlockPrefab;

    [SerializeField] private List<CraftingIngredient> _ingredientsList;

    [Header("Reference Child UI Components")]
    [SerializeField] private GameObject _btnCreateBlock;
    [SerializeField] private GameObject _insufMatAlert;

    private void Awake()
    {
        _btnCreateBlock.GetComponent<Button>().onClick.AddListener(_OnCreateBlockBtnClick);
    }


    //Called when player clicks the "Create" button for the block defined in this UI element. 
    private void _OnCreateBlockBtnClick()
    {
        
        Player.PlayerControllerParent.GetInstance().playerBaseBuildingComponent.CreateBlockForPlacement(_buildingBlockPrefab);
        Inventory.BuildingUIPanelUIManager.GetInstance().SetVisibility(false);
    }


    //Updates this item to make it clickable based on if there is enough material in the player inventory to create this item. Returns true in suffecient materials. Returns false otherwise. 
    public bool UpdateSlotForMaterialAvailabiltiy()
    {
        return true;
    }
}

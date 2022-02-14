using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Inventory/SOGroupableInventoryItemsData")]
public class SOGroupableInventoryItemsData : ScriptableObject, IInventoryData
{
    [Header("Items of type 'Material'")]
    [SerializeField] public Inventory.InventoryItemCategoryData materials;

    [Header("Items of type 'Ammo'")]
    [SerializeField] public Inventory.InventoryItemCategoryData ammo;


    public  bool TryAddInventoryItem(Inventory.AbstractInventoryItem objInventoryItemComponent)
    {
        SOInventoryItem itemIfDefined = _GetInventoryItemIfDefined(objInventoryItemComponent.GetItemDataSO());
        SOGroupableInventoryItem groupable = (SOGroupableInventoryItem)itemIfDefined;
        groupable.IncrementQuantity();
        Inventory.InventoryUIPanelManager.GetInstance().UpdateInventoryUI();

        if (itemIfDefined == null) return false;
  
        return true;
    }


    protected SOInventoryItem _GetInventoryItemIfDefined(SOInventoryItem inventoryItemToAddSO)
    {
        SOInventoryItem item = null;
        item = _IsInventoryItemDefinedInCategory(materials, inventoryItemToAddSO);
        if (item != null) return item;

        item = _IsInventoryItemDefinedInCategory(ammo, inventoryItemToAddSO);
        if (item != null) return item;
        return null;
    }


    private SOInventoryItem _IsInventoryItemDefinedInCategory(Inventory.InventoryItemCategoryData category, SOInventoryItem inventoryItemToAddSO)
    {
        foreach (var item in category.items)
        {
            if (item.name == inventoryItemToAddSO.name)
            {
                return item;
            }
        }
        return null;
    }
}

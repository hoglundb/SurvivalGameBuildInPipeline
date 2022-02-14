//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///*
// *  Holds the scriptable objects for each inventory item, grouped by type.  
//  */
//[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Inventory/SOAbstractInventoryItemsData")]
//public class SOAbstractInventoryItemsData : ScriptableObject
//{
//    [Header("Items of type 'Weapon/Tool'")]
//    [SerializeField] public InventoryItemCategoryData weaponsAndTools;

//    [Header("Items of type 'Clothing'")]
//    [SerializeField] public InventoryItemCategoryData clothing;

//    [Header("Items of type 'Material'")]
//    [SerializeField] public InventoryItemCategoryData materials;

//    [Header("Items of type 'Food'")]
//    [SerializeField] public InventoryItemCategoryData food;

//    [Header("Items of type 'Ammo'")]
//    [SerializeField] public InventoryItemCategoryData ammo;


//    public virtual bool TryAddInventoryItem(Inventory.AbstractInventoryItem objInventoryItemComponent) { return false; }


//    //public bool TryAddInventoryItem(Inventory.AbstractInventoryItem objInventoryItemComponent)
//    //{
//    //    if (!_IsInventoryItemDefined(objInventoryItemComponent.GetItemDataSO())) return false;

//    //    return true;
//    //}


//    //protected bool _IsInventoryItemDefined(SOInventoryItem inventoryItemToAddSO)
//    //{
//    //    if (_IsInventoryItemDefinedInCategory(weaponsAndTools, inventoryItemToAddSO)) return true;
//    //    if (_IsInventoryItemDefinedInCategory(clothing, inventoryItemToAddSO)) return true;
//    //    if (_IsInventoryItemDefinedInCategory(materials, inventoryItemToAddSO)) return true;
//    //    if (_IsInventoryItemDefinedInCategory(food, inventoryItemToAddSO)) return true;
//    //    if (_IsInventoryItemDefinedInCategory(ammo, inventoryItemToAddSO)) return true;

//    //    return false;
//    //}



//    private bool _IsInventoryItemDefinedInCategory(InventoryItemCategoryData category, SOInventoryItem inventoryItemToAddSO)
//    {
//        foreach (var item in category.items)
//        {
//            if (item.name == inventoryItemToAddSO.name)
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//}






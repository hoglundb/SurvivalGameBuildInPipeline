using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inventory
{
    [System.Serializable]
    public class InventoryItemCategoryData
    {
        [SerializeField] public List<SOInventoryItem> items;
    }

}

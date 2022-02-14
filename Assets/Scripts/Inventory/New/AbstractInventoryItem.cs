using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inventory
{
    /*
     The base class for inventory item component. Contains the methods and members common to both Individual and Groupable Inventory item types. 
     */
    public abstract class AbstractInventoryItem : MonoBehaviour
    {
        [SerializeField] private SOInventoryItem _soInventoryItem;


        protected void Awake()
        {
            if (_soInventoryItem == null)
            {
                Debug.LogError("InventoryItem SO not defined");
            }
        }


        public string GetItemName()
        {
            return _soInventoryItem.name;
        }


        public SOInventoryItem GetItemDataSO()
        {
            return _soInventoryItem;
        }


        public SOInventoryItem GetInventoryItemSO()
        {
            return _soInventoryItem;
        }
    }
}


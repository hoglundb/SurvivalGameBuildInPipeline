using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Item
{
    public class WeaponOrToolItem : MonoBehaviour, Inventory.IInventoryItem
    {
        [SerializeField] private SOWeaponsAndTools _weaponData;

        public SOGeneralInventoryItem GetItemInfo()
        {
            return _weaponData;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialItem : MonoBehaviour, Inventory.IInventoryItem
{
    [SerializeField] private SOMaterial _materialData;

    public SOGeneralInventoryItem GetItemInfo()
    {
        return _materialData;
    }
}

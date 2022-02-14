using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Inventory/SOGroupableInventoryItem")]
public class SOGroupableInventoryItem : SOInventoryItem
{
    [Header("Quantity of this item currently in inventory")]
    public int quantity;

    private void OnEnable()
    {
        quantity = 0;
    }

    public void IncrementQuantity()
    {
        quantity++;
    }

    public void DecrementQuantity()
    {
        quantity--;
    }
}

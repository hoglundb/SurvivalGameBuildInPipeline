using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SOGroupableInventoryItem : SOGeneralInventoryItem
{
    [Header("Total quantity of this item current in inventory")]
    [SerializeField] public int quantity;

    [Header("Prefab variations for when item is removed from inventory")]
    [SerializeField] public List<GameObject> prefabs;
}

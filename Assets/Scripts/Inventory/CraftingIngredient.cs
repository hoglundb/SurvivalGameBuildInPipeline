using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Inventory
{
    [System.Serializable]
    public class CraftingIngredient
    {
        [SerializeField] public string itemName;
        [SerializeField] public int itemQuantity;
    }
}


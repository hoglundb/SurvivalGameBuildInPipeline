using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Crafting
{
    [System.Serializable]
    public class CraftingIngredient
    {
        [SerializeField] public SOMaterial _materialSO;
        [SerializeField] public int quantity;
    }
}


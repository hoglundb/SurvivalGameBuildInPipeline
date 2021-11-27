using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Defines an ingredient and quantity tuple for a specific ingredient in a crating list
[System.Serializable]
public class CraftingIngredient
{
    [SerializeField] public string ingredient;
    [SerializeField] public int quantity;
}



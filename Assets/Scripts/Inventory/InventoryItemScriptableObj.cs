using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory", menuName = "InventoryItem")]
public class InventoryItemScriptableObj : ScriptableObject
{
    [Header("General Item Info")]
    public string ItemName;
    public float ItemWeight;
    public Sprite ItemUISprite;

    [Header("Fields if edible")]
    public float isConsumable;
    public float proteinGain;
    public float mineralGame;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory", menuName = "InventoryItem")]
public class InventoryItemScriptableObj : ScriptableObject
{
    [Header("General Item Info")]
    public string ItemName;
    public Sprite ItemUISprite;

    [Header("Info if item can be equiped")]
    public bool isEquipable = false;
    public Vector3 localPosition;
    public Vector3 localEulers;
    public string animTriggerHold;
    public string animTriggerMelee;

    [Header("Fields if edible")]
    public bool isConsumable;
    [SerializeField] public NutritionStats nutritionStats;
}



[System.Serializable]
public class NutritionStats
{ 
    [SerializeField] public float Protein;
    [SerializeField] public float Mineral;
    [SerializeField] public float Hydration;

}

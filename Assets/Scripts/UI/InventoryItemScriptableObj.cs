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
    public EquipableItemInfo equipableItemInfo;
    public EquipableItemType equipableItemType = EquipableItemType.NONE;
    public LeftOrRight parentHand = LeftOrRight.NONE;
    public Vector3 localPosition;
    public Vector3 localEulers;
    public string animTriggerHold = "NA"; //Anim trigger name for player holding this item
    public string animTriggerMelee = "NA"; //Anim trigger name for player malee with this item (if applicable)
    public string animTriggerBowDraw = "NA"; //Anim trigger name for player drawing this item (if item a bow)
    public string animTriggerBowRelease = "NA"; //Anim trigger name for player releasing bow (if item is a bow)
    public string animTriggerReload = "NA"; //Anim trigger name if a reloadable weapon. 
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


public enum LeftOrRight
{ 
   LEFT, 
   RIGHT,
   NONE,
}



//Defines the list of equipable item types. The PlayerWeaponController users this to manage the gameplay logic for the equiped item. 
public enum EquipableItemType
{ 
    NONE,
    BOW, 
    MELEE,
    GUN,
    SPEAR,
}


//Holds all the info for a scriptable object inventory item that is also equipable (e.g. a weapon or tool)
[System.Serializable]
public class EquipableItemInfo
{   
    [SerializeField] public EquipableItemType equipableItemType = EquipableItemType.NONE;
    [SerializeField] public LeftOrRight parentHand = LeftOrRight.NONE;
    [SerializeField] public Vector3 localPosition;
    [SerializeField] public Vector3 localEulers;
    [SerializeField] public string animTriggerHold = "NA"; //Anim trigger name for player holding this item
    [SerializeField] public string animTriggerMelee = "NA"; //Anim trigger name for player malee with this item (if applicable)
    [SerializeField] public string animTriggerBowDraw = "NA"; //Anim trigger name for player drawing this item (if item a bow)
    [SerializeField] public string animTriggerBowRelease = "NA"; //Anim trigger name for player releasing bow (if item is a bow)
    [SerializeField] public string animTriggerReload = "NA"; //Anim trigger name if a reloadable weapon. 
}
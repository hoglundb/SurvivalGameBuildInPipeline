using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to create a scriptble object for an InventoryItem. It contains the static metadata pertaining to an inventory 
/// item of a particular type. A scriptable object created from this will live as a class member of an InventoryItem component
/// that is attached to a pickupable game object. 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryItem", order = 1)]
public class InventoryItemScriptableObjCreator : ScriptableObject
{
    /// <summary>
    /// Unique identifier for the specific InventoryItem the scriptable object will represent.
    /// </summary>
    [SerializeField] public new string name;

    /// <summary>
    /// The UI image in the inventory that represents a particlar type of inventory item.
    /// </summary>
    [SerializeField] public Sprite uiSpriteImage;

    /// <summary>
    /// /// True if an inventory item of a particular type can be combined in a single slot with other items of the same type. False otherwise. 
    /// </summary>
    [SerializeField] public bool isStackable;

    /// <summary>
    /// If isStackable=true, then this reprents the max number of items of this type that can be combined in a single inventory slot. 
    /// </summary>
    [SerializeField] public int stackHeight;
}

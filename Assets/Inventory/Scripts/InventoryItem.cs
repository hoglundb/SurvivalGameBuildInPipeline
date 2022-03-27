using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This componet is present on any game object that the player is able to pickup and add to their inventory
/// </summary>
public class InventoryItem : MonoBehaviour
{
    /// <summary>
    /// Scriptable object containing meta-data pertaining to this particular type of inventory item
    /// </summary>
    [SerializeField] private InventoryItemScriptableObjCreator _itemInfo;



    /// <summary>
    /// Allows the inventory to get the meta data item info for this type of inventory item. 
    /// </summary>
    /// <returns>The _itemInfo scripable object. </returns>
    public InventoryItemScriptableObjCreator GetItemData()
    {
        return _itemInfo;
    }

}

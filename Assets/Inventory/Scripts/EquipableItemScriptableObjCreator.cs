using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object referenced by an inventory item's "EquipableItem" component. 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Inventory/EquipableItem", order = 1)]
public class EquipableItemScriptableObjCreator : ScriptableObject
{
    /// <summary>
    /// The position offset when item is equiped and is a child of the player's hand bond
    /// </summary>
    [SerializeField] public Vector3 handPosOffset;

    /// <summary>
    /// The rotation offset when item is equiped and is a child of the player's hand bone.
    /// </summary>
    [SerializeField] public Vector3 handRotOffset;

    /// <summary>
    /// The hand to make this item a child of when player has it equiped
    /// </summary>
    [SerializeField] public LeftOrRight primaryHand;

    /// <summary>
    /// Name of the animation trigger for when player equips the item.
    /// </summary>
    [SerializeField] public string equipItemAnimationTrigger;
}

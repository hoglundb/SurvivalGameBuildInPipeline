using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObjects/ItemInteractionScriptableObject")]
public class ItemInteractionScriptableObject : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public string itemDisplayName;

    [SerializeField] public PickupableItemCategory itemCategory = PickupableItemCategory.NONE;
}
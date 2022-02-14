using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Sciptable object to hold all the information associated how an item behaves in the player's inventory
  */
[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Inventory/SOInventoryItem")]
public class SOInventoryItem : ScriptableObject
{

    [Header("The unique identifier for this type of inventory item")]
    [SerializeField] public GameObject itemPrefab;

    [Header("If variations on this type of item")]
    [SerializeField] public List<GameObject> itemPrefabVariations;

    [Header("The display name for this type of inventory item")]
    [SerializeField] public string displayName;

    [Header("Weight for one item of this type")]
    [SerializeField] public int itemWeight;

}

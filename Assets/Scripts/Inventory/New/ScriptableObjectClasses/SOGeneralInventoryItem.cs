using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOGeneralInventoryItem : ScriptableObject
{
    [Header("Info common to all inventory items")]
    [SerializeField] public string ItemName;
    [SerializeField] public int ItemWeight;
  
}

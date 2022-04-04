using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that is present on the "MeleeItem" comonent of Inventory items that can be used to melee. 
/// Contains all the static data associated with the Melee item of a particular type.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Inventory/MeleeItem", order = 1)]
public class MeleeItemScriptableObjCreator : ScriptableObject
{
    [Header("Animation Info")]
    [SerializeField] public string meleeAnimationTrigger;
    [SerializeField] public float meleeDelay;

    [Header("Melee Item Stats")]
    [SerializeField] [Range(0, 1)] public float treeDamageDealt;
    [SerializeField] [Range(0, 1)] public float rockDamageDealt;
    [SerializeField] [Range(0, 1)] public float enemyDamageDealt;
}

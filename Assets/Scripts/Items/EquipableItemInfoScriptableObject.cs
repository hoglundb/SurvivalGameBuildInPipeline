using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************************************************
 Creates a scriptable object for the AbstractEquipableItem class which is attached to objects the player is 
 able to equip. 
 *****************************************************************************************************************/
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EquipableItemInfoScriptableObject")]
public class EquipableItemInfoScriptableObject : ScriptableObject
{
    [Header("The parent hand")]
    [SerializeField] public bool _rightHandControlled = false;

    [Header("Position Offsets For When In Player Hand")]
    [SerializeField] public Vector3 _dominantHandPosOffset;
    [SerializeField] public Vector3 _dominantHandRotOffset;
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to hold the data for a bow. The BowItem component on the bow game object references this scriptable object. 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Inventory/BowItem", order = 1)]
public class BowItemScriptableObjectCreator : ScriptableObject
{
    [SerializeField] public string drawBowAnimationTrigger;
    [SerializeField] public string drawBowAnimationStateName;
    [SerializeField] public string aimBowAnimationStateName;
    [SerializeField] public string fireBowAnimationTrigger;
    [SerializeField] public string bowIdleAniamationStateName;
    [SerializeField] public Vector3 arrowEquippedPosition;
    [SerializeField] public Vector3 arrowEquippedEulers;
    [SerializeField] public float drawArrowPosOffset;
    [SerializeField] public float arrowSpeed;
}

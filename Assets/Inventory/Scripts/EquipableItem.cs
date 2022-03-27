using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This component is attached to an item prefab to indicate that it is equipable. It contains the stats related
/// to this type of equipable item. It contains the methods for the for the prefab to be equiped by the player. 
/// </summary>
public class EquipableItem : MonoBehaviour
{
    /// <summary>
    /// Reference the scriptable object that contains the data for equiping an item of this type.
    /// </summary>
    [SerializeField] private EquipableItemScriptableObjCreator _equipableItemData;

    /// <summary>
    /// Hand bone transform that will be set as this object's perent when equiped by the player. 
    /// </summary>
    private Transform _parentTransform;


    /// <summary>
    /// Make the script default to being disabled. Only enable it once player equips the item.
    /// </summary>
    private void Awake()
    {
        this.enabled = false;
    }


    /// <summary>
    /// Enabling this component triggers it to be equiped by the player
    /// </summary>
    private void OnEnable()
    {
        Debug.LogError("equipping " + gameObject.name);

        // Disable the rigidbody
        GetComponent<Rigidbody>().isKinematic = true;

        // set the parent of this game object to be the player's hand

        if (_equipableItemData.primaryHand == LeftOrRight.LEFT)
        {
            _parentTransform = Player.PlayerControllerParent.GetInstance().GetLeftHandBone();
        }
        else if (_equipableItemData.primaryHand == LeftOrRight.RIGHT)
        {
            _parentTransform = Player.PlayerControllerParent.GetInstance().GetRightHandBone();
        }

        Player.PlayerControllerParent.GetInstance().SetAnimationTrigger(_equipableItemData.equipItemAnimationTrigger);
        transform.parent = _parentTransform;
    }


    /// <summary>
    /// TODO
    /// </summary>
    private void Update()
    {
        if (_parentTransform != null)
        {
            transform.localPosition = _equipableItemData.handPosOffset * .0001f;
            transform.localRotation = Quaternion.Euler(_equipableItemData.handRotOffset * .25f);
        }
    }


    /// <summary>
    /// Disabling this script will trigger the unequiping of this item. 
    /// </summary>
    private void OnDisable()
    {
        // Enable the rigidbody
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = true;
        _parentTransform = null;
        Player.PlayerControllerParent.GetInstance().SetAnimationTrigger("Idle");
    }
}

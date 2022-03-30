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
    /// If this is true, we save the value of the game object transfor to the scriptable object that saves this data
    /// </summary>
    [SerializeField] private bool _isSetupMode = false;

    /// <summary>
    /// Hand bone transform that will be set as this object's perent when equiped by the player. 
    /// </summary>
    private Transform _parentTransform;


    /// <summary>
    /// Make the script default to being disabled. Only enable it once player equips the item.
    /// </summary>
    private void Awake()
    {
        _isSetupMode = false;
        this.enabled = false;
    }


    /// <summary>
    /// Enabling this component triggers it to be equiped by the player
    /// </summary>
    private void OnEnable()
    {
        _isSetupMode = false;

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
        transform.localPosition = _equipableItemData.handPosOffset;
        transform.localRotation = Quaternion.Euler(_equipableItemData.handRotOffset);

        // Enable the component responsible for the player interacting with the equipable item
        if (GetComponent<BowItem>())
        {
            GetComponent<BowItem>().OnEquipBow();
        }
    }


    /// <summary>
    /// TODO
    /// </summary>
    private void Update()
    {
        //if (_parentTransform != null && _isSetupMode)
        //{
        //    _equipableItemData.handPosOffset = transform.localPosition;
        //    _equipableItemData.handRotOffset = transform.localRotation.eulerAngles;
        //}
    }


    /// <summary>
    /// Disabling this script will trigger the unequiping of this item. 
    /// </summary>
    private void OnDisable()
    {
        // If bow was equipped, tell the bow to return the equipped arrow to the backpack
        if (GetComponent<BowItem>())
        {
            GetComponent<BowItem>().OnUnEquipBow();
        }

        // Enable the rigidbody
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = true;
        _parentTransform = null;  
    }
}

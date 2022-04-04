using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This component is present on bow prefab items. It handles the logic for the player interacting with 
/// an equipped bow item. 
/// </summary>
[RequireComponent(typeof(DamagableItem))]
public class BowItem : MonoBehaviour
{
    /// <summary>
    /// Reference to the scripable object that contains the data for this bow;
    /// </summary>
    [SerializeField] private BowItemScriptableObjectCreator _bowData;

    /// <summary>
    /// Reference the scriptable object for the arrow item. Allows us to tell the Inventory that we want to pull an
    /// item with this name identifier out of the player's backpack. 
    /// </summary>
    [SerializeField] private InventoryItemScriptableObjCreator _arrowData;

    /// <summary>
    /// Reference to the arrow that has been pulled out of the player backpack and equiped to this bow. 
    /// Null if no arrow equiped (e.g. between reloads or out of ammo).
    /// </summary>
    private GameObject _equippedArrow = null;

    /// <summary>
    /// Position of the arrow when drawn, this gets calculated based on the parameters in the _bowData scriptable object. 
    /// </summary>
    private Vector3 _drawnPos;

    /// <summary>
    /// Reference the rigidbody on the bow so we can toggle it with it gets equiped. 
    /// </summary>
    private Rigidbody _rb;

    /// <summary>
    /// The amount the bow is drawn, scaled from 0 to 1.
    /// </summary>
    private float _bowDrawAmount;


    /// <summary>
    /// Initialize components and local variables.
    /// </summary>
    private void Awake()
    {
        // Calculate the arrow drawn position based on the parameters in the _bowData scriptable object. 
        _drawnPos = _bowData.arrowEquippedPosition;
        _drawnPos.z += _bowData.drawArrowPosOffset;

        _rb = GetComponent<Rigidbody>();

        enabled = false;
    }
   

    /// <summary>
    /// Handle the player drawing/aiming/firing the bow. Trigger the appropriate animation based on the parameters in
    /// the _bowData scriptable object. Use the animation state to determine the draw amount/arrow position.
    /// </summary>
    private void Update()
    {
        if (_equippedArrow == null) return;
        // Set the draw amount of the arrow based on the current animation state.
        Player.PlayerControllerParent player = Player.PlayerControllerParent.GetInstance();
        if (player.IsAnimatorAtState(_bowData.drawBowAnimationStateName))
        {
            _bowDrawAmount = Mathf.Min(1, player.GetPercentThroughCurrentAnimation());
        }
        else if (player.IsAnimatorAtState(_bowData.aimBowAnimationStateName))
        {
            _bowDrawAmount = 1f;
        }
        else 
        {
            _bowDrawAmount = 0f;
        }

        // player the appropriate bow animation based on player input
        if (Input.GetKeyDown(KeyCode.Mouse0)) // player begins to draw the bow on key press (only allowed if idle)
        {
            if (player.IsAnimatorAtState(_bowData.bowIdleAniamationStateName))
            {
                player.SetAnimationTrigger(_bowData.drawBowAnimationTrigger);
            }
               
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) // player releases arrow if partially or fully drawn on key release.
        {
            if (_bowDrawAmount > 0)
            {
                player.SetAnimationTrigger(_bowData.fireBowAnimationTrigger);
                _bowDrawAmount = 0f;
                _equippedArrow.GetComponent<AmmoItem>().enabled = true;             
                _equippedArrow.GetComponent<AmmoItem>().Fire(Player.PlayerControllerParent.GetInstance().playerMovementComponent.GetLookDirection(), _bowData.arrowSpeed);            
                _equippedArrow = null;
                StartCoroutine(_LoadArrowCoroutine());
                return;
            }
        }

        if (_equippedArrow != null)
        {
            _equippedArrow.transform.localPosition = (1 - _bowDrawAmount) * _bowData.arrowEquippedPosition + _bowDrawAmount * _drawnPos;
        }
  
    }


    /// <summary>
    /// Called by the EquipItem component on the bow to tell this component the player just equiped the bow. 
    /// </summary>
    public void OnEquipBow()
    {
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _LoadArrowFromInventory();
    }


    /// <summary>
    /// Called by the EquipItem component on the bow to tell this component the player is uniequipping the bow
    /// </summary>
    public void OnUnEquipBow()
    {
        _ReturnEquippedArrowToInventory();
        enabled = false;
    }


    /// <summary>
    /// Called with player picks up an arrow or an arrow is crafted while the bow is equipped. 
    /// Triggers an auto reload if an arrow is not equipped because the player was out of ammo. 
    /// </summary>
    public void OnPickupArrow()
    {
        if (_equippedArrow == null)
        {
            _LoadArrowFromInventory();
        }
    }


    /// <summary>
    /// Takes an arrow out of the player's inventory backpack if there is at least one. Sets the value
    /// of _equippedArrow to reference this.
    /// </summary>
    private void _LoadArrowFromInventory()
    {
        _equippedArrow = InventoryController.instance.GetItemFromInventory(_arrowData.name);
        if (_equippedArrow == null)// out of ammo
        {
            return; 
        }
        _equippedArrow.transform.parent = transform;
        _equippedArrow.transform.localPosition = _bowData.arrowEquippedPosition;
        _equippedArrow.transform.localRotation = Quaternion.Euler(_bowData.arrowEquippedEulers);
        _equippedArrow.SetActive(true);
        _equippedArrow.GetComponent<Rigidbody>().isKinematic = true;
    }


    /// <summary>
    /// Called when player puts away the bow. Returns the currently equiped arrow to the player's inventory.
    /// </summary>
    public void _ReturnEquippedArrowToInventory()
    {

        if (_equippedArrow != null)
        {
            InventoryController.instance.AddItemToInventory(_equippedArrow.GetComponent<InventoryItem>());
        }
    }


    /// <summary>
    /// Coroutine to wait a small amount of time before calling the _LoadArrowFromInventory function.
    /// Time this to happen when the bow is out of the camera view during the reload phase of the shoot animation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator _LoadArrowCoroutine()
    {
        yield return new WaitForSeconds(.25f);
        _LoadArrowFromInventory();
    }
        

}

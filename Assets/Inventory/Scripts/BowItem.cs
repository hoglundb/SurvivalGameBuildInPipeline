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
    /// Called by the EquipItem component on the bow to tell this component the player just equiped the bow. 
    /// </summary>
    public void OnEquipBow()
    {
        _LoadArrowFromInventory();
    }


    /// <summary>
    /// Called by the EquipItem component on the bow to tell this component the player is uniequipping the bow
    /// </summary>
    public void OnUnEquipBow()
    {
        _ReturnEquippedArrowToInventory();
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
        Debug.LogError("Loading arrow "+_equippedArrow.name);
    }


    //Called when player puts away the bow. Returns the currently equiped arrow to the player's inventory. 
    public void _ReturnEquippedArrowToInventory()
    {

        if (_equippedArrow != null)
        {
            Debug.LogError("returning arrow to inventory");
            InventoryController.instance.AddItemToInventory(_equippedArrow.GetComponent<InventoryItem>());
        }
        else Debug.LogError("No equipped arrow");
    }

}

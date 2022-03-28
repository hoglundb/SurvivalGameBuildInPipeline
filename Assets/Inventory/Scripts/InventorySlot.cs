using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component is attached to each ui slot game object. A ui slot can belong to player backpack or equipables
/// </summary>
public class InventorySlot : MonoBehaviour
{
    /// <summary>
    /// The prefab for a inventory slot container that we attach to a slot and put inventory game object(s) into
    /// </summary>
    [SerializeField] private GameObject _inventorySlotItemContainerPrefab;

    /// <summary>
    /// The type of ui slot this is within the player inventory.
    /// </summary>
    [SerializeField] private InventorySlotTypesEnum _inventorySlotType;

    /// <summary>
    /// Color the border of the UI will be when item is selected
    /// </summary>
    [SerializeField] private Color _selectedColor;

    /// <summary>
    /// Color the border of the UI will be when item is not selected
    /// </summary>
    [SerializeField] private Color _nuetralColor;

    /// <summary>
    /// The current container attached to this inventory ui slot. It is null if this slot is empty.
    /// </summary>
    private GameObject _itemsContainer;

    /// <summary>
    /// Set to true if player has selected the item in this slot. False otherwise. 
    /// </summary>
    private bool _isSelected = false;


    /// <summary>
    /// Initalize some stuff
    /// </summary>
    private void Awake()
    {
        var childItem = GetComponentInChildren<InventoryItemContainer>();
        if (childItem != null)
        {
            _itemsContainer = childItem.gameObject;
        }
    }


    /// <summary>
    /// Returns the slot type for this slot.
    /// </summary>
    /// <returns>The value of the _inventorySlotType member</returns>
    public InventorySlotTypesEnum GetSlotType()
    {
        return _inventorySlotType;
    }


    /// <summary>
    /// Dereferences and returns the item container in this slot since getting it removes it from the slot. 
    /// </summary>
    /// <returns>The item container game object in the inventory slot</returns>
    /// <param name = "itemContainer" > A game object created from the inventory item container prefab.</param>
    public GameObject GetItemContainer(bool deReference)
    {
        GameObject itemContainer = _itemsContainer;
        if(deReference) _itemsContainer = null;
        return itemContainer;
    }


    /// <summary>
    /// Assignes an item container to this slot. This is called when the player is dragging an existing container from another slot to this one. 
    /// </summary>
    /// <param name="itemContainer">A game object created from the inventory item container prefab.</param>
    public void AssignItemContainer(GameObject itemContainer)
    {
        _itemsContainer = itemContainer;

        // Update the UI to show the count of the container that was assigned here
        itemContainer.GetComponent<InventoryItemContainer>().UpdateItemCountUI();

        // Reset the rect transform the spawned container so it sits in the slot as a child transform
        _ResetItemContainerRectTransform();
    }


    /// <summary>
    /// If this slot references a non-empty container (aka has item(s) in it) return true. Otherwise return false.
    /// </summary>
    /// <returns>True if _itemsContainer is set. Otherwise return false.</returns>
    public bool HasItems()
    {
        if (_itemsContainer == null || _itemsContainer.GetComponent<InventoryItemContainer>().GetItemCount() == 0) return false;
        return true;
    }


    /// <summary>
    /// Checks if the container already contains one or more item of the specified type. This function gets called to check
    /// what type if item(s) are already in this slot.
    /// </summary>
    /// <param name="itemTypeName">Name of the InventoryItem type as defined in it's scriptable object meta-data.</param>
    /// <returns>True if the container within this slot contains one or more items of the specified type. Returns false otherwise.</returns>
    public bool HasItemOfType(string itemTypeName)
    {
        // No items in this slot yet
        if (_itemsContainer == null) return false;

        if(_itemsContainer.GetComponent<InventoryItemContainer>().GetItemTypeName() == itemTypeName) return true;
        return false;
    }


    /// <summary>
    /// Called by the InventoryItemContainer class to add an item to the container in this slot. If slot is previously empty, a new container is spawned for this. 
    /// Only one non-stackable item allowed in a slot conatainer, but multiple stackable items can be combined.
    /// </summary>
    /// <param name="itemToAdd"></param>
    public void AddItemToSlot(InventoryItem itemToAdd)
    {
        // If no items in this clot (e.g. if no container to hold items is in this slot), create the new container for this slot.
        if (_itemsContainer == null)
        {
            // Create the new container and make a child of this slot
            _itemsContainer = Instantiate(_inventorySlotItemContainerPrefab);

            // Reset the rect transform the spawned container so it sits in the slot as a child transform
            _ResetItemContainerRectTransform();
        }

        //Assign the game object item to the container we just created
        _itemsContainer.GetComponent<InventoryItemContainer>().AddItemToContainer(itemToAdd);

        //Deactivate the game object that was just picked up
        itemToAdd.gameObject.SetActive(false);
    }


    /// <summary>
    /// Destroys this container. Generally this gets called with the last remaining item in the container is removed
    /// </summary>
    public void DestroyContainer()
    {
        if (_itemsContainer != null && _itemsContainer.GetComponent<InventoryItemContainer>().GetItemCount() == 0)
        {
            Destroy(_itemsContainer);
            _itemsContainer = null;
        }
    }


    /// <summary>
    /// Resets the rect transform component of the _itemsContainer game object in this inventory slot. 
    /// Also, set it to be a child of this slot. Call this when creating a new container in ths slot or
    /// when player drags an existing container from another slot into this one. 
    /// </summary>
    private void _ResetItemContainerRectTransform()
    {
        _itemsContainer.transform.parent = transform;

        //Reset the transform of the spawned items container so it sits inside this ui slot
        RectTransform itemRectTransform = _itemsContainer.GetComponent<RectTransform>();
        itemRectTransform.localPosition = Vector3.zero;
        itemRectTransform.localScale = Vector3.one;
    }


    /// <summary>
    /// Sets the UI border to the hightlight color to mark the item in this slot as selected. Also activates the item in the slot (if there is one) 
    /// if the UI slot is a quickSelect slot type.
    /// 
    /// Preconditions: Must call "DeSelectSlot()" on all other slots
    /// </summary>
    public void SelectSlot()
    {
        // Ignore if slot is already selected
        if (_isSelected) return;

        // Update the slot color to make it highlighted 
        GetComponent<Image>().color = _selectedColor;

        _isSelected = true;

        // Equip the item for the player if this slot is a quick-select slot type
        if (_inventorySlotType == InventorySlotTypesEnum.QUICK_SELECT)
        {
            if (HasItems())
            {
                _itemsContainer.GetComponent<InventoryItemContainer>().EquipItem();
            }
            else
            {
                Player.PlayerControllerParent.GetInstance().SetAnimationTrigger("Idle");
            }
        }
    }


    /// <summary>
    /// Sets the UI border to the nuetral color to mark the item in this slot as not being selected. 
    /// </summary>
    public void DeSelectSlot()
    {
        // Ignore if slot not selected
        if (!_isSelected) return;

        // Update the UI color
        GetComponent<Image>().color = _nuetralColor;

        _isSelected = false;

        // It slot we are de-selecting has an item in it, then tell the container to unEquip the item 
        if (HasItems())
        {
            _itemsContainer.GetComponent<InventoryItemContainer>().UnEquipItem();
        }
    }
}


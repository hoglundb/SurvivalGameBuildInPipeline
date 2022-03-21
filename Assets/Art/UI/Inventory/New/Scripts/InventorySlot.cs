using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    /// The current container attached to this inventory ui slot. It is null if this slot is empty.
    /// </summary>
    private GameObject _itemsContainer;



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

        // Reset the rect transform the spawned container so it sits in the slot as a child transform
        _ResetItemContainerRectTransform();
    }


    /// <summary>
    /// If this slot references a container (aka has item(s) in it) return true. Otherwise return false.
    /// </summary>
    /// <returns>True if _itemsContainer is set. Otherwise return false.</returns>
    public bool HasItems()
    {
        if (_itemsContainer == null) return false;
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
}


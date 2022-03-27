using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component is attached to any InventoryItemContainer prefab. This prefab is attached to an inventory slot (unless being 
/// dragged by the player) and contains the game object(s) of item(s) in the inventory slot.
/// </summary>
public class InventoryItemContainer : MonoBehaviour
{
    /// <summary>
    /// The child UI element that we assign a sprite to, based on what item type is going to be placed in this container.
    /// </summary>
    [SerializeField] private GameObject _containerImageChildObj;

    /// <summary>
    /// The child game object containing the item health bar for the item in this container
    /// </summary>
    [SerializeField] private GameObject _healthbarChildObj;

    /// <summary>
    /// The child game object containing the text for the item count in this container
    /// </summary>
    [SerializeField] private GameObject _itemCountChildObj;

    /// <summary>
    /// The textbox that shows the times symbol for the item count in this container.
    /// </summary>
    [SerializeField] private GameObject _itemCountTimesSymbolObj;

    /// <summary>
    /// List of inventory game objects in this slot
    /// </summary>
    private List<InventoryItem> _items;

    /// <summary>
    /// Reference the slider on this game object that indecates the health of the item it contains (if applicable to that item type)
    /// </summary>
    private Slider _itemHealthSlider;


    /// <summary>
    ///  Initailize references
    /// </summary>
    private void Awake()
    {
        _itemHealthSlider = GetComponent<Slider>();
    }


    /// <summary>
    /// Every frame, ensure that the slider value does't change. TODO: find a more efficient way to do this.
    /// </summary>
    private void Update()
    {
        _itemHealthSlider.value = 1f;
    }


    /// <summary>
    /// Called when player wants to add an item to this container within it's inventory slot. 
    /// </summary>
    /// <param name="itemTooAdd"></param>
    public void AddItemToContainer(InventoryItem itemTooAdd)
    {
        //If no items in this container yet, it must be the case that we are adding the first item.
        if (_items == null) 
        {
            // Instanciate the list to hold the item(s) in this container
            _items = new List<InventoryItem>();     

            // Assign the sprite image from the InventoryItem to the container since the container will only be holding items of that type.
            var itemImage = _containerImageChildObj.GetComponent<Image>();
           _containerImageChildObj.GetComponent<Image>().sprite = itemTooAdd.GetItemData().uiSpriteImage;
            itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 255);

            // If itemToAdd is stackable show the count but disable the health. Otherwise, do the opposite. 
            if (itemTooAdd.GetItemData().isStackable)
            {
                _healthbarChildObj.SetActive(false);
            }
            else 
            {
                _itemCountChildObj.SetActive(false);
                _itemCountTimesSymbolObj.SetActive(false);
                _itemHealthSlider.value = 1f;
            }
        }
        _items.Add(itemTooAdd);

        if (itemTooAdd.GetItemData().isStackable)
        {
            UpdateItemCountUI();
        }
    }


    /// <summary>
    /// Updates the UI text tracking item count for stackable items to be the number of elements in the _items list. 
    /// </summary>
    public void UpdateItemCountUI()
    {
        _itemCountChildObj.GetComponent<Text>().text = (_items.Count).ToString();
    }



    /// <summary>
    /// Gets the length of the _items list and returns it.
    /// </summary>
    /// <returns>The length of the list _items</returns>
    public int GetItemCount()
    {
        if (_items == null) return 0;

        return _items.Count;
    }


    /// <summary>
    /// Removes a subet of inventory items the _items list based on the value of SplitMode.  
    /// </summary>
    /// <param name="splitMode">Determines how large a subset to remove from _items list</param>
    /// <returns>A list of type InventoryItem that is a subset of _items</returns>
    public List<InventoryItem> ExtractItems(SplitMode splitMode)
    {
        List<InventoryItem> itemsToRemove = new List<InventoryItem>();
        int numItemsToRemove = 0;
        if (splitMode == SplitMode.HALF) numItemsToRemove = _items.Count / 2;
        else if (splitMode == SplitMode.ONE) numItemsToRemove = 1;
        Debug.LogError(numItemsToRemove.ToString());
        for (int i = 0; i < numItemsToRemove; i++)
        {
            itemsToRemove.Add(_items[i]);
        }
        for (int i = 0; i < numItemsToRemove; i++)
        {
            _items.Remove(_items[i]);
        }

        UpdateItemCountUI();
        return itemsToRemove;
    }


    /// <summary>
    /// Returns the list of inventory items that are in this slot. 
    /// </summary>
    /// <returns> Returns the list of inventory items that are in this slot. </returns>
    public List<InventoryItem> GetItems()
    {
        return _items;
    }


    /// <summary>
    /// Returns the name field (from the InventoryItem scriptable object) of the item(s) currently being stored in this slot. This is called
    /// to determine what item type is currently occupying this slot.
    /// </summary>
    /// <returns>Returns the name field (from the InventoryItem scriptable object) of the item(s) currently being stored in this slot. </returns>
    public string GetItemTypeName()
    {
        if (_items == null || _items.Count == 0)
        {
            return null;
        }

        return _items[0].GetItemData().name;
    }


    /// <summary>
    /// Called when we are combining the item(s) from another conatiner into this one. If all the items could successfully be dumped into this container,
    /// then destroy the empty container and return null. Otherwise, return the container with the remaining items in it.
    /// </summary>
    /// <param name="container">The container containing items we combine with the items in this container</param>
    /// <returns>Returns a conatainer with any remaining items that didn't have room to be dumped in this container</returns>
    public InventoryItemContainer DumpItemsFromOtherContainer(InventoryItemContainer container)
    {
        int numSlotsLeft = GetContainerRemainingCapacity();
        Debug.LogError("Slots left: " + numSlotsLeft.ToString());
        int curCount = 0;
        var items = container.GetItems();
        foreach (var item in items)
        {
            if (curCount == numSlotsLeft) break;
            AddItemToContainer(item);           
            curCount++;
        }

        container.GetItems().RemoveRange(0, curCount);
        if (container.GetItemCount() > 0) return container;

        // All items from other container had room to be added to this, so destroy the other empty container. 
        Destroy(container.gameObject);
        return null;
    }



    /// <summary>
    /// Checks how many more items (of the type currently in this slot) can be added to it. 
    /// </summary>
    /// <returns>Returns an int representing how many more items (of the type currently in this slot) can be added to this container.
    public int GetContainerRemainingCapacity()
    {
        int maxCap = GetContainerMaxCapacity();
        return maxCap - _items.Count;        
    }


    /// <summary>
    /// Checks a item in this container and looks at it's data to determine how many items of that type are allowed in a single inventory slot container.
    /// If no items in the container yet, return -1.
    /// </summary>
    /// <returns>An integer representing the number of items this container can hold at it's current state.</returns>
    public int GetContainerMaxCapacity()
    {
        if (_items == null || _items.Count == 0) return -1;
        return _items[0].GetItemData().maxItemsPerInventorySlot;
    }


    /// <summary>
    /// Removes an item from the _items list. This is generally called as part of dumping items from this container into another container. 
    /// </summary>
    public void RemoveOneItem()
    {
        if (_items != null && _items.Count > 0)
        {
            _items.RemoveAt(0);
        }    
    }


    /// <summary>
    /// Called on a Quick-Select slot type to tell the player to equip the item in this slot
    /// Preconditions:
    ///    1) Slot must not be empty
    ///    2) Slot must contain a non-stackable and equipable item type. 
    /// </summary>
    public void EquipItem()
    {
        // Verify the expected type of item is in this slot
        if (_items == null || _items.Count != 1)
        {
            Debug.LogError("Container expected to have one equipable item in it");
            return;
        }
        InventoryItem itemToEquip = _items[0];

        // Enable the EquipableItem component on this game object to tell it to equip it self to the player.
        itemToEquip.gameObject.SetActive(true);
        itemToEquip.gameObject.GetComponent<EquipableItem>().enabled = true;
    }


    /// <summary>
    /// Called on a Quick-Select slot type to tell the player to un-equip the item in this slot
    /// </summary>
    public void UnEquipItem()
    {
        InventoryItem itemToEquip = _items[0];
        itemToEquip.GetComponent<EquipableItem>().enabled = false;
        itemToEquip.gameObject.SetActive(false);
    }


    /// <summary>
    /// Determines if the item(s) in this container are stackable by looking at the scriptable object on this item's InventoryItem component.
    /// </summary>
    /// <returns>True if this container has one or more stackable items in it. Returns false otherwise.</returns>
    public bool ContainsStackableItems()
    {
        if (_items == null || _items.Count == 0) return false;

        return _items[0].GetItemData().isStackable;
    }
}

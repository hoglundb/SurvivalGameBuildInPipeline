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
           _containerImageChildObj.GetComponent<Image>().sprite = itemTooAdd.GetItemInfo().uiSpriteImage;
            itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 255);

            // If itemToAdd is stackable show the count but disable the health. Otherwise, do the opposite. 
            if (itemTooAdd.GetItemInfo().isStackable)
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

        if (itemTooAdd.GetItemInfo().isStackable)
        {
            _UpdateItemCountUI();
        }
    }


    /// <summary>
    /// Updates the UI text tracking item count for stackable items to be the number of elements in the _items list. 
    /// </summary>
    private void _UpdateItemCountUI()
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

        _UpdateItemCountUI();
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

        return _items[0].GetItemInfo().name;
    }


    /// <summary>
    /// Called when we are combining the item(s) from another conatiner into this one. 
    /// </summary>
    /// <param name="container">The container containing items we combine with the items in this container</param>
    public void DumpItemsFromOtherContainer(InventoryItemContainer container)
    {
        var items = container.GetItems();
        foreach (var item in items)
        {
            AddItemToContainer(item);
        }

        Destroy(container.gameObject);
    }

}

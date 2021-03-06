using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// This component to control and manage the player's inventory. It acts as a singleton class and is attached to the 'PlayerInventoryUIPanel' canvas game object. 
/// </summary>
public class InventoryController : MonoBehaviour
{
    /// <summary>
    /// Offset from the mouse position for when the "SplitInventoryItemsTooltip" is activated on an inventory slot container.
    /// </summary>
    [SerializeField] private Vector3 _splitItemsTooltipOffset;

    /// <summary>
    /// Tracks if the player's inventory is open. True if the player has the inventory open. False otherwise. 
    /// </summary>
    private bool _isInventoryActive = false;

    /// <summary>
    /// This gets set to the item container (within an inventory slot) that the player has selected. Null if non currently selected.
    /// </summary>
    private GameObject _selectedItemContainer;

    /// <summary>
    /// Reference the Canvas game object so we can manage placement of the inventory item container as the player clicks and drags it.
    /// </summary>
    private GameObject _canvas;    

    /// <summary>
    /// The inventory ui panel rectangle transform. We use the pivot point on this to show/hide the inventory based on player input.
    /// </summary>
    private RectTransform _backpackUIRectTransform;

    /// <summary>
    /// Singleton reference to this class so that other inventory components can globally access it.
    /// </summary>
    public static InventoryController instance;

    /// <summary>
    /// Holds a list of the backpack ui slots in the player's inventory.
    /// </summary>
    private List<InventorySlot> _backpackSlots;

    /// <summary>
    /// Holds the list of the quick select ui slots in the player's inventory.
    /// </summary>
    private List<InventorySlot> _quickSelectSlots;
    private int _curSelectedItemIndex = -1;

    /// <summary>
    /// Tracks the slot the player pulled a container out of to click and drag it to a new slot. We do this so that if the player tries to drag a container
    /// to a new slot that is invalid, we can snap the container back to the origonal slot. 
    /// </summary>
    private InventorySlot _prevousInventorySlot;

    /// <summary>
    /// Set up references to other game objects in the scene.
    /// </summary>
    private void Awake()
    {
        instance = this;
        _canvas = GameObject.Find("Canvas");
        _backpackUIRectTransform = GameObject.Find("BackpackUIPanel").GetComponent<RectTransform>();

        // Set up references to the ui slots for both the backpack and quickSelect slots.
        _backpackSlots = new List<InventorySlot>();
        _quickSelectSlots = new List<InventorySlot>();
        var slots = GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetSlotType() == InventorySlotTypesEnum.BACKPACK)
            {
                _backpackSlots.Add(slots[i]);
            }
            else if (slots[i].GetSlotType() == InventorySlotTypesEnum.QUICK_SELECT)
            {
                _quickSelectSlots.Add(slots[i]);
            }
        }
    }


    /// <summary>
    /// Initalize the state of the inventory.
    /// </summary>
    private void Start()
    {
        _isInventoryActive = false;
        _UpdateInventoryVisibility();
    }


    /// <summary>
    /// Every frame we check for player input related to opening/closing the inventory and respond as needed. 
    /// If inventory is open, we then manage any interactions between the player and the inventory.
    /// </summary>
    private void Update()
    {
        //Respond to player input to select a weapon/tool in the quickselect slots, but only if inventory is not open
        if (!_isInventoryActive)
        {
            int newSelectItemIndex = _curSelectedItemIndex;
            if (Input.GetKeyDown(KeyCode.Alpha1)) newSelectItemIndex = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) newSelectItemIndex = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) newSelectItemIndex = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4)) newSelectItemIndex = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5)) newSelectItemIndex = 4;
            if (Input.GetKeyDown(KeyCode.Alpha6)) newSelectItemIndex = 5;
            if (_curSelectedItemIndex != newSelectItemIndex)
            {
                _OnPlayerQuickSelectItem(newSelectItemIndex);
            }
            _curSelectedItemIndex = newSelectItemIndex;
        }        

        // Respond to player input to open/close the inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
                _isInventoryActive = !_isInventoryActive;
                _UpdateInventoryVisibility();
        }

        // if tooltip is open, allow player to close it by clicking, but don't respond to other clicks to inventory items. 
        if (SplitInventoryItemsTooltip.instance.IsInUse() && !SplitInventoryItemsTooltip.instance.IsCursorOverTooltip())
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                SplitInventoryItemsTooltip.instance.Hide();
            }
            return;
        }


        //Respond to mouse input for an open inventory
        if (_isInventoryActive && !SplitInventoryItemsTooltip.instance.IsInUse())
        {
            _ManagePlayerInventoryInteraction();

            //If currently dragging an inventoryContainer item, update it to the position of the cursor
            if (_selectedItemContainer != null)
            {
                _selectedItemContainer.transform.position = Input.mousePosition;
            }
        }       
    }


    /// <summary>
    /// Called from the EnvironmentDetector component when player wishes to pick up an item. 
    /// This function manages placing the item into the player's inventory based on inventory space and 
    /// if the item being added is stackable or not. 
    /// </summary>
    /// <param name="itemToAdd">The InventoryItem component on the game object of item that was just picked up</param>
    public void AddItemToInventory(InventoryItem itemToAdd)
    {
        //Get the ideal slot to add this item to. If no room in backpack then return.
        InventorySlot slotToAddItemTo = _GetBackpackSlotToAddItemTo(itemToAdd);
        if (slotToAddItemTo == null)
        {
            Debug.LogError("No room in inventory!!!");
            return;
        }
        slotToAddItemTo.AddItemToSlot(itemToAdd);

        // special case when item added is ammo, trigger an automatic reload
        if (itemToAdd.gameObject.GetComponent<AmmoItem>())
        {
            InventoryItem equippedItem = _GetEquippedItem();
            if (equippedItem != null)
            {
                if (equippedItem.gameObject.GetComponent<BowItem>())
                {
                    equippedItem.gameObject.GetComponent<BowItem>().OnPickupArrow();
                }
            }           
        }   
    }


    /// <summary>
    /// Returns the Inventory item object that is currently equipped in a quickselect slot. Returns null if none. 
    /// </summary>
    private InventoryItem _GetEquippedItem()
    {
        if (_curSelectedItemIndex <= 5 && _curSelectedItemIndex >= 0)
        {
            InventorySlot equippedSlot = _quickSelectSlots[_curSelectedItemIndex];
            if (equippedSlot.HasItems())
            {
                return equippedSlot.GetItemContainer(false).GetComponent<InventoryItemContainer>().GetItems()[0];
            }
        }
        return null;
    }


    /// <summary>
    /// Searches the player invetory for an item with the specified name. If one or more of that item is present in the player's backpack.
    /// then remove that item from the inventory and return it. 
    /// </summary>
    /// <param name="itemName">name identifier of the item type, as referenced in the itemData scriptable object on an inventory item.</param>
    public GameObject GetItemFromInventory(string itemName)
    {
        foreach (var item in _backpackSlots)
        {
            if (item.HasItemOfType(itemName))
            {
                GameObject removedItem = item.GetItemContainer(false).GetComponent<InventoryItemContainer>().RemoveOneItem();

                // If the last item was taken out, delete the tell the item slot to delete it's empty container.
                if (!item.HasItems())
                {
                    item.DestroyContainer();
                }
                return removedItem;
            }
        }
        return null;
    }


    /// <summary>
    /// Called from the UI tooltip when player wishes to split a stack of items in the specified inventory slot contianer.
    /// </summary>
    /// <param name="slotContainerReference">Reference to the slot container we are going to split items from.</param>
    /// <param name="splitMode">Determines if stack will be split in half or just have one piece taken off</param>
    public void SplitStackableItem(InventoryItemContainer slotContainerReference, SplitMode splitMode)
    {
        // Get the first empty slot which is were the split items get put into.
        InventorySlot emptySlotToSplitItemsInto = GetFirstEmptyBackpackSlot();
        if (emptySlotToSplitItemsInto == null) return; // TODO: provide the player with some feedback for this. 

        // Remove the items we are splitting from the current container
        List<InventoryItem> splitItems = slotContainerReference.ExtractItems(splitMode);

        // Put the split items into the a new empty continer in the first empty slot in the player backpack.
        foreach (InventoryItem item in splitItems)
        {
            emptySlotToSplitItemsInto.AddItemToSlot(item);
        }
    }


    /// <summary>
    /// Iterates through the list of backpack slots and returns a reference to the first one that is empty. Will return null if no emptly slots exist.
    /// </summary>
    /// <returns>A reference to an InventorySlot beloging to the _backpackSlots list</returns>
    private InventorySlot GetFirstEmptyBackpackSlot()
    {
        foreach (var slot in _backpackSlots)
        {
            if (!slot.HasItems())
            {
                return slot;
            }
        }
        return null;
    }


    /// <summary>
    /// Gets the first available backpack slot to add the specified inventory item to. If the item is non-stackable, return the first 
    /// empty backpack slot. If stackable, first check if there is a partially full slot with items of the same type. 
    /// </summary>
    /// <param name="itemToAdd">The InventoryItem component on the game object being added to the player's backpack</param>
    private InventorySlot _GetBackpackSlotToAddItemTo(InventoryItem itemToAdd)
    {
        // If stackable, first try to add to a non-full existing slot with items of the same type
        if (itemToAdd.GetItemData().isStackable)
        {
            // Check if a partially full slot is present with an item of this type so that adding a stackable item stacks it by default.
            foreach (var backpackSlot in _backpackSlots)
            {
                if (backpackSlot.HasItems() && backpackSlot.HasItemOfType(itemToAdd.GetItemData().name))
                {
                    if (backpackSlot.GetItemContainer(false).GetComponent<InventoryItemContainer>().GetContainerRemainingCapacity() > 0)
                    {
                        return backpackSlot;
                    }                   
                }
            }
        }

        //If non stackable or stackable but with no partially full slots, then find the first empty slot. 
        foreach (var backpackSlot in _backpackSlots)
        {
            if (!backpackSlot.HasItems())
            {
                return backpackSlot;
            }
        }

        // Returning null indecates that slot was found and the backpack does not have room the the item the player is trying to add. 
        return null;
    }


    /// <summary>
    /// Called every frame that the invetory is open, in order to manage player input on the inventory and respond accordingly.
    /// </summary>
    private void _ManagePlayerInventoryInteraction()
    {
        // player keys down on a slot, if the slot contains an container, then register a click on it, allowing the player to drag it.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject hoverUISlotGameObj = _GetPointerHoverInventorySlot();
            if (hoverUISlotGameObj != null)
            {
                _OnPlayerClickInventorySlot(hoverUISlotGameObj);
            }
        }

        // Player keys up on a slot while dragging a container, register the player dropped the dragged container into another slot. 
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_selectedItemContainer != null)
            {
                _OnPlayerDropSelectedInventoryItems();
            }
        }

        // Player right clicks on a slot with a container, bring up the tool tip to allow player to split the items (only applies to stackable inventory items).
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (_selectedItemContainer == null)
            {
                GameObject hoverUISlotGameObj = _GetPointerHoverInventorySlot();
                if (hoverUISlotGameObj != null)
                {
                    // Get the container attched to this slot. Will be null if the inventory slot is empty
                    var itemContainerInSlot = hoverUISlotGameObj.GetComponent<InventorySlot>().GetItemContainer(deReference: false);
                    if (itemContainerInSlot != null)
                    {
                        // Check the the slot contains multiple items, and we can therefore allow the player to split those items using the tooltip
                        InventoryItemContainer containerComponent = itemContainerInSlot.GetComponent<InventoryItemContainer>();
                        if (containerComponent.GetItemCount() > 1)
                        {
                            _OnPlayerActivateSplitItemTooltip(containerComponent);
                        }
                    }                    
                }               
            }
        }
    }


    /// <summary>
    /// Returns the inventory slot ui game object the player's cursor is hovering over. If not hovering over
    /// an inventory slot, return null.
    /// </summary>
    /// <returns>Returns the inventory slot ui game object the player's cursor is hovering over</returns>
    private GameObject _GetPointerHoverInventorySlot()
    {
        //Get the raycast data from the event system
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        for (int index = 0; index < raysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raysastResults[index];
            if (curRaysastResult.gameObject.GetComponent<InventorySlot>())
            {
                return curRaysastResult.gameObject;
            }
        }
        return null;
    }


    /// <summary>
    /// Sets the visibility of the inventory UI panel based on the value of _isInventoryActive. The ui panel is toggled by setting
    /// it's pivot point on it's rectangle transform.
    /// </summary>
    private void _UpdateInventoryVisibility()
    {
        Player.PlayerControllerParent.GetInstance().playerMovementComponent.enabled = !_isInventoryActive;

        // Player just opened inventory so make it visible and uniquip current item
        if (_isInventoryActive)
        {
            _backpackUIRectTransform.pivot = new Vector2(_backpackUIRectTransform.pivot.x, .5f);
            _DeSelectCurrentSlot();
            Player.PlayerControllerParent.GetInstance().SetAnimationTrigger("Idle");
            _curSelectedItemIndex = -1;
        }
        // Player just closed inventory so make it invisible
        else
        {
            _backpackUIRectTransform.pivot = new Vector2(_backpackUIRectTransform.pivot.x, 10f);
        }
    }


    /// <summary>
    /// Called when the player releases an inventory item container they are clicking/dragging. If player's cursor is over a valid slot, 
    /// then this item is dropped into that slot. Otherwise, the item is dropped onto the ground. 
    /// </summary>
    private void _OnPlayerDropSelectedInventoryItems()
    {
        // If player was dragging items over a empty slot, then drop the items if the slot can accomidate it.
        if (_selectedItemContainer != null)
        {
            GameObject hoverSlot = _GetPointerHoverInventorySlot();

            // If hovering over a valid slot, the dropped item(s) go into that slot
            if (hoverSlot != null)
            {
                InventorySlot slotComponent = hoverSlot.GetComponent<InventorySlot>();

                //If attempting to drop a non-equipable item into a quickselect slot, don't allow it. Snap the item back to it's original slot. 
                if (slotComponent.GetSlotType() == InventorySlotTypesEnum.QUICK_SELECT)
                {
                    if (!_selectedItemContainer.GetComponent<InventoryItemContainer>().ContainerEquipableItem())
                    {
                        _prevousInventorySlot.AssignItemContainer(_selectedItemContainer);
                        _selectedItemContainer = null;
                        return;
                    }
                }

                // If slot is empty, then drop the entire item container into the slot
            
                if (!slotComponent.HasItems())
                {
                    slotComponent.AssignItemContainer(_selectedItemContainer);
                    _selectedItemContainer.transform.position = slotComponent.transform.position;
                }

                // If slot is not empty, verify that the selected container's items can be added to it. If so, can dump the items out of the current container into the new slot. Then destroy the current container since it is empty
                else
                {
                    // Check the the selected container's items are the same type and will fit into the slot the player is trying to put them into.
                    InventoryItemContainer hoverSlotContainer = slotComponent.GetItemContainer(deReference: false).GetComponent<InventoryItemContainer>();
                    InventoryItemContainer currentContainer = _selectedItemContainer.GetComponent<InventoryItemContainer>();
                    if (hoverSlotContainer.ContainsStackableItems() && currentContainer.GetItemTypeName() == hoverSlotContainer.GetItemTypeName())
                    {                        
                       var remaining = hoverSlotContainer.DumpItemsFromOtherContainer(currentContainer);
                        if (remaining != null)
                        {
                            _prevousInventorySlot.AssignItemContainer(remaining.gameObject);
                        }
                    }

                    //Not a valid slot to dump items into. Snap the container back to the slot it was in before player moved and dragged it. 
                    else 
                    {
                        _prevousInventorySlot.AssignItemContainer(currentContainer.gameObject);
                    }
                }
            }
        }

        // De-reference the container since player has dropped it.
        _selectedItemContainer = null;
    }


    /// <summary>
    /// Called when player clicks on an inventory slot. If slot is not empty, then we assign it to the _selectedItemContainer so it will follow the player's 
    /// cursor until they mouseup.
    /// </summary>
    /// <param name="clickedInvnetorySlot">The ui inventory slot game object the player clicked on.</param>
    private void _OnPlayerClickInventorySlot(GameObject clickedInvnetorySlot)
    {

        _prevousInventorySlot = clickedInvnetorySlot.GetComponent<InventorySlot>();
        var itemContainerInSlot = _prevousInventorySlot.GetItemContainer(true);
        if (itemContainerInSlot != null)
        {
            _selectedItemContainer = itemContainerInSlot;
            _selectedItemContainer.transform.parent = _canvas.transform;
        }

    }


    /// <summary>
    /// Called when player right clicks an inventory slot containing a container. Manages the logic for the tooltip allowing the player to split the items.
    /// </summary>
    /// <param name="clickedInventorySlot"></param>
    private void _OnPlayerActivateSplitItemTooltip(InventoryItemContainer clickedInventorySlotContainer)
    {
        SplitInventoryItemsTooltip tooltipInstance = SplitInventoryItemsTooltip.instance;

        //Set the tooltip to be in use and position where the player clicked inside the inventory. 
        tooltipInstance.Show(Input.mousePosition + _splitItemsTooltipOffset);

        //Register the event handlers for the tooltip buttons and bind them to the event handlers in the clicked inventory slot container.
        tooltipInstance.currentContainer = clickedInventorySlotContainer;
    }


    /// <summary>
    /// Called when the player selects a new quick select item. Equip that item for the player after unequiping the previous item.
    /// </summary>
    /// <param name="slotIndex"></param>
    private void _OnPlayerQuickSelectItem(int slotIndex)
    {
        _DeSelectCurrentSlot();
        _quickSelectSlots[slotIndex].GetComponent<InventorySlot>().SelectSlot();
    }


    /// <summary>
    /// Calls "DeSelectSlot()" on all quick select slots so that if one is selected it gets deselected
    /// </summary>
    private void _DeSelectCurrentSlot()
    {
        foreach (var slot in _quickSelectSlots)
        {
            slot.GetComponent<InventorySlot>().DeSelectSlot();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        //References to other UI components related to the inventory. 
        [Header("UI Panel References")]
        [SerializeField] private GameObject _inventoryUISlotPrefab;
        [SerializeField] private GameObject _inventorySlotUIGrid;
        [SerializeField] private GameObject _scrollbarGameObj;

        //References to the filter buttons on the inventory 
        [Header("Btn References")]
        [SerializeField] private GameObject _btnFilterAll;
        [SerializeField] private GameObject _btnFilterWeapons;
        [SerializeField] private GameObject _btnFilterTools;
        [SerializeField] private GameObject _btnFilterMaterials;
        [SerializeField] private GameObject _btnFilterConsumables;

        //The hieght to increase the UI panel by each time an item is added. 
        [Header("UI Panel Slot Height")]
        [SerializeField] private float _itemSlotHeight;

        //Tracks the currently selected filter for the inventory. Allows us to avoid redundant sorting and save on CPU resources. 
        private PickupableItemCategory _currentItemCategoryFilter; 

        //Tracks if the inventory is showing. Show/hide functionallity is managed by scaling down the UI game object to zero
        private bool _isShowing = true;

        //The rect transform component on this game object
        private RectTransform _rectTransform;

        //Holds a list of UI elements for each inventory item. Each UI item in turn, contains the coorisponding item game object. 
        private List<GameObject> _inventoryItemSlots;

        //Counts the total number of active slots. Note, may be different from the size of _inventoryItemSlots since slots can be deactivate and reused. 
        private int _itemCount;
   

        //Reference to the crafting panel. Need to toggle this off when toggling the inventory panel 
        private CraftingManager _craftingManagerComponent;

        private void Awake()
        {
            _craftingManagerComponent = GameObject.Find("PlayerCraftingPanel").GetComponent<CraftingManager>();

            _inventoryItemSlots = new List<GameObject>();

            _rectTransform = GetComponent<RectTransform>();

            //register event handlers for the inventory filter buttons. 
            _btnFilterAll.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByAll);
            _btnFilterWeapons.GetComponent<Button>().onClick.AddListener(_OnBtnClickFiterByWeapons);
            _btnFilterTools.GetComponent<Button>().onClick.AddListener(_OnBtnClickFiterByTools);
            _btnFilterMaterials.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByMaterials);
            _btnFilterConsumables.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByConsumables);

            //Start out with the inventory UI hidden
            if (_isShowing)
            {
                ToggleVisibility();
            }         
        }


        //Called by the player when they wish to add an item to the inventory. If an inactive slot is available, us that. Otherwise instaciate a new slot. 
        public void AddItemToInventory(GameObject inventoryGameObj)
        {
            _itemCount++;

            //Init the new UI item to hold this inventory game object. Dynamically adjust the UI height to accomidate the new item.
            GameObject slotItem = _GetFirstUnusedSlot();

            //if no unused slot, create a new one, otherwise the unused one is re-initialized for use. 
            bool isReusingSlot = true;
            if (slotItem == null)
            {
                isReusingSlot = false;
                slotItem = Instantiate(_inventoryUISlotPrefab);
                slotItem.transform.parent = _inventorySlotUIGrid.transform;
            }
            else 
            {
                slotItem.gameObject.SetActive(true);
            }

            //Initalize the slot UI and expand the UI scroll area to accomidate the new item.
            slotItem.GetComponent<RectTransform>().localScale = Vector3.one;
            _IncrementUIScrollAreaHeight();

            //Tell the slot to reference the game object that is being placed in this slot
            slotItem.GetComponent<InventorySlotManager>().InitSlot(inventoryGameObj, _inventoryItemSlots.Count);

            //It we had to add a new slot, update the list that references all the inventory item slots. 
            if (!isReusingSlot)
            {
                _inventoryItemSlots.Add(slotItem);
            }       
        }


        //Returns the first unused slot in the list of inventory slots. This allows us to reuse slots and only create more when needed. 
        private GameObject _GetFirstUnusedSlot()
        {
            foreach (var s in _inventoryItemSlots)
            {
                if (s.gameObject.activeSelf == false) return s;
            }
            return null;
        }


        //Sets the scroll hight based on the number of items in the inventry list. 
        private void _IncrementUIScrollAreaHeight()
        {
            RectTransform rt = _inventorySlotUIGrid.GetComponent<RectTransform>();
            Vector2 curSizeDelta = rt.sizeDelta;
            curSizeDelta.y += _itemSlotHeight;
            rt.sizeDelta = curSizeDelta;
        }


        //Find the inventory slot with the matching ID and deactivate it. Note, it will still be in the _inventoryItemSlots list, but the game object will be inactive.
        public void RemoveItemFromInventory(int slotID)
        {
            for (int i = 0; i < _inventoryItemSlots.Count; i++)
            {
                if (_inventoryItemSlots[i].GetComponent<InventorySlotManager>()._slotID == slotID)
                {
                    _inventoryItemSlots[i].gameObject.SetActive(false);
                    return;
                }
            }
        }


        //Toggles the visiblity of the Inventory UI. 
        public bool ToggleVisibility()
        {
            _isShowing = !_isShowing;
            return _UpdateVisibility();           
        }


        public bool SetVisibility(bool makeVisible)
        {
            _isShowing = makeVisible;
            return _UpdateVisibility();
        }


        //Called when the _isShowing value is changed. Updates the UI accordingly.
        private bool _UpdateVisibility()
        {

            if (_isShowing)
            {
                //Turn off the crafting panel UI if it is showing
                _craftingManagerComponent.SetVisibility(false);

                //Scale the UI back to 1 so it is visible
                _rectTransform.localScale = Vector3.one;
                _ResetChildTransforms();
                return true;
            }
            else
            {
                Vector3 hiddenScale = _rectTransform.localScale;
                hiddenScale.x = 0f;
                _rectTransform.localScale = hiddenScale;
                return false;
            }
        }


        //Resets the scale of the UI items in the enventory. Call this when re-enabling the inventory UI and scaling it back to 1. 
        private void _ResetChildTransforms()
        {
            foreach (Transform child in _inventorySlotUIGrid.transform)
            {
                child.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }


        //Filters the player inventory by the specified item type. This gets called when one of the filter buttons is clicked. 
        private void _FilterInventoryByItemType(PickupableItemCategory itemCategory)
        {
            //Ignore if filter is already selected
            if (_currentItemCategoryFilter == itemCategory) return;

            foreach (var slotItem in _inventoryItemSlots)
            {
                ItemInteraction itemInteractionComponent = slotItem.GetComponent<InventorySlotManager>().GetAttachedGameObj().GetComponent<ItemInteraction>();
                if (itemCategory == PickupableItemCategory.ALL || itemInteractionComponent.GetItemCategory() == itemCategory)
                {
                    slotItem.SetActive(true);
                }
                else 
                {
                    slotItem.SetActive(false);
                }
            }
        }


        #region Btn Event Handlers

        private void _OnBtnClickFilterByAll()
        {
            _FilterInventoryByItemType(PickupableItemCategory.ALL);
        }


        private void _OnBtnClickFiterByWeapons()
        {
            _FilterInventoryByItemType(PickupableItemCategory.WEAPON);
        }


        private void _OnBtnClickFiterByTools()
        {
            _FilterInventoryByItemType(PickupableItemCategory.TOOLS);
        }


        private void _OnBtnClickFilterByMaterials()
        {
            _FilterInventoryByItemType(PickupableItemCategory.MATERIAL);
        }


        private void _OnBtnClickFilterByConsumables()
        {
            _FilterInventoryByItemType(PickupableItemCategory.CONSUMABLE);
        }


        #endregion

    }


}



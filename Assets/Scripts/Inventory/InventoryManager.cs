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


        private void Awake()
        {
            _inventoryItemSlots = new List<GameObject>();

            _rectTransform = GetComponent<RectTransform>();

            //register event handlers for the inventory filter buttons. 
            _btnFilterAll.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByAll);
            _btnFilterWeapons.GetComponent<Button>().onClick.AddListener(_OnBtnClickFiterByWeapons);
            _btnFilterTools.GetComponent<Button>().onClick.AddListener(_OnBtnClickFiterByTools);
            _btnFilterMaterials.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByMaterials);
            _btnFilterConsumables.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByConsumables);
        }


        //Called by the player when they wish to add an item to the inventory. 
        public void AddItemToInventory(GameObject inventoryGameObj)
        {
            //Init the new UI item to hold this inventory game object. Dynamically adjust the UI height to accomidate the new item. 
            GameObject newItem = Instantiate(_inventoryUISlotPrefab);
            newItem.transform.parent = _inventorySlotUIGrid.transform;    
            newItem.GetComponent<RectTransform>().localScale = Vector3.one;
            _SetRectHeightBasedOnItemCount();

            //The UI slot with own the actual inventory game object until dropped or equiped by the player. 
            newItem.GetComponent<InventorySlotManager>().InitSlot(inventoryGameObj);

            //Update the list that references all the inventory item slots. 
            _inventoryItemSlots.Add(newItem);
        }


        //Sets the scroll hight based on the number of items in the inventry list. 
        private void _SetRectHeightBasedOnItemCount()
        {
            RectTransform rt = _inventorySlotUIGrid.GetComponent<RectTransform>();
            Vector2 curSizeDelta = rt.sizeDelta;
            curSizeDelta.y += _itemSlotHeight;
            rt.sizeDelta = curSizeDelta;
        }


        //TODO
        public void TryRemoveItemFromInventory()
        {

        }


        //Toggles the visiblity of the Inventory UI. 
        public void ToggleVisibility()
        {
            _isShowing = !_isShowing;

            if (_isShowing)
            {
                _rectTransform.localScale = Vector3.one;
                _ResetChildTransforms();
            }
            else 
            {
                Vector3 hiddenScale = _rectTransform.localScale;
                hiddenScale.x = 0f;
                _rectTransform.localScale = hiddenScale;
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



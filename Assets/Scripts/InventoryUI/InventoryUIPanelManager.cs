using Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Inventory
{
    public class InventoryUIPanelManager : MonoBehaviour
    {

        [Header("Parent slot containing inventory slots")]
        [SerializeField] private GameObject _uiInventorySlotContent;

        [Header("UI Slot Prefabs to spawn")]
        [SerializeField] private GameObject _weaponOrToolUISlotPrefab;

        [Header("Filter Buttons")]
        [SerializeField] private GameObject _materialsFilterBtn;
        [SerializeField] private GameObject _ammoFilterBtn;
        [SerializeField] private GameObject _foodFilterBtn;
        [SerializeField] private GameObject _clothingFilterBtn;
        [SerializeField] private GameObject _weaponsAndToolsFilterBtn;

        private List<GameObject> _uiSlots;

        //Treat this class as a singleton since only ever one inventory for the player
        private static InventoryUIPanelManager _instance;
        public static InventoryUIPanelManager GetInstance()
        {
            return _instance;
        }

        private UIWeaponOrToolSlot _currentEquipedItemUISlot;



        private void Awake()
        {
            _instance = this;

            _uiSlots = new List<GameObject>();
            foreach (Transform t in _uiInventorySlotContent.transform)
            {
                 _uiSlots.Add(t.gameObject);
            }

            _InitInventoryFilterButtons();
            _UpdateInventoryUI();
        }



        //Sets up the event listeners for the inventory filter buttons
        private void _InitInventoryFilterButtons()
        {
            _materialsFilterBtn.GetComponent<Button>().onClick.AddListener(delegate { _FilterInventoryByCategory(typeof(UIMaterialSlot));});
            _ammoFilterBtn.GetComponent<Button>().onClick.AddListener(delegate { _FilterInventoryByCategory(typeof(UIAmmoSlot)); });
            //TODO: add the rest of the button event handlers here. 
        }


        private bool _isEnabled = false;
        public bool IsEnabled()
        {
            return _isEnabled;
        }



        public void SetEnablement(bool shouldEnable)
        {
            _isEnabled = shouldEnable;

            if(_isEnabled) GetComponent<RectTransform>().pivot = Vector2.zero;
            else GetComponent<RectTransform>().pivot = Vector2.one * 100f;
        }


 
        //Called by the player to add an item to their inventory. 
        public void AddItemToInventory(GameObject inventoryItemGameObject)
        {
            //If adding a material item to inventory
            MaterialItem mi = inventoryItemGameObject.GetComponent<MaterialItem>();
            if (mi != null)
            {
                ItemDefinitions.GetInstance().AddMaterial((SOMaterial)mi.GetItemInfo());
                _UpdateInventoryUI();
                Destroy(inventoryItemGameObject);
                return;
            }

            //If adding an ammo item to inventory
            AmmoItem ai = inventoryItemGameObject.GetComponent<AmmoItem>();
            if (ai != null)
            {
                ItemDefinitions.GetInstance().AddAmmo((SOAmmo)ai.GetItemInfo());
                Destroy(inventoryItemGameObject);
                _UpdateInventoryUI();
                return;
            }

            //If adding a weapon/tool item to inventory
            Item.WeaponOrToolItem we = inventoryItemGameObject.GetComponent<Item.WeaponOrToolItem>();
            if (we != null)
            {
                GameObject newItemSlotUIElement = Instantiate(_weaponOrToolUISlotPrefab);
                newItemSlotUIElement.transform.parent = _uiInventorySlotContent.transform;
                newItemSlotUIElement.GetComponent<RectTransform>().localScale = Vector3.one;
                newItemSlotUIElement.GetComponent<UIWeaponOrToolSlot>().AttachItemGameObject(inventoryItemGameObject);
                inventoryItemGameObject.SetActive(false);
            }
        }



        //Filter inventory slots by the specified component type
        private void _FilterInventoryByCategory(System.Type type)
        {
            foreach (var item in _uiSlots)
            {
                item.SetActive(false);
                if (type == typeof(UIMaterialSlot))
                {
                    if (item.GetComponent<UIMaterialSlot>()) item.SetActive(true);
                }
                else if (type == typeof(UIAmmoSlot))
                {
                    if (item.GetComponent<UIAmmoSlot>()) item.SetActive(true);
                }
                else if (type == typeof(UIWeaponOrToolSlot))
                {
                    if (item.GetComponent<UIWeaponOrToolSlot>()) item.SetActive(true);
                }
                //TODO add the other types here
            }
        }



        private void _UpdateInventoryUI()
        {
            foreach (var i in _uiSlots)
            {
                if (i.activeSelf == false) continue;
                IGroupableItemUISlot slot = i.GetComponent<IGroupableItemUISlot>();
                if (slot != null) slot.UpdateItemQuantityText();
            }
        }



        public void UniquipCurrentItem()
        {
            if (_currentEquipedItemUISlot == null) return;
            _currentEquipedItemUISlot.GetComponent<UIWeaponOrToolSlot>().GetAttachedItemGameObj().GetComponent<Items.AbstractEquipableItem>().UnEquip();
            _currentEquipedItemUISlot = null;
        }



        public void EquipItem(UIWeaponOrToolSlot weaponUISlotReference)
        {
            weaponUISlotReference.GetAttachedItemGameObj().GetComponent<Items.MeleeItemController>().Equip();
            _currentEquipedItemUISlot = weaponUISlotReference;
        }
    } 
}




//namespace Inventory
//{

//    /**************************************************************************************
//     This component exists on the PlayerInventoryPanel UI game object. It is responsible for 
//     managing all the inventory items in the child UI slots. This class is called by the Player
//     to add, remove, or iteract with inventory items. The PlayerInventoryPanel never gets disabled, 
//     but we scale it betweeb 0 and 1 so the player can "toggle" it on and off. 
//     *************************************************************************************/
//    public class InventoryUIPanelManager : MonoBehaviour
//    {

//        ////Reference the scriptable object where all the inventory data/prefabs is defined
//        //[Header("Reference The Groupable Inventory Items Data SO")]
//        //[SerializeField] private SOGroupableInventoryItemsData _groupableInventoryData;

//        //[Header("Reference The Individual Inventory Items Data SO")]
//        //[SerializeField] private SOIndividualInventoryItemsData _individualInventoryData;

//        //[Header("UI slots created to hold each individual item")]
//        //[SerializeField] private GameObject _individualInventoryItemUISlotPrefab;

//        ////References to the filter buttons on the inventory 
//        //[Header("Btn References")]
//        //[SerializeField] private GameObject _btnClothing;
//        //[SerializeField] private GameObject _btnFood;
//        //[SerializeField] private GameObject _btnToolsAndWeapons;
//        //[SerializeField] private GameObject _btnMaterials;
//        //[SerializeField] private GameObject _btnAmmo;

//        ////Reference all the UI slots for Groupable InventoryItems
//        //private List<GroupableInventoryItemSlot> _groupableInventoryItemUISlots;

//        //private bool _isVisible = false;
//        //private Vector2 _hiddenPosition = new Vector2(100, 0);
//        //private RectTransform _rectTransform;

//        ////Tracks the currently selected filter for the inventory. Allows us to avoid redundant sorting and save on CPU resources. 
//        //private PickupableItemCategory _currentItemCategoryFilter; 

//        ////Holds a list of UI elements for each inventory item. Each UI item in turn, contains the coorisponding item game object. 
//        //private List<GameObject> _inventoryItemSlots;


//        //private static InventoryUIPanelManager _instance;


//        //public static InventoryUIPanelManager GetInstance()
//        //{
//        //    return _instance;    
//        //}


//        //protected void Awake()
//        //{
//        //    _instance = this;
//        //    _rectTransform = GetComponent<RectTransform>();
//        //    _inventoryItemSlots = new List<GameObject>();

//        //    _groupableInventoryItemUISlots = GetComponentsInChildren<GroupableInventoryItemSlot>().ToList();

//        //    //register event handlers for the inventory filter buttons. 
//        //    //_btnFilterAll.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByAll);
//        //    //_btnFilterWeapons.GetComponent<Button>().onClick.AddListener(_OnBtnClickFiterByWeapons);
//        //    //_btnFilterTools.GetComponent<Button>().onClick.AddListener(_OnBtnClickFiterByTools);
//        //    //_btnFilterMaterials.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByMaterials);
//        //    //_btnFilterConsumables.GetComponent<Button>().onClick.AddListener(_OnBtnClickFilterByConsumables);       
//        //}


//        //public void UpdateInventoryUI()
//        //{
//        //    foreach (var item in _groupableInventoryItemUISlots)
//        //    {
//        //        var groupableInventoryItem = item.gameObject.GetComponent<GroupableInventoryItemSlot>();
//        //        if (groupableInventoryItem != null)
//        //        {
//        //            groupableInventoryItem.UpdateQuantityUITextElement();
//        //        }
//        //    }
//        //}


//        //public void Show()
//        //{
//        //    _isVisible = true;
//        //    _rectTransform.pivot =  Vector2.zero;
//        //}


//        //public void Hide()
//        //{
//        //    _isVisible = false;
//        //    _rectTransform.pivot = _hiddenPosition;
//        //}


//        //public bool IsVisible()
//        //{
//        //    return _isVisible;
//        //}


//        ////Verifies that the AbstractInventoryItem base component is on this game object. Returns true if it is, returns false otherwise. 
//        //private InventoryItem _IsValidInventoryItem(GameObject inventoryGameObj)
//        //{
//        //    InventoryItem inventoryItemBaseCompoenent = inventoryGameObj.GetComponent<InventoryItem>();
//        //    if (inventoryItemBaseCompoenent == null)
//        //    {
//        //        return inventoryItemBaseCompoenent;
//        //    }
//        //    return null;
//        //}


//        ////Called by the player when they wish to add an item to the inventory. If an inactive slot is available, us that. Otherwise instaciate a new slot. 
//        //public void AddItemToInventory(GameObject inventoryGameObj)
//        //{
//        //    InventoryItem inventoryItemCompoenent = inventoryGameObj.GetComponent<InventoryItem>();
//        //    if (inventoryItemCompoenent == null)
//        //    {
//        //        Debug.LogError("Error adding item to inventory. No AbstractInventoryItem component on this game objecct");
//        //        return;
//        //    }

//        //    //If a groupable item
//        //    IGroupableInventoryItem groupableInventoryItemComponent = inventoryGameObj.GetComponent<IGroupableInventoryItem>();
//        //    if (groupableInventoryItemComponent != null)
//        //    {
//        //        //Update the scriptable object that tracks
//        //        _groupableInventoryData.TryAddInventoryItem(inventoryItemCompoenent);
//        //        return;
//        //    }

//        //    //If an individual item
//        //    IIndividualInventoryItem individualInventoryItem = inventoryGameObj.GetComponent<IIndividualInventoryItem>();
//        //    if (individualInventoryItem != null)
//        //    {
//        //        _individualInventoryData.TryAddInventoryItem(inventoryGameObj);
//        //        Debug.LogError("individual item added!");
//        //        return;
//        //    }

//        //     Debug.LogError("Error adding item to inventory");            

//      //  }


//        //Returns the number of items in the inventory of the specified type
//        public int GetItemCountOfType(string itemName)
//        {
//            int count = 0;
//            foreach (var item in _inventoryItemSlots)
//            {
//                string materialName = item.GetComponent<InventorySlotManager>().GetAttachedGameObj().GetComponent<ItemInteraction>().GetItemName();
//                if (materialName.Trim().ToLower() == itemName.Trim().ToLower())
//                {
//                    count++;
//                }
//            }
//            return count;
//        }


//        //Find the inventory slot with the matching ID and deactivate it. Note, it will still be in the _inventoryItemSlots list, but the game object will be inactive.
//        public void RemoveItemFromInventoryByID(int slotID)
//        {
//            for (int i = 0; i < _inventoryItemSlots.Count; i++)
//            {
//                if (_inventoryItemSlots[i].GetComponent<InventorySlotManager>()._slotID == slotID)
//                {
//                    _inventoryItemSlots[i].gameObject.SetActive(false);
//                    return;
//                }
//            }
//        }


//        //Removes the specified quantity of materials of the given type from the player inventory. Must check that items are present for this to work. Called when player crafts an item or places a building block.
//        public void RemoveItemQuantityByType(string itemName, int quantityToRemove)
//        {
//            int removedCount = 0;
//            foreach (var item in _inventoryItemSlots)
//            {
//                InventorySlotManager _slotManagerComponent = item.GetComponent<InventorySlotManager>();
//                ItemInteraction _itemInteractionComponent = _slotManagerComponent.GetAttachedGameObj().GetComponent<ItemInteraction>();
//                if (_itemInteractionComponent.GetItemName().ToLower().Trim() == itemName.ToLower().Trim())
//                {
//                    RemoveItemFromInventoryByID(_slotManagerComponent._slotID);
//                    removedCount++;
//                }

//                if (removedCount == quantityToRemove)
//                {
//                    return;
//                }
//            }
//            Debug.LogError("Error trying to remove " + quantityToRemove.ToString() + " items of type '" + itemName + "' from inventory");
//        }


//        //Removes an item of the specified type from the inventory and returns the game object associated with it. Returns null if item not present in inventory.
//        public GameObject GetItemFromInventory(string itemName)
//        {          
//            for (int i = 0; i < _inventoryItemSlots.Count; i++)
//            {
//                ItemInteraction inventoryItemGameObj = _inventoryItemSlots[i].GetComponent<InventorySlotManager>().GetAttachedGameObj().GetComponent<ItemInteraction>();
//                if (inventoryItemGameObj.GetItemName() == itemName)
//                {
//                    //Skip empty slots that don't technically hold anything. 
//                    if (_inventoryItemSlots[i].gameObject.activeSelf == false) continue;

//                    _inventoryItemSlots[i].gameObject.SetActive(false);
//                    return inventoryItemGameObj.gameObject;
//                }
//            }    
//            return null;
//        }



//        //Filters the player inventory by the specified item type. This gets called when one of the filter buttons is clicked. 
//        private void _FilterInventoryByItemType(PickupableItemCategory itemCategory)
//        {
//            //Ignore if filter is already selected
//            if (_currentItemCategoryFilter == itemCategory) return;

//            foreach (var slotItem in _inventoryItemSlots)
//            {
//                ItemInteraction itemInteractionComponent = slotItem.GetComponent<InventorySlotManager>().GetAttachedGameObj().GetComponent<ItemInteraction>();
//                if (itemCategory == PickupableItemCategory.ALL || itemInteractionComponent.GetItemCategory() == itemCategory)
//                {
//                    slotItem.SetActive(true);
//                }
//                else 
//                {
//                    slotItem.SetActive(false);
//                }
//            }
//        }


//        #region Btn Event Handlers

//        private void _OnBtnClickFilterByAll()
//        {
//            _FilterInventoryByItemType(PickupableItemCategory.ALL);
//        }


//        private void _OnBtnClickFiterByWeapons()
//        {
//            _FilterInventoryByItemType(PickupableItemCategory.WEAPON);
//        }


//        private void _OnBtnClickFiterByTools()
//        {
//            _FilterInventoryByItemType(PickupableItemCategory.TOOLS);
//        }


//        private void _OnBtnClickFilterByMaterials()
//        {
//            _FilterInventoryByItemType(PickupableItemCategory.MATERIAL);
//        }


//        private void _OnBtnClickFilterByConsumables()
//        {
//            _FilterInventoryByItemType(PickupableItemCategory.CONSUMABLE);
//        }


//        #endregion

//    }


//}



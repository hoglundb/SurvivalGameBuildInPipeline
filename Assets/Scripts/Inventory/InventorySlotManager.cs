using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlotManager : MonoBehaviour
    {
        [SerializeField] private GameObject _btnEquipItem;
        [SerializeField] private GameObject _btnConsumeItem;
        [SerializeField] private GameObject _btnDropItem;
  

        [SerializeField] private GameObject _labelItemName;

        [SerializeField] public int _slotID;

        private GameObject _itemGameObj;
        private ItemInteraction _itemInteractionComponent;

        //Reference the component that manages the collective group of inventory item slots. 
        private InventoryManager _inventoryManagerComponent;

        private void Awake()
        {
            _btnEquipItem.GetComponent<Button>().onClick.AddListener(_OnEquipItemBtnClick);
            _btnConsumeItem.GetComponent<Button>().onClick.AddListener(_OnConsumeItemBtnClick);
            _btnDropItem.GetComponent<Button>().onClick.AddListener(_OnDropItemBtnClick);

            _inventoryManagerComponent = GameObject.Find("PlayerInventoryPanel").GetComponent<InventoryManager>();
        }


        //Called when this slot is created. This slot references the phyisical game object that it now contains. 
        public void InitSlot(GameObject itemInSlot, int slotID)
        {
            _itemGameObj = itemInSlot;
            _slotID = slotID;
            //Reference the ItemInteraction component on the item game object we are about to put in this inventory slot. 
            _itemInteractionComponent = itemInSlot.GetComponent<ItemInteraction>();


            //Populate the slot with the name of the item type
             _labelItemName.GetComponent<Text>().text = _itemInteractionComponent.GetItemDisplayName();

            //Show hide slot buttons depending on the item type
            var itemCategory = _itemInteractionComponent.GetItemCategory();
            if (itemCategory == PickupableItemCategory.CONSUMABLE)
            {
                _btnConsumeItem.GetComponent<Button>().gameObject.SetActive(true);
                _btnEquipItem.GetComponent<Button>().gameObject.SetActive(false);
            }
            else if(itemCategory == PickupableItemCategory.TOOLS || itemCategory == PickupableItemCategory.WEAPON)
            {
                _btnConsumeItem.GetComponent<Button>().gameObject.SetActive(false);
                _btnEquipItem.GetComponent<Button>().gameObject.SetActive(true);
            }

            _itemGameObj.SetActive(false);
        }


        public GameObject GetAttachedGameObj()
        {
            return _itemGameObj;
        }


        private void _OnEquipItemBtnClick()
        { 
        
        }


        private void _OnConsumeItemBtnClick()
        { 
        
        }


        private void _OnDropItemBtnClick()
        {
            //Toss the item onto the ground
            Player.PlayerControllerParent playerRef = Player.PlayerControllerParent.GetInstance();
            _itemInteractionComponent.ActivateItem(playerRef.transform.position + Vector3.up * 1f + playerRef.transform.forward, playerRef.transform.forward);

            //Call the parent inventory manager to remove the item from this UI slot
            _inventoryManagerComponent.RemoveItemFromInventory(_slotID);
        }
    }
}


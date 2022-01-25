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

        private GameObject _itemGameObj;


        private void Awake()
        {
            _btnEquipItem.GetComponent<Button>().onClick.AddListener(_OnEquipItemBtnClick);
            _btnConsumeItem.GetComponent<Button>().onClick.AddListener(_OnConsumeItemBtnClick);
            _btnDropItem.GetComponent<Button>().onClick.AddListener(_OnDropItemBtnClick);
        }


        //Called when this slot is created. This slot references the phyisical game object that it now contains. 
        public void InitSlot(GameObject itemInSlot)
        {
            _itemGameObj = itemInSlot;

            //Reference the ItemInteraction component on the item game object we are about to put in this inventory slot. 
            ItemInteraction ItemInteractionComponent = itemInSlot.GetComponent<ItemInteraction>();


            //Populate the slot with the name of the item type
             _labelItemName.GetComponent<Text>().text = ItemInteractionComponent.GetItemDisplayName();

            //Show hide slot buttons depending on the item type
            var itemCategory = ItemInteractionComponent.GetItemCategory();
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
        
        }
    }
}


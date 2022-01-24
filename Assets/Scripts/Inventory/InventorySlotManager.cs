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


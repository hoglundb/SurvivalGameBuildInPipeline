using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponOrToolSlot : MonoBehaviour
{
    [SerializeField] private GameObject _itemNameTextbox;
    [SerializeField] private GameObject _equipItemBtn;
    [SerializeField] private GameObject _dropItemBtn;

    private GameObject _attachedItem;

    public void AttachItemGameObject(GameObject itemObj)
    {
        _attachedItem = itemObj;

        _itemNameTextbox.GetComponent<Text>().text =  itemObj.GetComponent<Item.WeaponOrToolItem>().GetItemInfo().ItemName;
        InitButtons();
    }


    public GameObject GetAttachedItemGameObj()
    {
        return _attachedItem;
    }


    //Sets up the click event handlers for the buttons on this UI element
    private void InitButtons()
    {
        _equipItemBtn.GetComponent<Button>().onClick.AddListener(_OnEquipBtnClick);
    }


    //Called when user clicks the equip button
    private void _OnEquipBtnClick()
    {  
        //Alert the inventory manager that we are equiping this item so it can be tracked
        Inventory.InventoryUIPanelManager.GetInstance().EquipItem(this);
    }
}

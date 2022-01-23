using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//This component is present on any item that the player can pick up
public class ItemInteraction : MonoBehaviour
{
    [SerializeField] private ItemInteractionScriptableObject _itemInfo;

    private Rigidbody _rb;
    private GameObject _childObj;

    private void Awake()
    {
        _childObj = transform.GetChild(0).gameObject;
        _rb = GetComponent<Rigidbody>();
    }

    public ItemInteractionScriptableObject GetItemInfo()
    {
        return _itemInfo;
    }


    public void DeactivateItem()
    {
        _childObj.SetActive(false);
    }


    public void ActivateItem()
    {
        _childObj.SetActive(true);
    }
}






enum PickupItemCategory
{ 
    NONE,
    WEAPON, 
    CONSUMABLE,
    MATERIAL,
}
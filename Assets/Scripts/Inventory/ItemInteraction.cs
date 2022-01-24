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


    //Returns the category of item as defined in it's scriptable object. The inventory calls this when filtering items by category. 
    public PickupableItemCategory GetItemCategory() 
    {
        return _itemInfo.itemCategory;
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



public enum PickupableItemCategory
{ 
    NONE,
    WEAPON, 
    TOOLS,
    CONSUMABLE,
    MATERIAL,
    ALL,
}
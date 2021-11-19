using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//An inventory slot. This component is present on each UI inventory slot game object. 
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float _maxWeight = 50;
    private Image _uiImage;
    private Text _itemCountText;
    [SerializeField] private Sprite _emptyItemSprite;
   [SerializeField] Button _button;
   [SerializeField] private GameObject _itemImageReference;
   [SerializeField] private GameObject _itemCountUIReference;
   [SerializeField] private List<GameObject> _itemsInSlot;
   [SerializeField] private float _curWeight = 0;

    public GameObject GetImage()
    {
        return _itemImageReference;
    }


    private void Awake()
    {
        //Reference the Imange component on this gameobject and log error if not present. 
        _uiImage = _itemImageReference.GetComponent<Image>();
        if (_uiImage == null)
        {
            Debug.LogError("No UI Image component on this inventory slot game object");
        }

        //Refernce the text component where we display the number of items in this slot
        _itemCountText = _itemCountUIReference.GetComponent<Text>();
        if (_itemCountText == null)
        {
            Debug.LogError("No text component assigned for this inventory slot item count");
        }
        //_button.GetComponentInChildren<Button>();
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        _button.gameObject.transform.localScale = Vector3.one * 1.2f;
    }

     
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _button.gameObject.transform.localScale = Vector3.one;
    }

    public void OnHover()
    { 
    
    }

    //Returns true if this slot has at least one item present. Returns false otherwise.
    public bool HasItems()
    {
        if (_itemsInSlot.Count == 0) return false;
        return true;
    }


    //Returns true if slot contains at least one item of the specified type. Returns false otherwise. 
    public bool HasItemOfThisType(InventoryItemScriptableObj itemObj)
    {
        //Not possible for slot to have an item of this type if it is empty. 
        if (!HasItems())
        {
            return false;
        }
        //Check if the first item in this slot contains an item of this type. Only need to check first one, since only one type is allowed in the list.
        if (_itemsInSlot[0].GetComponent<InventoryItem>().inventoryItemObj.ItemName == itemObj.ItemName)
        {
            return true;
        }

        return false;
    }


    //Returns true if item was successfully added to this slot. Returns false if slot has items of another type or specified item to add would exceed the max weight
    public bool TryAddItem(GameObject itemGameObj)
    {
        //Get the scripable obj component on the game object, and validate that it has this component. 
        InventoryItemScriptableObj obj = itemGameObj.GetComponent<InventoryItem>().inventoryItemObj;
        if (obj == null)
        {
            //Throw error if null and return falure status. 
            Debug.LogError("Inventory item does not have a InventoryItemScriptableObj conmponent");
            return false;
        }

        //Check if item is permitted to be added to this slot.
        if (!_CanAddItemToSlot(obj))
        {
            //Return falure status
            return false;
        }

        //Add the item to this slot.
        _itemsInSlot.Add(itemGameObj);
        _curWeight += obj.ItemWeight;

        //Update the slot UI image. Only really needs done when adding the first item. 
        _uiImage.gameObject.SetActive(true);
        _uiImage.sprite = obj.ItemUISprite;
        _itemCountText.text = _itemsInSlot.Count.ToString();
      
        //Return success status. 
        return true;
    }


    //If an item with this type is in this slot, drop it and updated the UI accordingly
    public bool TryDropItem(InventoryItemScriptableObj itemTypeToDrop)
    {
        //If slot has items of a different type, we can't add pull an item out of it
        if (!HasItemOfThisType(itemTypeToDrop)) return false;

        //Drop the item. 
        _itemsInSlot.RemoveAt(_itemsInSlot.Count - 1);
        _curWeight -= itemTypeToDrop.ItemWeight;
        _itemCountText.text = _itemsInSlot.Count.ToString();

        //If no items left in slot, disable the UI image that shows what type of item is in this slot. 
        if (!HasItems())
        {
            _uiImage.gameObject.SetActive(false);
        }
        return true;
    }


    //Returns true if an item of this type can be added to slot. Slot must have room and not have any other type of items in it. 
    private bool _CanAddItemToSlot(InventoryItemScriptableObj itemTypeToAdd)
    {
        //If slot has items of a different type, we can't add an item of the specified type. 
        if (!HasItemOfThisType(itemTypeToAdd) && HasItems()) return false;

        //If adding item would exceed the max weight for this slot, than we can't add the item. 
        if (_curWeight + itemTypeToAdd.ItemWeight > _maxWeight) return false;

        //Item can be added if conditions above were not met. 
        return true;
    }

}

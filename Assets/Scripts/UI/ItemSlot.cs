using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _initialScale;
    [SerializeField] private float _highlightSize;
    private GameObject _attachedDragDropGameObj; //Reference to DragDrop game object in this slot. Null if slot is empty. The DragDrop object has the actual inventory game object attached to it. 

    private void Awake()
    {
        _initialScale = transform.localScale;
    }


    public GameObject GetCurrentItemInSlot()
    {
        return _attachedDragDropGameObj;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {       
        if (_attachedDragDropGameObj == null && Input.GetKey(KeyCode.Mouse0))
        {
            transform.localScale = Vector3.one * _highlightSize;
        }       
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _initialScale;
    }


    //Returns true if this slot references an item. Returns false if their is no item in this slot. 
    public bool HasItemInSlot() { return (_attachedDragDropGameObj == null) ? false : true; }


    //DragDrop oject calls this when it is no longer being assigned to this iventory item slot.
    public void RemoveItemFromSlot()
    {
        _attachedDragDropGameObj = null;
    }


    //Called by inventory manager to assign a drag drop item to this slot. The drag drop item has a game object and image attached to represent the pysical item in this slot.
    public void AssignDragDropItem(GameObject dragDropItem)
    {
        //Flag this inventory item slot as having an item in it. 
        _attachedDragDropGameObj = dragDropItem;

        //Set the transform of the  object being dropped into the slot to that of the slot itself
        dragDropItem.GetComponent<Transform>().position = transform.position;

        //Tell the object being droped into this slot to reference this slot as it's current assigned slot. 
        dragDropItem.GetComponent<DragDrop>().AssignToItemSlot(gameObject);
    }


    //Removes the item in this slot and completely distroys it. Called when item is used to craft another item. 
    public void DestroyItemInSlot()
    {
        var dd = _attachedDragDropGameObj.GetComponent<DragDrop>();
        dd.Foo();
        _attachedDragDropGameObj.SetActive(false);
        _attachedDragDropGameObj = null;
       // Debug.LogError("destroying item in slot" + gameObject.name);
       //DragDrop dd = _attachedDragDropGameObj.GetComponent<DragDrop>();
       // dd.Foo();
       // if (_attachedDragDropGameObj != null) _attachedDragDropGameObj.gameObject.SetActive(false);
       // _attachedDragDropGameObj = null;
       //_currentItemSlot = null;
       //_previousItemSlot = null;
       //_attachedItemGameObj.SetActive(true);
       //_attachedItemGameObj.GetComponent<InventoryItem>().DropItem();
       //_attachedItemGameObj = null;
       //gameObject.SetActive(false);

        ////Free up the attached drag drop item
        //if (_attachedDragDropGameObj != null)
        //{
        //    _attachedDragDropGameObj.GetComponent<DragDrop>().DestroyAttachedGameObj();
        //    _attachedDragDropGameObj.SetActive(false);
        //    _attachedDragDropGameObj = null;
        //}
    }
}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Vector2 _offset;
    private Vector3 _initialScale;
    private Vector3 _highlightSize;
    private GameObject _currentItemInSlot; //Reference to game object item in this slot. Null if slot is empty. 

    private void Awake()
    {
        _initialScale = transform.localScale;
        _highlightSize = Vector3.one * 1.12f;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {       
        if (_currentItemInSlot == null && Input.GetKey(KeyCode.Mouse0))
        {
            transform.localScale = _highlightSize;
        }       
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _initialScale;
    }


    //Returns true if this slot references an item. Returns false if their is no item in this slot. 
    public bool HasItemInSlot() { return (_currentItemInSlot == null) ? false : true; }


    //DragDrop oject calls this when it is no longer being assigned to this iventory item slot.
    public void RemoveItemFromSlot()
    {
        _currentItemInSlot = null;
    }


    //Called by inventory manager to assign a drag drop item to this slot. The drag drop item has a game object and image attached to represent the pysical item in this slot.
    public void AssignDragDropItem(GameObject dragDropItem)
    {
        //Flag this inventory item slot as having an item in it. 
        _currentItemInSlot = dragDropItem;

        //Set the transform of the  object being dropped into the slot to that of the slot itself
        dragDropItem.GetComponent<Transform>().position = transform.position;

        //Tell the object being droped into this slot to reference this slot as it's current assigned slot. 
        dragDropItem.GetComponent<DragDrop>().AssignToItemSlot(gameObject);
    }
}





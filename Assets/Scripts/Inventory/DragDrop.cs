using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//Represents an inventory item that can be interacted with with the cursor. Can either be attached to the cursor or in an inventory slot. 
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _attachedItemGameObj; //Game object attached to this drag/drop item. 
    [SerializeField] private Canvas _canvas; //reference to the main canvas UI

    private GameObject _currentItemSlot; //Reference the item slot this item is currently assigned to. Reassigned with dropped into a new slot by the player. n
    private GameObject _previousItemSlot; //Track previous item slot so we can snap back if player drags this to an already occupied slot. 
    private readonly float _highlightSize = 1.12f;
    private readonly float _highlightAlpha = .75f;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private float _initialScale;

    private void Awake()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _initialScale = 1f;
    }


    //Tells this DragDrop item to reference the specified inventory item game object. Called when item is added to inventory of player manually moves to another slot. 
    public void AssignToItemSlot(GameObject itemSlot)
    {
       // Debug.LogError("Assigning item " + itemSlot.name + " to dragDrop object");
        _currentItemSlot = itemSlot;    
    }


    //Event is triggered when mouse begins to drag this object. Set to block raycasts so cursor can then interact with the inventory item slots to drop the item into.
    public void OnBeginDrag(PointerEventData eventData)
    {        
        _canvasGroup.blocksRaycasts = false;

        //Tell the inventory itemSlot that it is no longer assigned the this object
        if (_currentItemSlot == null) return;
        ItemSlot assignedItemSlot = _currentItemSlot.GetComponent<ItemSlot>();
        if (assignedItemSlot != null)
        {
            assignedItemSlot.RemoveItemFromSlot();
            _previousItemSlot = _currentItemSlot;
            _currentItemSlot = null;
        }
    }


    //Called when pointer enters this item. Increase the size of the transform to highlight it. 
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * _highlightSize;
    }


    //Called when pointer exits this item. Reset the size of this item to un-highlight it
    public void OnPointerExit(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1;
        transform.localScale = Vector3.one * _initialScale;
    }


    //Called for every frame this item is being dragged by the cursor. Move this transform along with the cursor for the duration of the player dragging it. 
    public void OnDrag(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * _highlightSize;
        _canvasGroup.alpha = _highlightAlpha;

        //move the item with the cursor
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        GameObject hoverdItemSlot = GetHoveredItemSlot(eventData);
        if(hoverdItemSlot != null)
        {
           // hoverdItemSlot.transform.localScale = Vector3.one * _highlightSize;
        }
    }


    //Returns an inventory item slot game object if cursor is hovering over it. 
    private GameObject GetHoveredItemSlot(PointerEventData eventData)
    {
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (var result in raycastResults)
        {
            if (result.gameObject.tag == "UISlot") { return result.gameObject; }
        }
        return null;
    }


    //Called when player stopps dragging this item with the mouse. Unhighlight the item and allow raycasts on the canvas group so it this item can be interacted with again.
    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        //If placing item in new slot
        GameObject hoveredItemSlot = GetHoveredItemSlot(eventData);
        if (hoveredItemSlot != null)
        {
            ItemSlot hovered = hoveredItemSlot.GetComponent<ItemSlot>();

            //If slot already has an item in it, set it back to origonal spot
            if (hovered.HasItemInSlot())
            {
                transform.position = _previousItemSlot.transform.position;
                _currentItemSlot = _previousItemSlot;
                _currentItemSlot.GetComponent<ItemSlot>().AssignDragDropItem(gameObject);
            }
            //Slot we are hovering over is empty, so drop item in that slot. 
            else
            {
                transform.position = hoveredItemSlot.transform.position;
                _currentItemSlot = hoveredItemSlot;
                _currentItemSlot.GetComponent<ItemSlot>().AssignDragDropItem(gameObject);
            }
        }

        //If player is ententionally droping item into game
        else 
        {
            Debug.LogError("dropping item");
        }
      
    }


    //Called when pointer clicks on this item. 
    public void OnPointerDown(PointerEventData eventData) { }


    //Called to attach a game object and image to this item. This typically gets called when the player ls picking up an item. 
    public void AttachGameObject(GameObject gameObjToAttach)
    {
        //check that an InventoryItem component is attached to the gameObj we want to attach to this. 
        InventoryItem inventoryItem = gameObjToAttach.GetComponent<InventoryItem>();
        if (inventoryItem == null)
        {
            Debug.LogError("No InventoryItem component on this game object.");
            return;
        }

        GetComponent<Image>().sprite = inventoryItem.inventoryItemObj.ItemUISprite;
        _attachedItemGameObj = gameObjToAttach;
    }

}

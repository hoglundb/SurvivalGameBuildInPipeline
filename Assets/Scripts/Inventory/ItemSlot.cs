using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Vector2 _offset;
    private Vector3 _initialScale;
    private bool _hasItem = false;


    private void Awake()
    {
        _initialScale = transform.localScale ;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {         
            eventData.pointerDrag.gameObject.transform.position = transform.position;           
            _hasItem = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //if dragging a game object
        if (!_hasItem && Input.GetKey(KeyCode.Mouse0)) 
        {
            transform.localScale = Vector3.one * 1.05f;
            return;
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        //if dragging a game object, this slot is now marked as empty
        if(eventData.pointerDrag != null && eventData.pointerDrag.gameObject != null)
        _hasItem = false;
        transform.localScale = _initialScale;
    }   
}





using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//Manages the UI inventory slot object pools. When an item is added to the inventory, we pull a UI slot drag drop item from the obj pool. 
public class InventorySlotUIController : MonoBehaviour
{
    [SerializeField] private GameObject _dragDropUIPrefab; //Prefab for the drag drop UI element to populate the queue
    private Queue<GameObject> _dragDropItemObjPool;   //Queue of drag drop items to pull from 
    private readonly int _numItems = 50;  //queue size
    private static InventorySlotUIController _instance;  //singleton instance of this class so globally exessable by the player.

    private List<GameObject> _inventorySlots;
    private Vector3 _startPos;

    private void Awake()
    {
        _instance = this;
        _startPos = transform.position;

        //Initialize the drag drop item queue. 
        _dragDropItemObjPool = new Queue<GameObject>();
        for (int i = 0; i < _numItems; i++)
        {
            GameObject uiElement =  GameObject.Instantiate(_dragDropUIPrefab);
            uiElement.transform.parent = transform;
            uiElement.SetActive(false);
            _dragDropItemObjPool.Enqueue(uiElement);
        }

        _inventorySlots = GameObject.FindGameObjectsWithTag("UISlot").ToList();        
    }


    //Sets the screen visibility of this canvas element by moving it way off the screen and back. 
    public void SetVisibility(bool visibility)
    {
        if (visibility)
        {
            transform.position = _startPos;
        }
        else 
        {
            transform.position = Vector3.down * 1000;
        }
    }


    //Singleton for this class so it is globally accessable to the player
    public static InventorySlotUIController GetInstance() { return _instance; }


    public void AddItemToInventory(GameObject itemToPlaceInInventory)
    {
        //Find a unoccupied inventory slot
        foreach (var item in _inventorySlots)
        {
            ItemSlot itemSlotComponent =  item.GetComponent<ItemSlot>();
            if (!itemSlotComponent.HasItemInSlot())
            {
                //Get an object from the dragdrop obj pool to attach game object to and assign to inventory item slot. 
                GameObject dragDropObj = _dragDropItemObjPool.Dequeue();
                dragDropObj.SetActive(true);
                dragDropObj.GetComponent<DragDrop>().AttachGameObject(itemToPlaceInInventory);
                _dragDropItemObjPool.Enqueue(dragDropObj);

                //Tell the item slot it is attached to this dragdrop item. 
                itemSlotComponent.AssignDragDropItem(dragDropObj);

                //Make the item that was just picked up invisable
                itemToPlaceInInventory.SetActive(false);

                return;
            }
        }
    }
}

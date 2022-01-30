using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//Manages the UI inventory slot object pools. When an item is added to the inventory, we pull a UI slot drag drop item from the obj pool. 
namespace UI
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject _dragDropUIPrefab; //Prefab for the drag drop UI element to populate the queue
        private Queue<GameObject> _dragDropItemObjPool;   //Queue of drag drop items to pull from 
        private readonly int _numItems = 50;  //queue size
        private static InventoryController _instance;  //singleton instance of this class so globally exessable by the player.

        private List<GameObject> _inventorySlots;
        private Vector3 _startPos;
        private GameObject _playerReference;

        //Singleton for this class so it is globally accessable to the player
        public static InventoryController GetInstance() { return _instance; }


        private void Awake()
        {
            _instance = this;
            _startPos = transform.position;

            //Initialize the drag drop item queue. 
            _dragDropItemObjPool = new Queue<GameObject>();
            for (int i = 0; i < _numItems; i++)
            {
                GameObject uiElement = GameObject.Instantiate(_dragDropUIPrefab) as GameObject;
                uiElement.transform.parent = transform;
                uiElement.SetActive(false);
                _dragDropItemObjPool.Enqueue(uiElement);
            }

            _inventorySlots = GameObject.FindGameObjectsWithTag("UISlot").ToList();
        }


        //Called by the player so this component can set up a reference to the player game object. 
        public void AssignPlayerReference(GameObject playerReference)
        {
            _playerReference = playerReference;
        }


        //Sets the screen visibility of this canvas element by moving it way off the screen and back. 
        public void SetVisibility(bool visibility)
        {
            if (visibility)
            {
                foreach (var item in _dragDropItemObjPool)
                {
                    item.transform.GetComponent<RectTransform>().localScale = Vector3.one;
                }
            }
            gameObject.SetActive(visibility);
        }


        internal void AddItemToInventory(GameObject itemToPlaceInInventory)
        {
            //Find a unoccupied inventory slot
            foreach (var item in _inventorySlots)
            {
                ItemSlot itemSlotComponent = item.GetComponent<ItemSlot>();
                if (!itemSlotComponent.HasItemInSlot())
                {
                    //Get an object from the dragdrop obj pool to attach game object to and assign to inventory item slot. 
                    GameObject dragDropObj = _dragDropItemObjPool.Dequeue();
                    dragDropObj.SetActive(true);
                    dragDropObj.GetComponent<DragDrop>().AttachGameObject(itemToPlaceInInventory);
                    _dragDropItemObjPool.Enqueue(dragDropObj);

                    //Tell the item slot it is attached to this dragdrop item. 
                    itemSlotComponent.AssignDragDropItem(dragDropObj);

                    //Disable any colliders on the game object we picked up
                    foreach (Collider c in itemToPlaceInInventory.GetComponents<Collider>())
                    {
                        c.enabled = false;
                    }

                    //Make the item that was just picked up invisible since only the sprite remains visible in the UI. 
                    itemToPlaceInInventory.SetActive(false);

                    return;
                }
            }
        }


        //Returns the count of inventory items of the specified type. Call this to determine if enough of an item to craft another item. 
        public int GetItemCount(string itemName)
        {
            int itemCount = 0;
            //Find a unoccupied inventory slot
            foreach (var item in _inventorySlots)
            {
                ItemSlot itemSlotComponent = item.GetComponent<ItemSlot>();
                if (itemSlotComponent.HasItemInSlot())
                {
                    DragDrop d = itemSlotComponent.GetCurrentItemInSlot().GetComponent<DragDrop>();
                    if (d == null)
                    {
                        return 0;
                    }
                    InventoryItem inventoryItem = d.GetAttachedGameObject().GetComponent<InventoryItem>();
                    if (inventoryItem == null)
                    {
                        return 0;
                    }

                    if (inventoryItem.inventoryItemObj.ItemName == itemName)
                    {
                        itemCount++;
                    }
                }
            }
            return itemCount;
        }



        //Removes and destroys the specified quantity of items of the given type. Called when using inventory items to craft another item. 
        public bool UseAndDestroyItems(string itemName, int quantity)
        {
            Debug.LogError("going to destroy " + quantity.ToString() + " of " + itemName);
            int quantityDropped = 0;
            foreach (var item in _inventorySlots)
            {
                ItemSlot itemSlotComponent = item.GetComponent<ItemSlot>();
                if (itemSlotComponent.HasItemInSlot())
                {
                    DragDrop dd = itemSlotComponent.GetCurrentItemInSlot().GetComponent<DragDrop>();
                    if (dd)
                    {
                        GameObject attached = dd.GetAttachedGameObject();
                        if (attached)
                        {
                            InventoryItem ii = attached.GetComponent<InventoryItem>();
                            if (ii)
                            {
                                if (ii.inventoryItemObj.ItemName == itemName)
                                {
                                    Debug.LogError("removing a " + itemName + " from " + itemSlotComponent.name);
                                    itemSlotComponent.DestroyItemInSlot();
                                    quantityDropped++;
                                }
                            }
                        }
                    }
                }

                //We are done once we removet the specified quantity of items of this type. 
                if (quantity == quantityDropped)
                {
                    return true;
                }
            }

            return false;
        }


        //Callback for when player updates an item in their equipable enventory. Tells the player to perform the needed updates to refresh weapon animation
        //public void OnPlayerUpdateEquipedItem()
        //{
        //    _playerReference.GetComponent<Player.PlayerWeaponController>().OnPlayerUpdateEquipedItem();
        //}
    }
}

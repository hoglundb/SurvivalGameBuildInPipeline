using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to manage the player inventory. Tracks the physical inventory and updates the inventory UI as needed. 
public class PlayerInventoryManager : MonoBehaviour
{
    [Header("UI elements for inventory slots (populated on Awake)")]
     private List<PlayerInventorySlot> _playerInventorySlots;
    [SerializeField] private GameObject _uiPrompt;
     private GameObject _inventoryUIPanel;
    public LayerMask mask;

    private void Awake()
    {
        _InitPlayerInventory();
        _uiPrompt.SetActive(false);
        _inventoryUIPanel = GameObject.FindGameObjectWithTag("InventoryItemsTab");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _inventoryUIPanel.SetActive(!_inventoryUIPanel.activeSelf);
        }

        _ScanEnvironmentForItems();
    }



    //Called each frame to scan the invironment around the player. Prompt the player if a pickupable item is in range
    private void _ScanEnvironmentForItems()
    {  
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
        {
            if (hit.transform.gameObject.tag == "PickupableItem")
            {
                _uiPrompt.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    TryPlaceItemInInventory(hit.transform.gameObject);
                }
                return;
            }                      
        }
        _uiPrompt.SetActive(false);
    }


    //Called on Awake to intialize the player inventory and set up any game object references. 
    private void _InitPlayerInventory()
    {
        GameObject[] uiSlotGameObjects = GameObject.FindGameObjectsWithTag("UISlot");
        _playerInventorySlots = new List<PlayerInventorySlot>();
        foreach (GameObject uiSlot in uiSlotGameObjects)
        {
            _playerInventorySlots.Add(new PlayerInventorySlot
            {
                slotGameObj = uiSlot,
                inventorySlotComponent = uiSlot.GetComponent<InventorySlot>(),
            });
        }
    }


    //Attempts to put the specified item into the inventory. Returns false if inventory is full. 
    public bool TryPlaceItemInInventory(GameObject itemGameObj)
    {
        //reference the scriptable object on the inventoryitem game object
        InventoryItemScriptableObj itemScriptableObj = itemGameObj.GetComponent<InventoryItem>().inventoryItemObj;
        //Check for a partially empty slot that this item can fit into
        foreach (var s in _playerInventorySlots)
        {
            if (s.inventorySlotComponent.HasItemOfThisType(itemScriptableObj))
            {
                if (s.inventorySlotComponent.TryAddItem(itemGameObj))
                {
                    return true;
                }
            }
        }
        //add item to first availble inventory slot
        foreach (var s in _playerInventorySlots)
        {
            if (s.inventorySlotComponent.TryAddItem(itemGameObj))
            {
                return true;
            }
        }
        Debug.LogWarning("Cannot place item in inventory. Inventory may be full");
        return false;
    }
}


//Serves as a reference to UI slot game objects so we can cache their InventorySlot component. 
public class PlayerInventorySlot
{
    public GameObject slotGameObj;
    public InventorySlot inventorySlotComponent;
}
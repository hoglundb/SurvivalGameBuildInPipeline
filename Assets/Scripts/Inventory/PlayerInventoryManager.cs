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
        //_InitPlayerInventory();
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
                    PlaceItemInInventory(hit.transform.gameObject);
                }
                return;
            }                      
        }
        _uiPrompt.SetActive(false);
    }


    private void PlaceItemInInventory(GameObject gameObj)
    {
       // Debug.LogError("Placing item in inventory");
        InventorySlotUIController.GetInstance().AddItemToInventory(gameObj);
    }
}


//Serves as a reference to UI slot game objects so we can cache their InventorySlot component. 
public class PlayerInventorySlot
{
    public GameObject slotGameObj;
    public InventorySlot inventorySlotComponent;
}
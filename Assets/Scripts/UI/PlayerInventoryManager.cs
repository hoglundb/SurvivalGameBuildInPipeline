using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to manage the player inventory. Tracks the physical inventory and updates the inventory UI as needed. 
public class PlayerInventoryManager : MonoBehaviour
{
    private Inventory.InventoryManager _inventoryManager;

    [SerializeField] private GameObject _baseBuildingUIPanel;

    public LayerMask mask;

    public PlayerMovement _playerMovement;  //Reference the player movement script on this game object.

    public bool _isInventoryInUse;

    private CanvasController _canvasController;


    private void Awake()
    {
        _inventoryManager = GameObject.Find("PlayerInventoryPanel").GetComponent<Inventory.InventoryManager>();
        _baseBuildingUIPanel.SetActive(false);
        
        //_isInventoryInUse = false;
        //_uiPrompt.SetActive(false);
        //_playerMovement = GetComponent<PlayerMovement>();
        //Cursor.visible = false;
    }


    private void Start()
    {
        _canvasController = CanvasController.GetInstance();        
    }


    //// Update is called once per frame
    void Update()
    {
        //ToggleInventory visability and interactivity
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _baseBuildingUIPanel.SetActive(false);
            _inventoryManager.ToggleVisibility();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            _baseBuildingUIPanel.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }


    ////Called every frame to respond to player input to toggle the visibility of the inventory IU. 
    //private void ToggleInventory()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        _isInventoryInUse = !_isInventoryInUse;
    //        if (_isInventoryInUse)
    //        {            
    //            Cursor.visible = true;
    //        }
    //        else
    //        {
    //            Cursor.visible = false;
    //        }
    //        _canvasController.ToggleInventoryCraftingUI(_isInventoryInUse);
    //        _playerMovement.SetMovementEnablement(!_isInventoryInUse);
    //    }
    //}


    ////Called each frame to scan the invironment around the player. Prompt the player if a pickupable item is in range
    //private void _ScanEnvironmentForItems()
    //{  
    //    //RaycastHit hit;
    //    //if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
    //    //{
    //    //    if (hit.transform.gameObject.tag == "PickupableItem")
    //    //    {
    //    //        _uiPrompt.SetActive(true);
    //    //        if (Input.GetKeyDown(KeyCode.E))
    //    //        {
    //    //            PlaceItemInInventory(hit.transform.gameObject);
    //    //        }
    //    //        return;
    //    //    }                      
    //    //}
    //    //_uiPrompt.SetActive(false);
    //}


    //private void PlaceItemInInventory(GameObject gameObj)
    //{
    //    _canvasController.AddItemToInventory(gameObj);;
    //}


}


using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to manage the player inventory. Tracks the physical inventory and updates the inventory UI as needed. 
public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _uiPrompt;

    public LayerMask mask;

    public PlayerMovement _playerMovement;  //Reference the player movement script on this game object.

    public bool _isInventoryInUse;

    private CanvasController _canvasController;

    private void Awake()
    {
        _isInventoryInUse = false;
        _uiPrompt.SetActive(false);
        _playerMovement = GetComponent<PlayerMovement>();
        Cursor.visible = false;
    }


    private void Start()
    {
        _canvasController = CanvasController.GetInstance();        
    }


    // Update is called once per frame
    void Update()
    {
        //ToggleInventory visability and interactivity
        ToggleInventory();

        //Player scans the envirnoment using raycasts
        _ScanEnvironmentForItems();
    }


    //Called every frame to respond to player input to toggle the visibility of the inventory IU. 
    private void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _isInventoryInUse = !_isInventoryInUse;
            if (_isInventoryInUse)
            {            
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
            _canvasController.ToggleInventoryCraftingUI(_isInventoryInUse);
            _playerMovement.SetMovementEnablement(!_isInventoryInUse);
        }
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
        InventorySlotUIController.GetInstance().AddItemToInventory(gameObj);
    }
}


using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to manage the player inventory. Tracks the physical inventory and updates the inventory UI as needed. 
public class PlayerInventoryManager : MonoBehaviour
{
    private Inventory.InventoryManager _inventoryManager;
    private Inventory.CraftingManager _craftingManager;

    [SerializeField] private GameObject _baseBuildingUIPanel;

    public LayerMask mask;

    public PlayerMovement _playerMovement;  //Reference the player movement script on this game object.

    public bool _isInventoryInUse;

    private CanvasController _canvasController;

    private PlayerControllerParent _playerControllerParentComponent;

    private void Awake()
    {
        _inventoryManager = GameObject.Find("PlayerInventoryPanel").GetComponent<Inventory.InventoryManager>();
        _craftingManager = GameObject.Find("PlayerCraftingPanel").GetComponent<Inventory.CraftingManager>();
        _baseBuildingUIPanel.SetActive(false);
        _playerControllerParentComponent = GetComponent<PlayerControllerParent>();
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
            bool isNowShowing = _inventoryManager.ToggleVisibility();
            _playerControllerParentComponent.SetMovementEnablement(!isNowShowing);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            _baseBuildingUIPanel.GetComponent<RectTransform>().localScale = Vector3.one;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _baseBuildingUIPanel.SetActive(false);
            bool isShowing = _craftingManager.ToggleVisibility();
            _playerControllerParentComponent.SetMovementEnablement(!isShowing);
        }
    }
}


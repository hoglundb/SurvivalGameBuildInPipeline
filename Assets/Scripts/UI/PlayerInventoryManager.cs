using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to manage the player inventory. Tracks the physical inventory and updates the inventory UI as needed. 
public class PlayerInventoryManager : MonoBehaviour
{
    private InventoryUI.InventoryManager _inventoryManager;
    private InventoryUI.CraftingManager _craftingManager;

    [SerializeField] private GameObject _baseBuildingUIPanel;

    public LayerMask mask;

    public PlayerMovement _playerMovement;  //Reference the player movement script on this game object.

    public bool _isInventoryInUse;

    private InventoryUI.CanvasController _canvasController;

    private PlayerControllerParent _playerControllerParentComponent;

    private void Awake()
    {
        _inventoryManager = GameObject.Find("PlayerInventoryPanel").GetComponent<InventoryUI.InventoryManager>();
        _craftingManager = GameObject.Find("PlayerCraftingPanel").GetComponent<InventoryUI.CraftingManager>();
        _baseBuildingUIPanel.SetActive(false);
        _playerControllerParentComponent = GetComponent<PlayerControllerParent>();
    }


    private void Start()
    {
        _canvasController = InventoryUI.CanvasController.GetInstance();        
    }


    //// Update is called once per frame
    void Update()
    {
       
    }
}


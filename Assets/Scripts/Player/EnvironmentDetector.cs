using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Player
{
    public class EnvironmentDetector : MonoBehaviour
    {
        private GameObject _uiPromptToPickItemUp;
        private InventoryUI.InventoryManager _inventoryManagerComponent;

        private void Awake()
        {
            _uiPromptToPickItemUp = GameObject.Find("PickupItemPrompt");
            _uiPromptToPickItemUp.SetActive(false);
            var foo = GameObject.Find("PlayerInventoryPanel");
            _inventoryManagerComponent = GameObject.Find("PlayerInventoryPanel").GetComponent<InventoryUI.InventoryManager>();
        }


        private void Update()
        {
            _ScanInvironment();
        }


        private void _ScanInvironment()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
            {
                if (hit.transform.gameObject.tag == "PickupableItem")
                {
                    _uiPromptToPickItemUp.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _inventoryManagerComponent.AddItemToInventory(hit.transform.gameObject);
                    }
                    return;
                }                      
            }
            _uiPromptToPickItemUp.SetActive(false);
        }
    }
}


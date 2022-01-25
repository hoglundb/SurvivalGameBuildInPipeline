using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private GameObject _craftingItemUIPrefab;
        [SerializeField] private GameObject _itemContentUIPanel;
        [SerializeField] private GameObject _itemIngredientsTextbox;
        [SerializeField] private float _heightPerItem = 2f;

        //Reference the inventory panel so we can disable it if the crafting panel is activated
        private InventoryManager _inventoryManagerComponent;

        private bool _isShowing;


        private void Awake()
        {
            _inventoryManagerComponent = GameObject.Find("PlayerInventoryPanel").GetComponent<InventoryManager>();
            SetVisibility(false);
        }


        public void UpdateItemIngredientsTextbox(string ingredientsList)
        {
            _itemIngredientsTextbox.GetComponent<Text>().text = ingredientsList;
        }


        //Toggles the visibility of the crafting UI by scaling the panel between 0 and 1. 
        public bool ToggleVisibility()
        {
            _isShowing = !_isShowing;
            return _UpdateVisibility();
        }


        public bool SetVisibility(bool makeVisible)
        {
            _isShowing = makeVisible;
            return _UpdateVisibility();
        }


        private bool _UpdateVisibility()
        {
            if (_isShowing)
            {
                //toggle off the inventory panel
                _inventoryManagerComponent.SetVisibility(false);

                //make this panel visible by setting it's scale back to 1
                GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            else
            {
                //make this panel invisible by setting it's scale to 0
                GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
            }
            return _isShowing;
        }
    }

}

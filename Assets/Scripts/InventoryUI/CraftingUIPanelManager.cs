//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Inventory
//{
//    /**********************************************************************************************
//     This component lives on the PlayerCraftingPanel game object. Within this panel, the craftable
//     items are contained in a scroll view. Note that the PlayerCraftingPanel game object is never disabled. 
//     Toggling this panel is done by scaling the panel between 0 and 1. 
//     **********************************************************************************************/
//    public class CraftingUIPanelManager : UIPanelManagerBase
//    {
//        //UI references
//        [SerializeField] private GameObject _craftingItemUIPrefab;
//        [SerializeField] private GameObject _itemContentUIPanel;
//        [SerializeField] private GameObject _itemIngredientsTextbox;

//        //Amount to increase/decrease the scroll height of the panel when an item is added/removed
//        [SerializeField] private float _heightPerItem = 2f;

//        //Reference the inventory panel so we can disable it if the crafting panel is activated
//        private InventoryUIPanelManager _inventoryManagerComponent;

//        //List of all child items in the crafting UI. We gather these on Awake. 
//        private List<CraftableItemController> _craftableItemsList;


//        protected override void Awake()
//        {
//            base.Awake();

//            //Reference the PlayerInventoryPanel UI since these game objects need to comminicate back and forth. 
//            _inventoryManagerComponent = GameObject.Find("PlayerInventoryPanel").GetComponent<InventoryUIPanelManager>();

//            //Populate the _craftableItemsList based on the UI elements defined in the scroll view UI.
//            _craftableItemsList = new List<CraftableItemController>();
//        }


//        //Populates the ingredients list textbox with the specified string. This is called when player selects an craftable item to view details. 
//        public void UpdateItemIngredientsTextbox(string ingredientsList)
//        {
//            _itemIngredientsTextbox.GetComponent<Text>().text = ingredientsList;
//        }
     

//        //Updates each craftable item in the list based on if the player has the ingredients to craft that item. Items that are uncraftable have their "craft" btn disabled. 
//        public void UpdateCraftabilityForItems()
//        {
//            foreach (var item in _craftableItemsList)
//            {
//                item.SetCraftability(_HasIngredientsToCraftItem(item));
//            }
//        }


//        //Looks at the inventory and returns true if the materials in the ingredients list are found in the inventory. Returns false otherwise. 
//        private bool _HasIngredientsToCraftItem(CraftableItemController _itemCraftingComponent)
//        {
//            foreach (var ingredient in _itemCraftingComponent.ingredients)
//            {
//                int numIngredientsOfThisTypeInInventory = _inventoryManagerComponent.GetItemCountOfType(ingredient.ingredient);
//                if (numIngredientsOfThisTypeInInventory < ingredient.quantity)
//                {
//                    //Debug.LogError("only have " + numIngredientsOfThisTypeInInventory.ToString() + " of " + ingredient.ingredient + ". Need " + ingredient.quantity.ToString());
//                    return false;
//                }
//            }
//            return true;
//        }


//        //Updates the visibility of this panel based on the value of _isShowing.
//        //private bool _UpdateVisibility()
//        //{
//        //    if (_isShowing)
//        //    {
//        //        //toggle off the inventory panel
//        //        _inventoryManagerComponent.SetVisibility(false);

//        //        //make this panel visible by setting it's scale back to 1
//        //        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
//        //    }
//        //    else
//        //    {
//        //        //make this panel invisible by setting it's scale to 0
//        //        GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
//        //    }
//        //    return _isShowing;
//        //}
//    }

//}

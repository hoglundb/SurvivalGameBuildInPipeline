using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseBuilding
{
    public class BaseBuildingUISlot : MonoBehaviour
    {
        [Header("The scriptable obj representing this piece")]
        [SerializeField] private SOBaseBuildingPiece _soBuildingPiece;

        [Header("The 'Create' btn reference")]
        [SerializeField] private GameObject _createBtnGameObj;
        private Button _createBtn;

        private void Awake()
        {
            _createBtn = _createBtnGameObj.GetComponent<Button>();
            _createBtn.onClick.AddListener(_OnCreateBtnClick);

        }


        private void _OnCreateBtnClick()
        {
            //Pass the scriptable obj contining the item data to the player and request that the player allow the player to place it. 
            Player.PlayerControllerParent.GetInstance().playerBaseBuildingComponent.OnPlayerSelectItemToPlace(_soBuildingPiece);
        }


        //Enables or disables this item slot UI based on if there is the required material in the player's inventory. 
        public void UpdateBasedOnMaterialAvailability()
        {
            if (_HasEnoughMaterialInInventoryToCreateItem())
            {
                _createBtn.interactable = true;
                Debug.LogError("enabling");
            }
            else 
            {
                _createBtn.interactable = false;
                Debug.LogError("disabling");
            }
        }


        private bool _HasEnoughMaterialInInventoryToCreateItem()
        {
            ItemDefinitions inventory = ItemDefinitions.GetInstance();
            foreach (SOMaterial matIngredient in _soBuildingPiece.ingredientsList)
            {
                //  Debug.LogError(inventory.GetItemQuantity(matIngredient));
                Debug.LogError(inventory.GetItemQuantity(matIngredient));
                Debug.LogError(_soBuildingPiece.materialQuantity);
                Debug.LogError("\n\n");
                if (inventory.GetItemQuantity(matIngredient) < _soBuildingPiece.materialQuantity)
                {
                    return false;
                }
            }
            return true;
        }
    }
}


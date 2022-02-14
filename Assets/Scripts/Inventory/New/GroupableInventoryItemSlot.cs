using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This component is on each GroupableInventoryItemSlot in the player inventory for groupable items. 
 */
namespace Inventory
{
    public class GroupableInventoryItemSlot : MonoBehaviour
    {
        [Header("Reference the SO defining this item's type's data")]
        [SerializeField] private SOGroupableInventoryItem _itemSOReference;

        [Header("Child UI Element References")]
        [SerializeField] private GameObject _quantityTextElement;
        private Text _quantityTextBox;

        private void Awake()
        {
            _quantityTextBox = _quantityTextElement.GetComponent<Text>();
        }


        public void UpdateQuantityUITextElement()
        {
            _quantityTextBox.text = _itemSOReference.quantity.ToString();
        }
    }
}


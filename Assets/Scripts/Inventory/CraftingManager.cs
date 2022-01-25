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


        public void UpdateItemIngredientsTextbox(string ingredientsList)
        {
            _itemIngredientsTextbox.GetComponent<Text>().text = ingredientsList;
        }

        //TODO
        public void ToggleVisibility()
        {

        }
    }

}

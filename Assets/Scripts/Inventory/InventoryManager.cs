using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryUISlotPrefab;
        [SerializeField] private GameObject _inventorySlotUIGrid;
        [SerializeField] private GameObject _scrollbarGameObj;
        [SerializeField] private float _itemSlotHeight;
        private bool _isShowing = true;
        private float _numItems = 0f;
        private RectTransform _rectTransform;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }


        public void AddItemToInventory(GameObject inventoryGameObj)
        {
            _numItems++;
           var newItem = GameObject.Instantiate(_inventoryUISlotPrefab);
            newItem.transform.parent = _inventorySlotUIGrid.transform;
    
            newItem.GetComponent<RectTransform>().localScale = Vector3.one;
            _SetRectHeightBasedOnItemCount();
        }


        private void _SetRectHeightBasedOnItemCount()
        {
            RectTransform rt = _inventorySlotUIGrid.GetComponent<RectTransform>();
            //Debug.LogError(rt.gameObject.name);
            ////rt.sizeDelta = new Vector2(0, 0);
            //float curHeight = rt.rect.height;
            //float curWidth =rt.rect.width;
            //float x = rt.rect.position.x;
            //float y = rt.rect.position.y;
            //Debug.LogError(curHeight.ToString());
            //rt.rect.Set(x, y, curHeight, curWidth + _itemSlotHeight);

            Vector2 curSizeDelta = rt.sizeDelta;
            curSizeDelta.y += _itemSlotHeight;
            rt.sizeDelta = curSizeDelta;
        }

        public void TryRemoveItemFromInventory()
        {

        }


        public void ToggleVisibility()
        {
            _isShowing = !_isShowing;

            if (_isShowing)
            {
                _rectTransform.localScale = Vector3.one;
                _ResetChildTransforms();
            }
            else 
            {
                Vector3 hiddenScale = _rectTransform.localScale;
                hiddenScale.x = 0f;
                _rectTransform.localScale = hiddenScale;
            }
        }


        private void _ResetChildTransforms()
        {
            foreach (Transform child in _inventorySlotUIGrid.transform)
            {
                child.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }

    }
  

}



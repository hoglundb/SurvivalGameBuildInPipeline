using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//All the functions a UI controller component must perform (e.g. the inventory UI, building UI, crafting UI, etc.)
namespace InventoryUI
{
    public abstract class UIControllerBase : MonoBehaviour
    {
        protected RectTransform _rectTransform;
        protected Vector3 _uiVisibleScale;
        protected Vector3 _uiInvisibleScale;

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _uiInvisibleScale = new Vector3(0, 1, 1);
            _uiVisibleScale = Vector3.one;
        }


        //Toggles the visiblity of the Inventory UI. If visible, scale to 0 to make invisible. If 1, the scale to 0. Return true if updated to be visible and return false o.w.
        public void ToggleVisibility()
        {
            if (_rectTransform.localScale == Vector3.one) { _rectTransform.localScale = new Vector3(0, 1, 1); }
            else { _rectTransform.localScale = Vector3.one; }
        }


        //Returns true if the panel is visible (i.e. the rect transform is scaled to 1). Returns false otherwise. 
        public bool IsVisible()
        {
            if (_rectTransform.localScale == Vector3.one) { return true; }
            return false;
        }


        //Sets the the scale of the rect transform to set it's visibility based on the provided parameter. 
        public void SetVisibility(bool makeVisible)
        {
            if (makeVisible) { _rectTransform.localScale = Vector3.one; }
            else { _rectTransform.localScale = _rectTransform.localScale = new Vector3(0, 1, 1); }
        }
    }
}
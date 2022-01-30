using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//All the functions a UI controller component must perform (e.g. the inventory UI, building UI, crafting UI, etc.)
namespace UI
{
    public interface IUIController
    {
        void ToggleVisibility();
        void IsVisible();
        void SetVisibility();
    }
}
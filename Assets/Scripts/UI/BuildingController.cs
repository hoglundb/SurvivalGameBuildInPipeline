using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************************************************
 This component lives on the PlayerBuildingPanel UI. It manages the actions the player takes
 to build items from matarials in the inventory
 ********************************************************************************************/
namespace UI
{
    public class BuildingController : MonoBehaviour, IUIController
    {
        //The rect transform that we scale between 0 and 1 to show/hide the UI panel. 
        private RectTransform _rectTransform;

        //Singleton pattern for this class since there will only ever be one game object with a BuidlingManager component. 
        private static BuildingController _instance;
        public static BuildingController GetInstance()
        {
            return _instance;
        }


        private void Awake()
        {
            _instance = this;
            _rectTransform = GetComponent<RectTransform>();
        }


        //TODO
        public void ToggleVisibility()
        {

        }


        //TODO
        public void SetVisibility()
        {

        }


        public void IsVisible()
        { 
        
        }

    }
}

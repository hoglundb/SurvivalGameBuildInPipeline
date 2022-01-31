using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************************************************
 This component lives on the PlayerBuildingPanel UI. It manages the actions the player takes
 to build items from matarials in the inventory
 ********************************************************************************************/
namespace Inventory
{
    public class BuildingUIPanelUIManager : UIPanelManagerBase
    {

        //Singleton pattern for this class since there will only ever be one game object with a BuidlingManager component. 
        private static BuildingUIPanelUIManager _instance;
        public static BuildingUIPanelUIManager GetInstance()
        {
            return _instance;
        }


        protected override void Awake()
        {
            base.Awake();

            _instance = this;
        }

    }
}

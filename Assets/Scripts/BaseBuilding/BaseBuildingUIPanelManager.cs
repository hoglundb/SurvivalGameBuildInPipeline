using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BaseBuilding
{
    public class BaseBuildingUIPanelManager : MonoBehaviour
    {
        private static BaseBuildingUIPanelManager _instance;
        public static BaseBuildingUIPanelManager GetInstance()
        {
            return _instance;
        }


        private void Awake()
        {
            _instance = this;
        }

        private bool _isEnabled = false;


        public bool IsEnabled()
        {
            return _isEnabled;
        }

        public void SetEnablement(bool shouldEnable)
        {
            _isEnabled = shouldEnable;
            if (_isEnabled) GetComponent<RectTransform>().pivot = Vector2.zero;
            else GetComponent<RectTransform>().pivot = Vector2.one * 100f;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BaseBuilding
{
    public class BaseBuildingUIPanelManager : MonoBehaviour
    {
        [Header("Parent of all the UI item slots")]
        [SerializeField] private GameObject _scrollViewContent;

        //Reference all the UI item slots
        private List<BaseBuildingUISlot> _uiSlots;

        private static BaseBuildingUIPanelManager _instance;
        public static BaseBuildingUIPanelManager GetInstance()
        {
            return _instance;
        }


        private void Awake()
        {
            _instance = this;

            //Reference all the child UI slots for this base building panel 
            _uiSlots = new List<BaseBuildingUISlot>();
            foreach (Transform tran in _scrollViewContent.transform)
            {
                BaseBuildingUISlot uiSlotComponent = tran.gameObject.GetComponent<BaseBuildingUISlot>();
                if(uiSlotComponent != null) _uiSlots.Add(tran.gameObject.GetComponent<BaseBuildingUISlot>());
            }
        }

        private bool _isEnabled = false;


        //Returns true if the base building UI panel is enabled/visible and returns false otherwise.
        public bool IsEnabled()
        {
            return _isEnabled;
        }



        //Enables/disables the UI panel by setting it's pivot component making it visible/invisible
        public void SetEnablement(bool shouldEnable)
        {           
            _isEnabled = shouldEnable;
            if (IsEnabled()) UpdateItemSlots(); //Update the item slots since inventory may have changed since we last opened this.
            if (_isEnabled) GetComponent<RectTransform>().pivot = Vector2.zero;
            else GetComponent<RectTransform>().pivot = Vector2.one * 100f;
        }


        //Tells each UI slot to update to reflect inventory material availbility. 
        public void UpdateItemSlots()
        {
            foreach (BaseBuildingUISlot b in _uiSlots)
            {
                b.UpdateBasedOnMaterialAvailability();
            }
        }
    }
}


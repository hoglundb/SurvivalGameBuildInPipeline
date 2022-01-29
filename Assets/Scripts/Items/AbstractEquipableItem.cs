using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Items
{
    /****************************************************************************************************
     Parent component for a game object that the player is able to equip.
     **************************************************************************************************/
    public class AbstractEquipableItem : MonoBehaviour
    {
        [Header("scripable object info for this equipable item")]
        [SerializeField] private EquipableItemInfoScriptableObject _equipableItemInfo;

        //Reference the player so we can tell it what animations to play and parent this item to the player hand. 
        private Player.PlayerControllerParent _playerParentControllerComponent;

        private bool _isEquiped = false;

        private void Start()
        {
            _playerParentControllerComponent = Player.PlayerControllerParent.GetInstance();
        }


        public void Equip()
        {
            _isEquiped = true;
            gameObject.SetActive(true);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            if (_equipableItemInfo._rightHandControlled)
            {
                transform.parent = _playerParentControllerComponent.GetRightHandBone();
            }
            else 
            {
                transform.parent = _playerParentControllerComponent.GetLeftHandBone();
            }

            _playerParentControllerComponent.SetAnimationTrigger("EquipBow");
        }


        public void UnEquip()
        {
            _isEquiped = false;
            gameObject.SetActive(false);
            transform.parent = null;
        }


        private void Update()
        {
            if (!_isEquiped) return;

            transform.localPosition = _equipableItemInfo._dominantHandPosOffset * .001f;
            transform.localRotation = Quaternion.Euler(_equipableItemInfo._dominantHandRotOffset * .5f);
        }
    }
}


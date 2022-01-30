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

        [SerializeField] private Vector3 _arrowLocalRotation;

        //Reference the player so we can tell it what animations to play and parent this item to the player hand. 
        private Player.PlayerControllerParent _playerParentControllerComponent;

        private bool _isEquiped = false;
        private GameObject _equipedArrow;

        

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

            //put the arrow on the bow, if there is one in the player's inventory. 
            _equipedArrow = Inventory.InventoryManager.GetInstance().GetItemFromInventory("BASIC_ARROW");
            if (_equipedArrow == null)
            {
                Debug.LogError("No arrows in inventory");
                return;
            }

            _equipedArrow.transform.parent = transform;
            transform.localPosition = Vector3.zero;
            _equipedArrow.GetComponent<Rigidbody>().isKinematic = true;
            _equipedArrow.SetActive(true);
            transform.localPosition = Vector3.zero;
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

            transform.localPosition = Vector3.zero;
          //  transform.position = Player.PlayerControllerParent.GetInstance().GetLeftHandBone().transform.position;
            transform.localPosition = _equipableItemInfo._dominantHandPosOffset * .00025f;
          //  transform.rotation = Player.PlayerControllerParent.GetInstance().GetLeftHandBone().transform.rotation;
            transform.localRotation = Quaternion.Euler(_equipableItemInfo._dominantHandRotOffset * .5f);

            if (_equipedArrow != null)
            {
                _equipedArrow.transform.localPosition = _equipableItemInfo._projectilePosOffset * .005f;
                _equipedArrow.transform.localRotation = Quaternion.Euler(_equipableItemInfo._projectileRotOffset);
            }
        }

        private void LateUpdate()
        {
            if (!_isEquiped) return;
           // transform.position = Player.PlayerControllerParent.GetInstance().GetLeftHandBone().transform.position;
            //  transform.localPosition = _equipableItemInfo._dominantHandPosOffset * .0005f;
           // transform.rotation = Player.PlayerControllerParent.GetInstance().GetLeftHandBone().transform.rotation;
        }

        
    }
}


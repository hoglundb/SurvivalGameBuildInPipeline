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

        private float _drawAmount = 0f;

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
            Reload();
            Player.PlayerControllerParent.GetInstance().SetAnimationFloat("BowDrawAmount", 0f);
        }


        public void Reload()
        {
            //put the arrow on the bow, if there is one in the player's inventory. 
            _equipedArrow = Inventory.InventoryUIPanelManager.GetInstance().GetItemFromInventory("BASIC_ARROW");
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
            _ManageDrawAmount();
            transform.localPosition = Vector3.zero;
            transform.localPosition = _equipableItemInfo._dominantHandPosOffset * .00025f;
            transform.localRotation = Quaternion.Euler(_equipableItemInfo._dominantHandRotOffset * .5f);

            if (_equipedArrow != null)
            {
                _equipedArrow.transform.localPosition = _equipableItemInfo._projectilePosOffset * .005f;
                _equipedArrow.transform.localRotation = Quaternion.Euler(_equipableItemInfo._projectileRotOffset);
            }

           
        }



        private void _ManageDrawAmount()
        {
            //increment the draw amount
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _drawAmount += 3f * Time.deltaTime;
            }
            else
            {
                _drawAmount -= 5f * Time.deltaTime;
            }
            if (_drawAmount > 1f) _drawAmount = 1f;
            else if (_drawAmount < 0f) _drawAmount = 0f;

            Player.PlayerControllerParent.GetInstance().SetAnimationFloat("BowDrawAmount", _drawAmount);

            //If player has arrow drawn and is loosing it
            if (_drawAmount > .1f && Input.GetKeyUp(KeyCode.Mouse0))
            {
                _LooseArrow();
                StartCoroutine("_ReloadCoroutine");
            }
        }


        private IEnumerator _ReloadCoroutine()
        {
            yield return new WaitForSeconds(1f);
            Reload();
        }


        private void _LooseArrow()
        {
            Rigidbody arrowRB = _equipedArrow.GetComponent<Rigidbody>();
            arrowRB.isKinematic = false;
            arrowRB.velocity = Camera.main.transform.forward * 10f * _drawAmount;
            _equipedArrow = null;        
        }
    }
}


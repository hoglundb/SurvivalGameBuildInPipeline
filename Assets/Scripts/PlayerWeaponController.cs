using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _rightHandBone;
        [SerializeField] private Transform _leftHandBone;

        [SerializeField] private List<GameObject> _equipableSlots;

        private EquipedItemContainer _equipedItem = null;

        private int _currentSlot = -1;

        [SerializeField] private bool __savePosRotOnExit;

        private Animator _anim;
        private PlayerMovement _playerMovement;
        private Transform _currentHandBone;

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();  //Reference the animator component on this player's avatar
            _leftHandBone = GameObject.Find("LeftHand").transform; //Reference the avatar's left hand bone
            _rightHandBone = GameObject.Find("RightHand").transform; //Reference the avatar's right hand bone
            _playerMovement = GetComponent<PlayerMovement>();  //Reference the player movement script on this player game object            
        }


        private void OnApplicationQuit()
        {
            //Save the equipable item setup that was done in the editor
            if (__savePosRotOnExit && _equipedItem != null)
            {
              //  _equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.localPosition = _equipedItem.gameObj.transform.localPosition;
               // _equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.localEulers = _equipedItem.gameObj.transform.localRotation.eulerAngles;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            _UnhighlightAllSlots();

            if (InventoryController.GetInstance() != null)
            {
                InventoryController.GetInstance().AssignPlayerReference(gameObject);
            }
           
        }


        // Update is called once per frame
        void Update()
        {
            //Manage player actions as long as inventory is not in use
            if (!_playerMovement.IsMovementInabled()) return;

            _SlotSelectPlayerAction();

            _PlayerInputAction();
        }


        //Responds to player input based on the type of item the player has equiped
        private void _PlayerInputAction()
        {
            if (_equipedItem == null) return;

            EquipableItemType equipedItemType = _equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.equipableItemType;
            if (equipedItemType == EquipableItemType.BOW)
            {
                _PlayerInputActionBow();
            }
            else if (equipedItemType == EquipableItemType.MELEE)
            {
                _PlayerInputActionMelee();
            }
            else if (equipedItemType == EquipableItemType.GUN)
            {
                _PlayerInputActionGun();
            }

            else if (equipedItemType == EquipableItemType.WAND)
            {
                _PlayerInputActionWand();
            }
        }


        //Handles player input action when a player has a bow equiped
        private void _PlayerInputActionBow()
        {
            //If player not reloading, aiming or firing. Allow player to draw the bow back
            AnimatorStateInfo curState = _anim.GetCurrentAnimatorStateInfo(0);

            //if no arrow equiped yet, load an arrow from the inventory onto the bow
            if (!_equipedItem.bowController.HasLoadedArrow())
            {
                _anim.SetTrigger("ReloadBow");
                _equipedItem.bowController.LoadArrow();
            }

            //The reload cycle tells us we can set the draw amount back to 0
            if (curState.IsTag("ReloadAnimation"))
            {
                _equipedItem.bowController.SetDrawAmount(0);
            }


            if (curState.IsTag("HoldingItemAnimation"))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _anim.SetTrigger("DrawBow");
                }
            }

            //If player has the bow drawn or is drawing the bow, allow firing upon button release.
            else if (curState.IsTag("DrawAnimation") || curState.IsTag("AimAnimation"))
            {
                //Player releases the shoot key
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    _anim.ResetTrigger("DrawBow");
                    _anim.SetTrigger("FireBow");
                    _equipedItem.bowController.LooseArrow();
                    return;
                }
                //Player is drawing bow so set bow draw amount to the percent through the draw animation
                float drawAmount = 1f;
                if (curState.IsTag("DrawAnimation"))
                {
                    drawAmount = _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                }
                drawAmount = Mathf.Min(1, drawAmount);
                _equipedItem.bowController.SetDrawAmount(drawAmount);
            }
        }


        //Handles player input action when player has a melee item equiped
        private void _PlayerInputActionMelee()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _anim.SetTrigger(_equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.animTriggerMelee);
            }
        }


        private int _curSpellIndex = 1;
        private void _PlayerInputActionWand()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _curSpellIndex = 1;
                _anim.SetTrigger(_equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.animTriggerMelee);
                StartCoroutine("_PlayerActionCastSpellCoroutine", _equipedItem.wandControllerComponent);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _anim.SetTrigger(_equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.animTriggerMelee);
                StartCoroutine("_PlayerActionCastSpellCoroutine", _equipedItem.wandControllerComponent);
                _curSpellIndex = 2;
            }
        }


        //Delays the spell casting by the specified amount of time. Then tell the want to cast the spell. 
        private IEnumerator _PlayerActionCastSpellCoroutine(WandController wandController)
        {           
            yield return new WaitForSeconds(.7f);
            wandController.ShootSpell(Camera.main.transform.forward, _curSpellIndex);
        }


        //Handles player input action when player has a gun equiped
        private void _PlayerInputActionGun()
        { 
           //TODO
        }


        private void _SlotSelectPlayerAction()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentSlot = 0;
                _HighlightCurrentSlot();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentSlot = 1;
                _HighlightCurrentSlot();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _currentSlot = 2;
                _HighlightCurrentSlot();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _currentSlot = 3;
                _HighlightCurrentSlot();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _currentSlot = 4;
                _HighlightCurrentSlot();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _currentSlot = 5;
                _HighlightCurrentSlot();
            }
        }


        private void _HighlightCurrentSlot()
        {
            _UnhighlightAllSlots();

            if (_currentSlot > -1)
            {
                _equipableSlots[_currentSlot].GetComponent<EquipableSlotSelector>().Highlight();
                _EquipedItem(_equipableSlots[_currentSlot]);
            }
        }


        private void _EquipedItem(GameObject gameObjSlotToEquip)
        {
            //This stays null if player is attempting to equip an empty slot or an unequipable item. 
            if (_equipedItem != null)
            {
                _equipedItem.gameObj.SetActive(false);
                _equipedItem = null;
            }


            //Ignore if selecting an empty slot. 
            ItemSlot itemSlot = gameObjSlotToEquip.GetComponent<ItemSlot>();
            if (!itemSlot.HasItemInSlot())
            {
                _anim.SetTrigger("PlayerIdle");
                return;
            }

            //Ignore if slot contains a non-equipable item
            GameObject slotGameObj = itemSlot.GetCurrentItemInSlot().GetComponent<DragDrop>().GetAttachedGameObject();
            InventoryItem invenItem = slotGameObj.GetComponent<InventoryItem>();
            if (!invenItem.inventoryItemObj.isEquipable)
            {
                _anim.SetTrigger("PlayerIdle");
                return;
            }

            //Reference the equiped item so player can handle it.            
            _equipedItem = new EquipedItemContainer
            {
                equipableItemComponent = slotGameObj.GetComponent<EquipableItem>(),
                bowController = slotGameObj.GetComponent<BowController>(), //Will be null if not a bow
                wandControllerComponent = slotGameObj.GetComponent<WandController>(), //will be null if not a wand
                inventoryItemComponent = invenItem,
                gameObj = slotGameObj,
                rigidbodyComponent = slotGameObj.GetComponent<Rigidbody>(),
                relativePosition = invenItem.inventoryItemObj.equipableItemInfo.localPosition,
                relativeEulers = invenItem.inventoryItemObj.equipableItemInfo.localEulers,
            };

            //Set the hand bone to parent the item to (item will define which hand holds it in it's scriptable object)
            _currentHandBone = _rightHandBone;
            if (_equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.parentHand == LeftOrRight.LEFT) _currentHandBone = _leftHandBone;

            //Set the equipable item to be active in the player's hand and kickoff the animation to hold that item
            _equipedItem.rigidbodyComponent.isKinematic = true;
            _equipedItem.gameObj.SetActive(true);
            _equipedItem.gameObj.transform.parent = _currentHandBone.transform;
            _equipedItem.gameObj.transform.localPosition = _equipedItem.relativePosition;
            _equipedItem.gameObj.transform.localRotation = Quaternion.Euler(_equipedItem.relativeEulers);
            _anim.SetTrigger(_equipedItem.inventoryItemComponent.inventoryItemObj.equipableItemInfo.animTriggerHold);
        }


        private void _UnhighlightAllSlots()
        {
            for (int i = 0; i < _equipableSlots.Count; i++)
            {
                _equipableSlots[i].GetComponent<EquipableSlotSelector>().Unhighlight();
            }
        }


        //Called when player adds or removes item from slot that is currently equiped. Need to update since currently selected slot may have changed
        public void OnPlayerUpdateEquipedItem()
        {
            _HighlightCurrentSlot();
        }


        //References all the components for a game object that is equiped. 
        private class EquipedItemContainer
        {
            public GameObject gameObj;
            public InventoryItem inventoryItemComponent;
            public EquipableItem equipableItemComponent;
            public BowController bowController; //this component will be null if equiped item is not a bow
            public WandController wandControllerComponent; //This component will be null if equiped item is not a want
            public Rigidbody rigidbodyComponent;
            public Vector3 relativePosition;
            public Vector3 relativeEulers;
        }
    }
}






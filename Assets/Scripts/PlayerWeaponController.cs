using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform _rightHandBone;

    [SerializeField] private List<GameObject> _equipableSlots;

    private EquipedItemContainer _equipedItem = null;

    private int _currentSlot = -1;

    [SerializeField]  private bool __savePosRotOnExit;


    private Animator _anim;


    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();             
    }


    private void OnApplicationQuit()
    {
        //Save the equipable item setup that was done in the editor
        if (__savePosRotOnExit && _equipedItem != null)
        {
            _equipedItem.invnentoryItem.inventoryItemObj.localPosition = _equipedItem.gameObj.transform.localPosition;
            _equipedItem.invnentoryItem.inventoryItemObj.localEulers = _equipedItem.gameObj.transform.localRotation.eulerAngles;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _UnhighlightAllSlots();
    }


    // Update is called once per frame
    void Update()
    {
        _SlotSelectPlayerAction();

        _PlayerInputAction();
    }


    private void _PlayerInputAction()
    {
        if (_equipedItem == null) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _anim.SetTrigger(_equipedItem.invnentoryItem.inventoryItemObj.animTriggerMelee);
        }
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
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentSlot = 2;
            _HighlightCurrentSlot();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            _currentSlot = 3;
            _HighlightCurrentSlot();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            _currentSlot = 4;
            _HighlightCurrentSlot();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
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
        _equipedItem = null;

        //Ignore if selecting an empty slot. 
        ItemSlot itemSlot = gameObjSlotToEquip.GetComponent<ItemSlot>();
        if (!itemSlot.HasItemInSlot())
        {
            return;
        }

        //Ignore if slot contains a non-equipable item
        GameObject slotGameObj = itemSlot.GetCurrentItemInSlot().GetComponent<DragDrop>().GetAttachedGameObject();
        InventoryItem invenItem = slotGameObj.GetComponent<InventoryItem>();
        if (!invenItem.inventoryItemObj.isEquipable)
        {
            return;
        }

        //Reference the equiped item so player can handle it.            
        _equipedItem = new EquipedItemContainer
        {
            equipableItem = slotGameObj.GetComponent<EquipableItem>(),
            invnentoryItem = invenItem,
            gameObj = slotGameObj,
            rigidbody = slotGameObj.GetComponent<Rigidbody>(),
            relativePosition = invenItem.inventoryItemObj.localPosition,
            relativeEulers = invenItem.inventoryItemObj.localEulers,
        };
        _equipedItem.rigidbody.isKinematic = true;
        _equipedItem.gameObj.SetActive(true);
        _equipedItem.gameObj.transform.parent = _rightHandBone.transform;
        _equipedItem.gameObj.transform.localPosition = _equipedItem.relativePosition;
        _equipedItem.gameObj.transform.localRotation = Quaternion.Euler(_equipedItem.relativeEulers);
        _anim.SetTrigger(_equipedItem.invnentoryItem.inventoryItemObj.animTriggerHold);
    }


    private void _UnhighlightAllSlots()
    {
        for (int i = 0; i < _equipableSlots.Count; i++)
        {
            _equipableSlots[i].GetComponent<EquipableSlotSelector>().Unhighlight();
        }
    }


    //References all the components for a game object that is equiped. 
    private class EquipedItemContainer
    {
        public GameObject gameObj;
        public InventoryItem invnentoryItem;
        public EquipableItem equipableItem;
        public Rigidbody rigidbody;
        public Vector3 relativePosition;
        public Vector3 relativeEulers;
    }

}





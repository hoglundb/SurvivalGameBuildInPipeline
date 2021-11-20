using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] public InventoryItemScriptableObj inventoryItemObj;

    [SerializeField] private Rigidbody _rigidbody;

    private bool _wasThrownOnGround = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    public void DropItem()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
        if (!_wasThrownOnGround)
        {
            _wasThrownOnGround = true;
            transform.up = -1 * transform.up;
        }
      
       // _rigidbody.freezeRotation = true;
        _rigidbody.isKinematic = false;
    }
}



public interface IInventoryItem
{
    abstract void DropItem();
}
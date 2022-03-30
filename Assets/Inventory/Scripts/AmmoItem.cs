using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Component attached to prefab items that can be used as ammo. Contains the metadata pertaining to how this ammo item is used. 
/// </summary>
[RequireComponent(typeof(InventoryItem))]
public class AmmoItem : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _hasBeenFired = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        enabled = false;
    }
    public void Fire(Vector3 direction, float speed)
    {
         transform.parent = null;
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.velocity = direction * speed;
        _hasBeenFired = true;
    }

    private void Update()
    {
        if (_hasBeenFired)
        {
            transform.up = _rb.velocity.normalized;                 
        }

    }
}

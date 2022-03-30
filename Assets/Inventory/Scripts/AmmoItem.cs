using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Component attached to prefab items that can be used as ammo. Contains the metadata pertaining to how this ammo item is used. 
/// </summary>
[RequireComponent(typeof(InventoryItem))]
public class AmmoItem : MonoBehaviour
{
    /// <summary>
    /// The rigid body component on this game object
    /// </summary>
    private Rigidbody _rb;


    /// <summary>
    /// Initialize component references and disable this script intially.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        enabled = false;
    }


    private void Update()
    {
        transform.up = _rb.velocity.normalized;
    }


    /// <summary>
    /// Called from the bow class to shoot this
    /// </summary>
    /// <param name="direction">A normalized vector specifying the direction to launch the projectile</param>
    /// <param name="speed">A float specifying the magintute of the arrow's initial velocity.</param>
    public void Fire(Vector3 direction, float speed)
    {
        // have the arrow start a tiny bit out front so it looks better comming off the bow
        transform.position = transform.position + transform.up * .5f;

        transform.parent = null;
        _rb.useGravity = true;
        _rb.isKinematic = false;       
        _rb.velocity = transform.up * speed;
        _hasBeenFired = true;
    }

}

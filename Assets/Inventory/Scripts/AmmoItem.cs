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
    /// The collision layers that the arrow should not interact with. We do this for things the arrow should be able
    /// to pass through or ignore, such as the collider on the player that shot it. 
    /// </summary>
    [SerializeField] private LayerMask _ignoreCollisionsWith;

    /// <summary>
    /// Tracks the position of the projectile each frame so we can reset it's position to what it was
    /// at the previous frame once it hits something.
    /// </summary>
    private Vector3 _posPrevFrame;

    /// <summary>
    /// The rigid body component on this game object
    /// </summary>
    private Rigidbody _rb;

    /// <summary>
    /// Turn this to true when the projectile hits a game object. Reset to false when first firing. 
    /// </summary>
    private bool _hasHitSomething = false;


    /// <summary>
    /// Initialize component references and disable this script intially.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        enabled = false;
    }


    /// <summary>
    /// Manage the projectile flight one it has been launched. Update won't be called before then since this script is 
    /// inactive until after.
    /// </summary>
    private void Update()
    {
        if (!_hasHitSomething)
        {
            _posPrevFrame = transform.position;
            transform.up = _rb.velocity.normalized;
        }
    }


    /// <summary>
    /// Called from the bow class to shoot this
    /// </summary>
    /// <param name="direction">A normalized vector specifying the direction to launch the projectile</param>
    /// <param name="speed">A float specifying the magintute of the arrow's initial velocity.</param>
    public void Fire(Vector3 direction, float speed)
    {
        GetComponent<Collider>().isTrigger = true;
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _hasHitSomething = false;

        // have the arrow start a tiny bit out front so it looks better comming off the bow
        transform.position = transform.position + transform.up * .5f;
        transform.parent = null;
        _rb.useGravity = true;
        _rb.isKinematic = false;       
        _rb.velocity = transform.up * speed;
    }


    /// <summary>
    /// Handle the logic when arrow hits something based on it's layer.
    /// </summary>
    /// <param name="other">The collider on game object this projectile collided with</param>
    private void OnTriggerEnter(Collider other)
    {

        if (_hasHitSomething || _ignoreCollisionsWith == (_ignoreCollisionsWith | 1 << other.gameObject.layer))
        {
            return;
        }
        _hasHitSomething = true;
        Vector3 velAtHit = _rb.velocity;
        Vector3 aveAtHit =  (_rb.velocity.normalized + transform.up.normalized) * .5f;
        _rb.velocity = Vector3.zero;
        transform.parent = other.transform;
        transform.position = _posPrevFrame;
        _rb.isKinematic = true;

        // The projectile might not quite be in the object it hit. Raycast to figure out exactly where to put it.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, aveAtHit, out hit, Mathf.Infinity))
        {
            _posPrevFrame = hit.point - aveAtHit * .1f;
            transform.position = _posPrevFrame;
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForceAtPosition(velAtHit * .1f, hit.point, ForceMode.Impulse);
            }
        }

        enabled = false;
    }

}

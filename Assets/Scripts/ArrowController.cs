using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script resides on a arrow game object and manages the arrow during it's life cycle. 
public class ArrowController : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isLanched = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public void EnablePhysics()
    {
        _rb.isKinematic = false;
    }


    public void DisablePhysics()
    {
        _rb.isKinematic = true;
    }


    //The bow controller component calls this when an arrow is loaded onto the player's bow
    public void LoadArrow()
    { 
     
    }


    //Called by the bow class when the arrow is to be shot. Applies the specified velocity to the the arrows rigidbody. 
    public void LaunchArrow(Vector3 vel, Vector3 pos)
    {
        _isLanched = true;
        transform.SetParent(null);
        transform.position = pos;
        _rb.isKinematic = false;
        _rb.velocity = vel;
    }


    private void Update()
    {
        if (_isLanched)
        {
            transform.up = _rb.velocity.normalized;
        }
       
    }
}



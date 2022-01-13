using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollsionChecker : MonoBehaviour
{
    private bool _isColliding = false;
    private float _timeSinceLastCollision;
    private void OnTriggerStay(Collider collider)
    {
        LayerMask collidedLayer = collider.gameObject.layer;
        if (collidedLayer == LayerMask.NameToLayer("FoundationBuildingBlock") || collidedLayer == LayerMask.NameToLayer("Default"))
        {
            _timeSinceLastCollision = 0f;
            _isColliding = true;
        }
    }
    
    private void Update()
    {
        //If no collision in last 1/10 of a second, reset to not colliding. 
        _timeSinceLastCollision += Time.deltaTime;
        if (_timeSinceLastCollision > .1f)
        {
            _isColliding = false;
        }
    }


    public bool IsCollidingThisFrame()
    {
        return _isColliding;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollsionChecker : MonoBehaviour
{
    [SerializeField] private Transform _bottomCorner1;
    [SerializeField] private Transform _bottomCorner2;
    [SerializeField] private Transform _bottomCorner3;
    [SerializeField] private Transform _bottomCorner4;
    [SerializeField] private Transform _topCorner1;
    [SerializeField] private Transform _topCorner2;
    [SerializeField] private Transform _topCorner3;
    [SerializeField] private Transform _topCorner4;

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



    //Checks that no bottom corner of the block is above the ground. 
    private bool _HasTopCornerBelowGround()
    {
        if (!_IsPointAboveGround(_topCorner1.position)) return true;
        if (!_IsPointAboveGround(_topCorner2.position)) return true;
        if (!_IsPointAboveGround(_topCorner3.position)) return true;
        if (!_IsPointAboveGround(_topCorner4.position)) return true;
        return false;
    }


    //Checks that no top corder of the block is below the ground
    private bool _HasBottomCornerAboveGround()
    {
        if (_IsPointAboveGround(_bottomCorner1.position)) return true;
        if (_IsPointAboveGround(_bottomCorner2.position)) return true;
        if (_IsPointAboveGround(_bottomCorner3.position)) return true;
        if (_IsPointAboveGround(_bottomCorner4.position)) return true;
        return false;
    }


    //Samples the ground height at the x,z point of the specified point. 
    private bool _IsPointAboveGround(Vector3 point)
    {
        Vector3 abovePoint = point + Vector3.up * 4;
        RaycastHit hit;
        if (Physics.Raycast(abovePoint, Vector3.down, out hit, 10f))
        {
            return (hit.point.y < point.y);
      
        }
        return false;
    }


    public bool IsCollidingThisFrame()
    {
        if (_isColliding || _HasBottomCornerAboveGround() || _HasTopCornerBelowGround()) return true;
        return false;
    }
}

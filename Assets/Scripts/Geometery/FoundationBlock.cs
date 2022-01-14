using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class exists on all foundation blocks
public class FoundationBlock : MonoBehaviour
{
    private BlockCollsionChecker _blockCollisionCheckerComponent;
    private void Awake()
    {
        _blockCollisionCheckerComponent = GetComponent<BlockCollsionChecker>();
    }


    public bool IsCollidingThisFrame()
    {
        if (_blockCollisionCheckerComponent != null)
        {
            return _blockCollisionCheckerComponent.IsCollidingThisFrame();
        }
        return true;
    }

    //When the block is placed by the player, reset it's material and disable the collision checking script so that we save on resources. 
    public void OnBlockPlace()
    {
        GetComponent<BlockMaterialManager>().ResetMaterial();
        Destroy(GetComponent<Rigidbody>());
    }
}

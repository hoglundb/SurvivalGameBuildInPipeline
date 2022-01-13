using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class exists on all foundation blocks
public class FoundationBlock : MonoBehaviour
{
    [SerializeField] private Material _validPlacementMaterial;
    [SerializeField] private Material _invalidPlacementMaterial;
    [SerializeField] private GameObject _childObjWithMesh;
    private Material _origonalMaterial;

    private Renderer _childMeshRenderer;
    private BlockCollsionChecker _blockCollisionCheckerComponent;
    private void Awake()
    {
        _childMeshRenderer = _childObjWithMesh.GetComponent<MeshRenderer>();
        _origonalMaterial = _childMeshRenderer.material;
        _blockCollisionCheckerComponent = GetComponent<BlockCollsionChecker>();
    }


    public void UpdatePlacementMaterial(bool canPlace)
    {
        if (canPlace)
        {
            _childMeshRenderer.material = _validPlacementMaterial;
        }
        else 
        {
            _childMeshRenderer.material = _invalidPlacementMaterial;
        }
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
        _childMeshRenderer.material = _origonalMaterial;
        Destroy(GetComponent<Rigidbody>());
    }
}

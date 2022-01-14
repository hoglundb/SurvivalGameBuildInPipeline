using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterialManager : MonoBehaviour
{
    [SerializeField] private GameObject _childObjWithMesh;
    private Material _origonalMaterial;
    private Renderer _childMeshRenderer;
    

    private void Awake()
    {
        _childMeshRenderer = _childObjWithMesh.GetComponent<MeshRenderer>();
        _origonalMaterial = _childMeshRenderer.material;
    }

    public void UpdatePlacementMaterial(Material mat)
    {
        _childMeshRenderer.material = mat;
    }


    public void ResetMaterial()
    {
        _childMeshRenderer.material = _origonalMaterial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterialManager : MonoBehaviour
{
    [Header("Mesh ref for updating textures at runtime")]
    [SerializeField] private List<Renderer> _meshes;


    private List<Material> _origonalMaterials;

    private void Awake()
    {
        _origonalMaterials = new List<Material>();

        //Cache the meshes on the game object so we can reset them once the object has been placed by the player. 
        foreach (var m in _meshes)
        {           
            _origonalMaterials.Add(m.GetComponent<MeshRenderer>().material);
        }
    }

    public void UpdatePlacementMaterial(Material mat)
    {
        foreach (var m in _meshes)
        {
            m.material = mat;
        }
    }


    public void ResetMaterial()
    {
        for (int i = 0; i < _meshes.Count; i++)
        {
            _meshes[i].material = _origonalMaterials[i];
        }       
    }

}

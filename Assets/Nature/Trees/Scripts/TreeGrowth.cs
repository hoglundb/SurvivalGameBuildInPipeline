using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    [SerializeField] private Transform _treeObj;
    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum TreeTypes
{ 
   Broadleaf, 
   Fir,
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogChunkController : MonoBehaviour
{

    [SerializeField] [Range(0, 1)] private float _health;
    private float _sizeScale;

    //Called when this component is added to a log chunk game object. Tells it how much health to start with based on the size of the tree that spawned it. 
    public void Init(float treeSizeScale)
    {
        _sizeScale = treeSizeScale;
    }


    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
    }
  
}

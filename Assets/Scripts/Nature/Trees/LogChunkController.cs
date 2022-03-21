using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
public class LogChunkController : MonoBehaviour, IMeleeAbleItem
{

    [SerializeField] [Range(0, 1)] private float _health;
    private Rigidbody _rb;
    private float _sizeScale;


    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        _rb.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)).normalized * 5f, ForceMode.Impulse);
    }

    //Called when this component is added to a log chunk game object. Tells it how much health to start with based on the size of the tree that spawned it. 
    public void Init(float treeSizeScale)
    {
        _sizeScale = treeSizeScale;
    }


    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;

        if (_health <= 0)
        {
            LogBundleSpawner.GetInstance().SpawnLogs(5, transform);
            Destroy(gameObject);
        }
    }

    
  
}

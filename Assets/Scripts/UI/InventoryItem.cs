using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] public InventoryItemScriptableObj inventoryItemObj;

    private Rigidbody _rigidbody;

    private bool _wasThrownOnGround = false;
    private float _initialDrag = 1f;
    private Vector3 _randomDropOffset;

    private void Awake()
    {
        _randomDropOffset = new Vector3();

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
        }

        if (gameObject.activeSelf && _rigidbody != null)
        {
            _initialDrag = _rigidbody.drag;
            StartCoroutine(StopRollingCoroutine());
        }
       
    }


    public void DropItem()
    {
        Debug.LogError("player dropping item");
        if (gameObject.activeSelf && _rigidbody != null)
        {
            return;
        }

        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
        _randomDropOffset.x = _randomDropOffset.y = _randomDropOffset.z = Random.Range(-.1f, .1f);
        transform.position += _randomDropOffset;
        gameObject.SetActive(true);
        if (!_wasThrownOnGround)
        {
            _wasThrownOnGround = true;
            transform.up = -1 * transform.up;
        }     
    }


        //Incrementially increases drag on the object to slow it's rolling to a stop. 
        public IEnumerator StopRollingCoroutine()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 10; i++)
        {
            _rigidbody.drag += 4f;
            yield return new WaitForSeconds(3f);
        }

        _rigidbody.drag = 1000f;
    }
}



public interface IInventoryItem
{
    abstract void DropItem();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] public InventoryItemScriptableObj inventoryItemObj;

    private Rigidbody _rigidbody;

    private bool _wasThrownOnGround = false;
    private float _initialDrag = 1f;


    private void Awake()
    {
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


    //Called from the DragDrop component holding the game object when the player is dropping the item. Re-enable the object and then drop it onto the ground. 
    public void DropItem()
    {
        //re-enable any coliders on the object the player is dropping. 
        foreach (Collider c in gameObject.GetComponents<Collider>())
        {
            c.enabled = true;
        }

        if (gameObject.activeSelf && _rigidbody != null)
        {
            //Debug.LogError("no rigidbody");
            return;
        }
        if (!_wasThrownOnGround)
        {
            _wasThrownOnGround = true;
            transform.up = -1 * transform.up;
        }

        Vector3 newPos = GameObject.Find("PlayerPrefab").transform.position;
        newPos.y += 10f;
        Vector3 forwards = GameObject.Find("PlayerPrefab").transform.forward.normalized * .05f;
        forwards.y = 0;
        newPos += forwards;
        transform.position = newPos;
        gameObject.SetActive(true);     
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This component is attached to the part of a tree that falls over when a player chops it. 
When this objects heald reaches zero, it breaks apart into smaller log pieces.  
*/
namespace Tree
{
    public class FallenTreeDamageReciever : MonoBehaviour, IMeleeAbleItem
    {
        [Header("Sibling component with log chunks")]
        [SerializeField] private GameObject _trunkChunksParentObj;        

        private float _health;
        private bool _hasBrokenApart = false;

        //Cache the rigid bodies on each chunk of the tree that this will eventually break into
        private List<Rigidbody> _trunkChunkRigidBodyList;


        private void Awake()
        {
            _trunkChunksParentObj.SetActive(false);

            _trunkChunkRigidBodyList = new List<Rigidbody>();
            foreach (Transform t in _trunkChunksParentObj.transform)
            {
                _trunkChunkRigidBodyList.Add(t.GetComponent<Rigidbody>());
            }
        }


        //Called by a component that is doing damage to this fallen tree. (Ex: Could be a melee weapon)
        public void TakeDamage(float damageAmount)
        {
            //Nothing can happen if this item has already had it's health reduced to zero. 
            if (_hasBrokenApart) return;

            //TODO: decrement damage based on melee item doing the damage
            _health = 0f;

            if (_health <= 0)
            {
                _BreakApart();
            }
        }


        //Called when the tree health reaches zero. Breaks the fallen tree into it's smaller pieces. 
        private void _BreakApart()
        {
            //Activate the sibling game object that contains the smaller chunks of the tree.
            _trunkChunksParentObj.SetActive(true);

            //line up the chunks so the match the loc/rot of this fallen tree
            _trunkChunksParentObj.transform.position = transform.position;
            _trunkChunksParentObj.transform.rotation = transform.rotation;

            //Add a bit of force to the pieces so they dramatically break apart. 
            foreach (Rigidbody rb in _trunkChunkRigidBodyList)
            {
                rb.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)).normalized * 5f, ForceMode.Impulse);
                rb.gameObject.AddComponent<LogChunkController>();
            }

            _hasBrokenApart = true;

            //This game object is now destroyed since only the smaller tree chunks remain. 
            Destroy(gameObject);
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tree
{
    [RequireComponent(typeof(Rigidbody))] 
    public class TreeTrunkController : MonoBehaviour, IMeleeAbleItem
    {
        [SerializeField] private List<GameObject> _branches;
        [SerializeField] private List<GameObject> _trunkChunks;
        [SerializeField] private float _healthOnceFallen = 1f;
        private Rigidbody _rb;
        private TreeController _treeControllerParent;
        private bool _isStanding = true;

        private void Awake()
        {
            //Set up the reference to the tree controller that should be attachet to the top level game object of this tree prefab
            _treeControllerParent = transform.parent.parent.GetComponent<TreeController>();
            if (_treeControllerParent == null) Debug.LogError("Error: No TreeController component on this tree trunk's parent game object");

            //Set up the rigid body reference and disable physics initially
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
        }


        //Called by the parent TreeController component when the health of the standing tree reaches zero. 
        public void FallOver()
        {
            //Set this flag to know that the trunk has fallen and will now take individual damage, instead of applying damage to the parent obj.
            _isStanding = false;

            //Make the branches on the tree disapear as it starts to fall over
            foreach (var branch in _branches)
            {
                branch.SetActive(false);
            }

            //Activate the rigidbody physics on this tree trunk and apply some ramdom tourque to make it start falling over
            _rb.isKinematic = false;
        }


        //Called when the player chops the tree. If tree still standing, register damage with the parent tree obj. Otherwise take damage until braking apart into logs
        public void TakeDamage(float damageAmount)
        {
            if (_isStanding)
            {
                _treeControllerParent.TakeDamageFromChild(damageAmount);
            }
            else 
            {
                _healthOnceFallen -= damageAmount;
                if (_healthOnceFallen <= 0f) _DestroyTreeTrunk();
            }
        }



        //Called when health is zero on this tree trunk (once fallen and meleed a few times)
        private void _DestroyTreeTrunk()
        {
            _rb.isKinematic = true;

            foreach (var treeChunk in _trunkChunks)
            {
                treeChunk.transform.parent = null;
                treeChunk.gameObject.SetActive(true);
                treeChunk.AddComponent(typeof(LogChunkController));
            }

            //Notify the top level parent object to destroy itself since the log chunks are no longer attached to it.
            Destroy(_treeControllerParent.gameObject);
        }
    }
}


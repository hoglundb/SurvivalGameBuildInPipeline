using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tree
{
    /*
      Class to hold the various methods to controll behavior on a tree, allowing the player to interact with it.
      No Update methods are present, becuase of the large number of these game objects in the scene. 
    */
    public class TreeController : MonoBehaviour, IMeleeAbleItem
    {
        [Header("Reference the parts of the tree model")]
        [SerializeField] private GameObject _treeTrunkGameObj;
        [SerializeField] private GameObject _treeStumpGameObj;
        [SerializeField] private GameObject _branchesGameObj;

        [Header("Tree Stats")]
        [SerializeField] [Range(0, 1)] private float _health; //the amount of health on the tree. Tree falls when it reaches 0.
        [SerializeField] [Range (0f, 1f)] private float _damageScale; //Scale damage recieving to this tree has based on it's size. 

        //References to the physics components on the tree game objects
        private Rigidbody _treeTrunkRigidBody;
        private Collider _treeTrunkCollider;
        private Collider _treeCollider;

        //Reference to each tree trunk chunk and it's components that we will cache
        private bool _hasFallen = false;
        

        private void Awake()
        {
            //Reference components. 
            _treeTrunkRigidBody = _treeTrunkGameObj.GetComponent<Rigidbody>();
            _treeTrunkCollider = _treeTrunkGameObj.GetComponent<Collider>();
            _treeCollider = GetComponent<Collider>();

            //Disable colliders/physics on the part of the tree that will eventually fall down when health reaches zero. 
            _treeTrunkRigidBody.isKinematic = true;
            _treeTrunkCollider.enabled = false;  
            
        }


        //Called externally when something does damage to this tree. 
        public void TakeDamage(float damageAmount)
        {
            if (!_hasFallen)
            {
                _health -= damageAmount * _damageScale;

                if (_health <= 0)
                {
                    _FallOver();
                }
            }
        }


        //Makes the tree fall. This gets called internally when the tree health reaches 0. Disable the colliders on this and enable the part of the tree that will fall over. The FallenTreeeDamageReciever takes over from here. 
        private void _FallOver()
        {
            //Branches will no longer be rendered 
       
      //      _branchesGameObj.AddComponent(typeof(Trees.TreeBranches));
            //The primary collider is disabled, since player will only be able to now interact with the fallen portion of the tree. 
            _treeCollider.isTrigger = true;
            _treeCollider.enabled = false;

            //enable colliders and physics on the part of the tree that will now fall over. 
            _treeTrunkCollider.enabled = true;
            _treeTrunkRigidBody.isKinematic = false;

            //Start the tipping over of the falling tree by applying torque to it's rigidbody. TODO: randomize this or base if off of player position. 
            _treeTrunkRigidBody.AddForce(transform.right * 2f * transform.localScale.magnitude, ForceMode.Impulse);

            _hasFallen = true;
        }
    }
}


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
        [SerializeField] private GameObject _treeBranchesGameObj;
        [SerializeField] private GameObject _trunkChunksParentObj;

        [Header("Tree Stats")]
        [SerializeField] [Range(0, 1)] private float _health; //the amount of health on the tree. Tree falls when it reaches 0.
        [SerializeField] [Range (0f, 1f)] private float _damageScale; //Scale damage recieving to this tree has based on it's size. 

        //References to the physics components on the tree game objects
        private Rigidbody _treeTrunkRigidBody;
        private Collider _treeTrunkCollider;
        private Collider _treeCollider;

        //Reference to each tree trunk chunk and it's components that we will cache
        private List<GameObject> _trunkChunksObjList;
        private List<Rigidbody> _trunkChunkRigidBodyList;
        private bool _hasFallen = false;
        

        private void Awake()
        {
            //Reference components. 
            _treeTrunkRigidBody = _treeTrunkGameObj.GetComponent<Rigidbody>();
            _treeTrunkCollider = _treeTrunkGameObj.GetComponent<Collider>();
            _treeCollider = GetComponent<Collider>();

            //Disable colliders/physics enabled on the child objects. These are only activated if the tree gets chopped down. 
            _treeTrunkRigidBody.isKinematic = true;
            _treeTrunkCollider.isTrigger = true;

            _trunkChunksParentObj.SetActive(false);

            _trunkChunkRigidBodyList = new List<Rigidbody>();
            foreach (Transform t in _trunkChunksParentObj.transform)
            {
                _trunkChunkRigidBodyList.Add(t.GetComponent<Rigidbody>());
            }
        }


        //Called externally when something does damage to this tree. 
        public void TakeDamage(float damageAmount)
        {
            Debug.LogError("taking damage");
            if (!_hasFallen)
            {
                _health -= damageAmount * _damageScale;

                if (_health <= 0)
                {
                    _FallOver();
                }
            }
            else 
            {
                _BreakIntoChunks();
            }
        }


        //Makes the tree fall. This gets called internally when the tree health reaches 0. 
        private void _FallOver()
        {
            //Branches will no longer be rendered 
            _treeBranchesGameObj.gameObject.SetActive(false);

            //The primary collider is disabled and we enable the collider/rigidbody on the falling portion of the tree. 
            _treeCollider.isTrigger = true;
            _treeCollider.enabled = false;
            _treeTrunkCollider.isTrigger = false;
            _treeTrunkRigidBody.isKinematic = false;

            _treeTrunkRigidBody.AddTorque(transform.right * 75f, ForceMode.Force);

            _hasFallen = true;
        }



        //Breaks a the trunk into smaller pieces.
        private void _BreakIntoChunks()
        {
            _treeTrunkGameObj.SetActive(false);
            _trunkChunksParentObj.SetActive(true);

            //line up the chunks so the match the loc/rot of the trunk 
            _trunkChunksParentObj.transform.position = _treeTrunkGameObj.transform.position;
            _trunkChunksParentObj.transform.rotation = _treeTrunkGameObj.transform.rotation;

            //Add a bit of force to the pieces so they dramatically break apart. 
            foreach (Rigidbody rb in _trunkChunkRigidBodyList) 
            {
                rb.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100)).normalized * 5f, ForceMode.Impulse);
                rb.gameObject.AddComponent<LogChunkController>();
            }
        }



    }
}


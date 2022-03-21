using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tree
{
    [RequireComponent(typeof(MeshCollider))]
    public class TreeStumpController : MonoBehaviour, IMeleeAbleItem
    {
        private float _treeStumpHealth = 1f;
        private TreeController _treeControllerParent;
        private bool _isTrunkStanding = true;

        private void Awake()
        {
            _treeControllerParent = transform.parent.GetComponent<TreeController>();
            if (_treeControllerParent == null) Debug.LogError("Error: Tree trunk parent object must have a TreeController component attached to it.");
        }


        //Called when player melees this tree stump. Either does damage to overall tree (if tree still standing) or damages the stump (if tree trunk has fallen)
        public void TakeDamage(float damageAmount)
        {
            if (_isTrunkStanding)
            {
                _treeControllerParent.TakeDamageFromChild(damageAmount);
            }
            else
            {
                _treeStumpHealth -= damageAmount;
                if (damageAmount <= 0f)
                {
                    _DestroyStump();
                }
            }
        }



        //Called when the overall tree health is zero, at which point this stump takes damage independant of the parent
        public void OnTreeTrunkFall()
        {
            _isTrunkStanding = false;
        }


        private void _DestroyStump()
        {
            //TODO
            Debug.LogError("TODO: Destroying Stump");
        }
    }
}


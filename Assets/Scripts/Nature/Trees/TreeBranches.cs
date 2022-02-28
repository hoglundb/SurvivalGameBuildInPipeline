using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Trees
{
    public class TreeBranches : MonoBehaviour
    {
        [SerializeField] private LayerMask destroyCollisionMask;


        private void OnTriggerEnter(Collider other)
        {
            if ( destroyCollisionMask == (destroyCollisionMask | (1 << other.gameObject.layer)))
            {
                StartCoroutine(DestroyBranchesCoroutine());
            }
        }


        private IEnumerator DestroyBranchesCoroutine()
        {
            yield return new WaitForSeconds(.15f);
            Destroy(gameObject);
        }
    }
}


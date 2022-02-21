using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Nature.Rocks
{
    public class RockController : MonoBehaviour, IMeleeAbleItem
    {
        [SerializeField] private GameObject _smallRockPiecePrefab;
        private float _currentHealth = 10f;
        private const int NUM_PIECES = 10;        


        //TODO implement actual damange system based on tool being used to hit this rock. 
        public void TakeDamage(float damageAmount)
        {
            _BreakApartIntoSmallRocks();
        }



        private void _BreakApartIntoSmallRocks()
        {
         
            //compute the positinos of the mall rock pieces that are going to be spawned. 
            float spacing = .15f;
            float curHeight = .05f;
            List<Vector3> spawnPositions = new List<Vector3>();
            spawnPositions.Add(new Vector3(0, curHeight, 0));
            spawnPositions.Add(new Vector3(spacing, curHeight, 0f));
            spawnPositions.Add(new Vector3(spacing, curHeight, spacing));
            spawnPositions.Add(new Vector3(0f, curHeight, spacing));
            spawnPositions.Add(new Vector3(-spacing, curHeight, spacing));
            spawnPositions.Add(new Vector3(-spacing, curHeight, -spacing));
            spawnPositions.Add(new Vector3(spacing, curHeight, -spacing));

            curHeight += .3f;
            spawnPositions.Add(new Vector3(0, curHeight, 0));
            spawnPositions.Add(new Vector3(spacing, curHeight, 0f));
            spawnPositions.Add(new Vector3(spacing, curHeight, spacing));
            spawnPositions.Add(new Vector3(0f, curHeight, spacing));
            spawnPositions.Add(new Vector3(-spacing, curHeight, spacing));

            foreach (var pos in spawnPositions)
            {
                Instantiate(_smallRockPiecePrefab, transform.position + pos, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}


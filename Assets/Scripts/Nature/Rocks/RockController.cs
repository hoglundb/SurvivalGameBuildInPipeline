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



        //Spawns the small rock pieces and destroys this rock
        private void _BreakApartIntoSmallRocks()
        {       
            float spacing = .15f;
            float curHeight = .05f;
            for (int i = 0; i < 2; i++)
            {
                Instantiate(_smallRockPiecePrefab, transform.position + new Vector3(0, curHeight, 0), Quaternion.identity);
                Instantiate(_smallRockPiecePrefab, transform.position + new Vector3(spacing, curHeight, 0f), Quaternion.identity);
                Instantiate(_smallRockPiecePrefab, transform.position + new Vector3(spacing, curHeight, spacing), Quaternion.identity);
                Instantiate(_smallRockPiecePrefab, transform.position + new Vector3(0f, curHeight, spacing), Quaternion.identity);
                Instantiate(_smallRockPiecePrefab, transform.position + new Vector3(-spacing, curHeight, spacing), Quaternion.identity);
                Instantiate(_smallRockPiecePrefab, transform.position + new Vector3(-spacing, curHeight, -spacing), Quaternion.identity);
                Instantiate(_smallRockPiecePrefab, transform.position + new Vector3(spacing, curHeight, -spacing), Quaternion.identity);
                curHeight += .3f;
            }
            Destroy(gameObject);
        }
    }
}


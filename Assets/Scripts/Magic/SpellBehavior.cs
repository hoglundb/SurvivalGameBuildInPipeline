using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Magic
{
    public class SpellBehavior : MonoBehaviour
    {
        private Rigidbody _rb;
        [SerializeField] private float _spellSpeed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }



        //The wand class will call this to lanch the spell
        public void LaunchSpell(Vector3 initialPos, Vector3 dir)
        {
            _rb.position = initialPos;
            _rb.isKinematic = false;
            _rb.velocity = dir.normalized * _spellSpeed;
        }
    }
}


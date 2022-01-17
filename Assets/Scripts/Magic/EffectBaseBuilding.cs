using Geometery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    //This script lives on the base building particle effect game object. 
    public class EffectBaseBuilding : MonoBehaviour
    {
        [SerializeField] private float _spellSpeed = 10; 
        private Rigidbody _rb;

        private bool _isBeingShot = false;
        private Vector3 _targetPos;
        private Vector3 _startPos;
        private Vector3 _shootDir;
        private float _distFromShooterToTarget;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }


        public void ShootSpellEffect(Vector3 startPos, Vector3 targetPos)
        {
            _startPos = startPos;
            _targetPos = targetPos;
            _distFromShooterToTarget = Helpers.GetDistance(startPos, targetPos);
            _shootDir = (targetPos - startPos).normalized;
            _isBeingShot = true;

            transform.position = startPos;
            transform.forward = _shootDir;
            _rb.velocity = _shootDir * _spellSpeed;
        }


        private void Update()
        {
            if (!_isBeingShot) return;

            float curDistFromStartPos = Helpers.GetDistance(_startPos, transform.position);
            if (curDistFromStartPos - 50f > _distFromShooterToTarget)
            {
                _isBeingShot = false;
                gameObject.SetActive(false);
            }
        }

    }
}


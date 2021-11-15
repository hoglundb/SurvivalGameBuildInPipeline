using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _characterController;
        private Vector3 _downwardVelocity;
        private bool _isGrounded;
        private float _pitchRotation;
        private float _curSpeed;

        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _fpsArms;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _groundDistance;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _gravity = -10f;
        [SerializeField] [Range(1f, 200f)] private float _horizontalSensitivity;
        [SerializeField] [Range(1f, 200f)] private float _verticalSensitivity;
        [SerializeField] [Range(0f, 20f)] private float _walkSpeed = 6f;
        [SerializeField] [Range(0f, 20f)] private float _runSpeed = 12f;
        [SerializeField] [Range(0f, 30f)] private float _movementTransitionSpeed = 5f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _pitchRotation = 5f;
        }

        // Update is called once per frame
        void Update()
        {        
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            if (_isGrounded && _downwardVelocity.y < 0) _downwardVelocity.y = -2f;

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _downwardVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            _downwardVelocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_downwardVelocity * Time.deltaTime);

            float horiz = Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");
            Vector3 movement = (transform.forward * vert + transform.right * horiz).normalized * Time.deltaTime * _curSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (movement.magnitude > .01f)
                {
                    _curSpeed = Mathf.Lerp(_curSpeed, _runSpeed, _movementTransitionSpeed * Time.deltaTime);
                }
            }
            else
            {
                _curSpeed = Mathf.Lerp(_curSpeed, _walkSpeed, _movementTransitionSpeed * Time.deltaTime);
            }
            _characterController.Move(movement);
            _pitchRotation -= Input.GetAxis("Mouse Y") * _verticalSensitivity * Time.deltaTime;
            _pitchRotation = Mathf.Clamp(_pitchRotation, -90f, 90f);
            _fpsArms.localRotation = Quaternion.Euler(_pitchRotation, 0f, 0f);
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _horizontalSensitivity * Time.deltaTime);
        }
    }
}


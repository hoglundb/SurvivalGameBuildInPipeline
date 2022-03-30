using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {

        #region PrivateMemberVars

        private CharacterController _characterController;
        private Vector3 _downwardVelocity;
        private bool _isGrounded;
        private float _pitchRotation;
        private float _curSpeed;
        private bool _isMovementEnabled = true;

        private Transform _lowerSpineBone;
        private Camera _mainCamera;

        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _groundDistance;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _gravity = -10f;
        [SerializeField] [Range(1f, 300)] private float _horizontalSensitivity;
        [SerializeField] [Range(1f, 300)] private float _verticalSensitivity;
        [SerializeField] [Range(0f, 20f)] private float _walkSpeed = 6f;
        [SerializeField] [Range(0f, 20f)] private float _runSpeed = 12f;
        [SerializeField] [Range(0f, 30f)] private float _movementTransitionSpeed = 5f;
        [SerializeField] float lookUpAmount;

        #endregion



        #region MonoCallbacks
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _lowerSpineBone = GlobalStaticFunctions.CustomFindChild("spine.003", transform);
            _mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

            _mainCamera.transform.parent = GlobalStaticFunctions.CustomFindChild( "spine.006",transform);
        }

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _pitchRotation = 5f;
        }


        private void LateUpdate()
        {
            Vector3 rot = _lowerSpineBone.transform.localEulerAngles;
            rot.x = _mainCamera.transform.localRotation.eulerAngles.x + lookUpAmount;
            _lowerSpineBone.transform.localRotation = Quaternion.Euler(rot);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isMovementEnabled) return;

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
            lookUpAmount = _pitchRotation;
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _horizontalSensitivity * Time.deltaTime);
        }
        #endregion



        #region PlayerMovement
        //Sets the enableMovement flag. If set to false the player movement is disabled. Used for when player needs to interact with UI.
        public void SetMovementEnablement(bool shouldEnable)
        {
            _isMovementEnabled = shouldEnable;
        }


        public bool IsMovementInabled()
        {
            return _isMovementEnabled;
        }
        #endregion


        /// <summary>
        /// Allows other components to check the direction the camera is facing. 
        /// </summary>
        /// <returns>Returns a Vector3 representing the direction the camera is facing as a normalized vector</returns>
        public Vector3 GetLookDirection()
        {
            return _mainCamera.transform.forward;
        }
    }
}


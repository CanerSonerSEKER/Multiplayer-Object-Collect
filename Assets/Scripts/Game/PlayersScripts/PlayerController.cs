using System;
using Photon.Pun;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Game.PlayersScripts
{
    public class PlayerController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;

        [SerializeField] private Transform _playerTransform;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PhotonView _photonView;
        
        private Vector3 _moveDirection;
        private Vector2 _currentInput;
        
        private bool IsSprinting => _canSprint && Input.GetKey(_sprintKey);
        private bool ShouldCrouch => Input.GetKey(_crouchKey) && _characterController.isGrounded;
    

        [Header("Functional Options")] 
        [SerializeField] private bool _canSprint = true;
        [SerializeField] private bool _canJump = true;
        [SerializeField] private bool _canCrouch = true;

        [Header("Controls")] 
        private KeyCode _sprintKey = KeyCode.LeftShift;
        private KeyCode _crouchKey = KeyCode.LeftControl;

        [Header("Movement Parameters")] 
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _crouchSpeed;

        [Header("Jumping Parameters")] 
        [SerializeField] private float _jumpForce = 15.0f;
        [SerializeField] private float _jumpGravity = 30.0f;
        [SerializeField] private float _jumpButtonGracePeriod;
        private float _ySpeed;
        private float _originalStepOffSet;
        private float _lastGroundedTime;
        private float _jumpButtonPressedTime;
        private bool _isJumping;
        private bool _isGrounded;

        [Header("Crouch Parameters")] 
        private bool _isCrouching;

        [Header("Animation Values")] 
        [SerializeField] private Animator _animator;
        
        //[Header("Camera Parameters")]
        //private Vector3 _cameraFollowVelocity = Vector3.zero;
        //private float _cameraFollowSpeed = 0.2f;
        //private float _lookAngle;
        //private float _pivotAngle;
        //private float _cameraTurnSpeed = 2f;
        //private float _minimumAngle = -35f;
        //private float _maximumAngle = 35f;


        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            CameraWork cameraWork = gameObject.GetComponent<CameraWork>();

            if (cameraWork != null)
            {
                if (_photonView.IsMine)
                {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color = Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
        }

        private void Update()
        {
            if (!_photonView.IsMine)
            {
                return;
            }
            
            if (CanMove)
            {
                HandleMovementInput();

                if (_canJump)
                {
                    HandleJump();
                }
                
                if (_canCrouch)
                {
                    HandleCrouch();
                }

                ApplyFinalMovements();

            }
        }

        private void LateUpdate()
        {
            //FollowTarget();
            //RotateCamera();
        }

        private void HandleMovementInput()
        {
            _currentInput =
                new Vector2
                (
                    (IsSprinting ? _sprintSpeed : _isCrouching ? _crouchSpeed : _walkSpeed) * Input.GetAxis("Vertical"),
                    (IsSprinting ? _sprintSpeed : _isCrouching ? _crouchSpeed : _walkSpeed) *
                    Input.GetAxis("Horizontal")
                );
            
            _animator.SetFloat("Vertical", _currentInput.x);
            _animator.SetFloat("Horizontal", _currentInput.y);
            float moveDirectionY = _moveDirection.y;
            _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x) +
                             (transform.TransformDirection(Vector3.right) * _currentInput.y);
            
            _moveDirection.y = moveDirectionY;
        }

        private void HandleJump()
        {
            _ySpeed += Physics.gravity.y * Time.deltaTime;
            if (_characterController.isGrounded)
            {
                _lastGroundedTime = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpButtonPressedTime = Time.time;
            }

            if (Time.time - _lastGroundedTime <= _jumpButtonGracePeriod)
            {
                _characterController.stepOffset = _originalStepOffSet;

                _animator.SetBool("IsGrounded", true);
                _isGrounded = true;
                _animator.SetBool("IsJumping", false);
                _isJumping = false;
                _animator.SetBool("IsFalling", false);

                if (Time.time - _jumpButtonPressedTime <= _jumpButtonGracePeriod)
                {
                    _moveDirection.y = _jumpForce;
                    _animator.SetBool("IsJumping", true);
                    _isJumping = true;
                    _jumpButtonPressedTime = 0f;
                    _lastGroundedTime = 0f;
                }
            }
            else
            {
                _characterController.stepOffset = 0;
                _animator.SetBool("IsGrounded", false);
                _isGrounded = false;

                if (_isJumping && _ySpeed < 0 || _ySpeed < -2)
                {   
                    _animator.SetBool("IsFalling", true);
                }
            }
        }

        private void HandleCrouch()
        {
                if (ShouldCrouch)
                {
                    _animator.SetBool("IsCrouching", true);
                    _canSprint = false;
                    _canJump = false;
                }
                else
                {
                    _animator.SetBool("IsCrouching", false);
                    _canSprint = true;
                    _canJump = true;
                }
        }
        
        private void ApplyFinalMovements()
        {
            if (!_characterController.isGrounded) _moveDirection.y -= _jumpGravity * Time.deltaTime;
            _characterController.Move(_moveDirection * Time.deltaTime);
        }
        
        private void FollowTarget()
        {
            
        }
        
        private void RotateCamera()
        {
            //float inputMouseY = Input.GetAxis("Mouse Y");
            //float inputMouseX = Input.GetAxis("Mouse X");
            //
            //Vector3 rotation;
            //Quaternion targetRotation;
            //
            //_lookAngle = _lookAngle + (inputMouseX * _cameraTurnSpeed);
            //_pivotAngle = _pivotAngle - (inputMouseY * _cameraTurnSpeed);
            //_pivotAngle = Mathf.Clamp(_pivotAngle, _minimumAngle, _maximumAngle);
            //
            //rotation.x = _pivotAngle;
            //rotation.y = _lookAngle;
            //targetRotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
            //transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        }


    }
}
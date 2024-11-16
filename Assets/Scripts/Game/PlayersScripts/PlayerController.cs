using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Game.PlayersScripts
{
    public class PlayerController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;
        private bool IsSprinting => _canSprint && Input.GetKey(_sprintKey);
        private bool ShouldJump => Input.GetKeyDown(_jumpKey) && _characterController.isGrounded;
        private bool ShouldCrouch => Input.GetKey(_crouchKey) /*&& !_duringCrouchAnimation*/ && _characterController.isGrounded;
    

        [Header("Functional Options")] 
        [SerializeField] private bool _canSprint = true;
        [SerializeField] private bool _canJump = true;
        [SerializeField] private bool _canCrouch = true;
        [SerializeField] private bool _canUseHeadBob = true;
        [SerializeField] private bool _canWalk = true;

        [Header("Controls")] 
        private KeyCode _sprintKey = KeyCode.LeftShift;
        private KeyCode _jumpKey = KeyCode.Space;
        private KeyCode _crouchKey = KeyCode.LeftControl;

        [Header("Movement Parameters")] 
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _crouchSpeed;

        [Header("Look Parameters")] 
        [SerializeField, Range(1, 10)] private float _lookSpeedX = 2.0f;
        [SerializeField, Range(1, 10)] private float _lookSpeedY = 2.0f;
        [SerializeField, Range(1, 100)] private float _lookAngleUp = 80.0f;
        [SerializeField, Range(1, 100)] private float _lookAngleDown = 80.0f;

        [Header("Jumping Parameters")] 
        [SerializeField] private float _jumpForce = 8.0f;
        [SerializeField] private float _jumpGravity = 30.0f;

        [Header("Crouch Parameters")] 
        private bool _isCrouching;
        private bool _duringCrouchAnimation;

        [Header("Head Parameters")] 
        [SerializeField] private float _walkRobotSpeed = 14f;
        [SerializeField] private float _walkRobotAmount = 0.05f;
        [SerializeField] private float _sprintRobotSpeed = 18f;
        [SerializeField] private float _sprintRobotAmount = 0.11f;
        [SerializeField] private float _crouchRobotSpeed = 8f;
        [SerializeField] private float _crouchRobotAmount = 0.025f;
        private float _timer;

        [Header("Animation Values")] 
        [SerializeField] private Animator _animator;
        
        [SerializeField] private Transform _playerTransform;
        [SerializeField] CharacterController _characterController;
        
        private Vector3 _moveDirection;
        private Vector2 _currentInput;

        private float _rotationX = 0;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
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
            if (ShouldJump)
            {
                _moveDirection.y = _jumpForce;
            }
        }
        
        private void HandleCrouch()
        {
            if (ShouldCrouch)
            {
                //StartCoroutine(CrouchStand());
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
        
    }
}
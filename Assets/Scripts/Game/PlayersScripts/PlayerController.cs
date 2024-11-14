using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Game.PlayersScripts
{
    public class PlayerController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;
        private bool IsSprinting => _canSprint && Input.GetKey(_sprintKey);
        private bool IsCrouching => _canCrouch && Input.GetKey(_crouchKey);
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
        [SerializeField] private float _crouchHeight = 0.5f;
        [SerializeField] private float _standingHeight = 2.0f;
        [SerializeField] private float _crouchTime = 0.25f;
        [SerializeField] private Vector3 _crouchCenter = new Vector3(0f, 0.5f, 0f);
        [SerializeField] private Vector3 _standingCenter = new Vector3(0f, 0f, 0f);
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
                CrouchMove();
            }
        }

        private void CrouchMove()
        {
            /*
            if (Input.GetKey(KeyCode.LeftControl))
            {
                _animator.SetBool("isCrouching", true);
            }
            else
            {
                _animator.SetBool("isCrouching", false);
                _isCrouching = !_isCrouching;
            }
        */
            if (IsCrouching)
            {
                _animator.SetBool("IsCrouching", true);
                _canSprint = !_canSprint;
                _canJump = !_canJump;
            }
            else
            {
                _animator.SetBool("IsCrouching", false);
                _canSprint = true;
                _canJump = true;
            }
            
        }

        /*private IEnumerator CrouchStand()
        {
            /*if (_isCrouching && Physics.Raycast(_playerTransform.transform.position, Vector3.up, 1f))
            {
                yield break;
            }#1#

            /*if (Input.GetKey(KeyCode.LeftControl))
            {
                _animator.SetBool("isCrouching", true);
                _isCrouching = !_isCrouching;
                _canJump = !_canJump;
                _canSprint = !_canSprint;
            }
            else
            {
                _animator.SetBool("isCrouching", false);
            }#1#
            
            //_duringCrouchAnimation = true;
            

            /*float timeElapsed = 0;
            float targetHeight = _isCrouching ? _standingHeight : _crouchHeight;
            float currentHeight = _characterController.height;

            Vector3 targetCenter = _isCrouching ? _crouchCenter : _standingCenter;
            Vector3 currentCenter = _characterController.center;#1#

            /*
            while (timeElapsed < _crouchTime)
            {
                _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _crouchTime);
                _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _crouchTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            #1#

            /*_characterController.height = targetHeight;
            _characterController.center = targetCenter;#1#

            //_isCrouching = !_isCrouching;

            //_duringCrouchAnimation = false;
            //_animator.SetBool("isCrouching", false);
            //_canJump = !_canJump;
            //_canSprint = !_canSprint;
            
            //yield break;

        }*/
        
        private void ApplyFinalMovements()
        {
            if (!_characterController.isGrounded) _moveDirection.y -= _jumpGravity * Time.deltaTime;
            _characterController.Move(_moveDirection * Time.deltaTime);
        }
        
    }
}
using Unity.VisualScripting;
using UnityEngine;

namespace Game.PlayersScripts
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _cameraManagerTransform;
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private Transform _mainCamera;

        [SerializeField] private LayerMask _collisionLayers;

        private Transform _targetTransform;
        private float _defaultPosition;
        private Vector3 _cameraFollowVelocity = Vector3.zero;
        private Vector3 _cameraVectorPosition;
        private float _cameraFollowSpeed = 0.2f;
        private float _cameraCollisionRadius = 2f;
        private float _cameraCollisionOffSet = 0.2f;
        private float _minimumCollisionOffSet = 0.2f;
        
        private float _lookAngle;
        private float _pivotAngle;
        private float _cameraLookSpeed = 2f;
        private float _cameraPivotSpeed = 2f;
        
        private float _minimumPivotAngle = -35f;
        private float _maximumPivotAngle = 35f;

        private void Awake()
        {
            _targetTransform = _playerTransform.transform;
            _defaultPosition = _cameraManagerTransform.localPosition.z;
        }
        
        private void LateUpdate()
        {
            FollowTarget();            
            RotateCamera();
            // HandleCameraCollisions(); -- Hala fazla yaklaşıyor kamera
            
        }

        private void FollowTarget()
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _targetTransform.position,
                ref _cameraFollowVelocity, _cameraFollowSpeed);

            transform.position = targetPosition;
        }

        private void RotateCamera()
        {
            float mouseXInput = Input.GetAxis("Mouse X");
            float mouseYInput = Input.GetAxis("Mouse Y");

            Vector3 rotation;
            Quaternion targetRotation;
            
            _lookAngle = _lookAngle + (mouseXInput * _cameraLookSpeed);
            _pivotAngle = _pivotAngle - (mouseYInput * _cameraPivotSpeed);
            _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivotAngle, _maximumPivotAngle);
            
            rotation = Vector3.zero;
            rotation.y = _lookAngle;
            targetRotation = Quaternion.Euler(rotation);
            transform.localRotation = targetRotation;
            _playerTransform.localRotation = targetRotation;
            
            rotation.x = _pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            _cameraPivot.localRotation = targetRotation;
            
        }

        private void HandleCameraCollisions()
        {
            float targetPosition = _defaultPosition;
            RaycastHit hit;
            Vector3 direction = _mainCamera.position - _cameraPivot.position;
            direction.Normalize();

            if (Physics.SphereCast(_cameraPivot.transform.position, _cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), _collisionLayers))
            {
                float distance = Vector3.Distance(_cameraPivot.position, hit.point);
                targetPosition =- (distance - _cameraCollisionOffSet);
            }

            if (Mathf.Abs(targetPosition) < _minimumCollisionOffSet)
            {
                targetPosition = targetPosition - _minimumCollisionOffSet;
            }

            _cameraVectorPosition.z = Mathf.Lerp(_mainCamera.localPosition.z, targetPosition, 0.2f);
            _mainCamera.localPosition = _cameraVectorPosition;
          
        }

        private void RotatePlayer()
        {
            
        }

    }
}
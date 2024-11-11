using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Game.PlayersScripts
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        private Vector3 _cameraFollowVelocity = Vector3.zero;
        private float _cameraFollowSpeed = 0.2f;
        
        private float _lookAngle;
        private float _pivotAngle;
        private float _cameraLookSpeed = 2f;
        private float _cameraPivotSpeed = 2f;
        
        private float _minimumPivotAngle = -35f;
        private float _maximumPivotAngle = 35f;

        private void Awake()
        {
        }


        private void LateUpdate()
        {
            FollowTarget();            
            RotateCamera();
        }

        private void FollowTarget()
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _playerTransform.position,
                ref _cameraFollowVelocity, _cameraFollowSpeed);

            transform.position = targetPosition;
        }

        private void RotateCamera()
        {
            float mouseXInput = Input.GetAxis("Mouse X");
            float mouseYInput = Input.GetAxis("Mouse Y");

            Vector3 rotation = Vector3.zero;
            Quaternion targetRotation;
            
            _lookAngle = _lookAngle + (mouseXInput * _cameraLookSpeed);
            _pivotAngle = _pivotAngle - (mouseYInput * _cameraPivotSpeed);
            _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivotAngle, _maximumPivotAngle);
            
            rotation.y = _lookAngle;
            rotation.x = _pivotAngle;
            targetRotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
            transform.rotation = targetRotation;
            _playerTransform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            
            
        }

    }
}
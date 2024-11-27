using UnityEngine;

namespace Game.PlayersScripts
{
    public class CameraWork : MonoBehaviour
    {
        [SerializeField] private float _distance = 7.0f;
        [SerializeField] private float _height = 3.0f;
        [SerializeField] private Vector3 _centerOffSet = Vector3.zero;
        [SerializeField] private bool _followOnStart = false;
        [SerializeField] private float _smoothSpeed = 0.125f;

        private Transform _cameraTransform;

        private bool _isFollowing;

        private Vector3 _cameraOffSet = Vector3.zero;
        private float _lookAngle;
        private float _pivotAngle;
        private float _minPivotAngle = -35f;
        private float _maxPivotAngle = 35f;
        private float _cameraRotateSpeed = 2f;


        private void Start()
        {
            if (_followOnStart)
            {
                OnStartFollowing();
            }
        }

        private void LateUpdate()
        {
            if (_cameraTransform == null && _isFollowing)
            {
                OnStartFollowing();
            }

            if (_isFollowing)
            {
                Follow();
                RotateCam();
            }
        }

        public void OnStartFollowing()
        {
            _cameraTransform = Camera.main.transform;
            _isFollowing = true;

            Cut();
        }

        private void Follow()
        {
            _cameraOffSet.z = -_distance;
            _cameraOffSet.y = _height;

            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                transform.position + transform.TransformVector(_cameraOffSet), _smoothSpeed * Time.deltaTime);
            
            _cameraTransform.LookAt(transform.position, _centerOffSet);
        }
        
        private void Cut()
        {
            _cameraOffSet.z = -_distance;
            _cameraOffSet.y = _height;

            _cameraTransform.position = transform.position + transform.TransformVector(_cameraOffSet);
            _cameraTransform.LookAt(transform.position + _centerOffSet);
        }
        
        private void RotateCam()
        {
            float inputX = Input.GetAxis("Mouse X");
            float inputY = Input.GetAxis("Mouse Y");

            Vector3 rotation;

            _lookAngle = _lookAngle + (inputX * _cameraRotateSpeed);
            _pivotAngle = _pivotAngle - (inputY * 2f);
            _pivotAngle = Mathf.Clamp(_pivotAngle, _minPivotAngle, _maxPivotAngle);
            
            rotation.x = _pivotAngle;
            rotation.y = _lookAngle;
            
            _cameraTransform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        }


    }
}
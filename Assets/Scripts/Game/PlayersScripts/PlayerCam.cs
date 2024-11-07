using System;
using UnityEngine;

namespace Game.PlayersScripts
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _cameraTransform;
        private Settings _mySettings;
        private float _rotationX = 0;
        private float _lookSpeedY = 2f;

        private void Awake()
        {
            _mySettings = new Settings();
        }

        private void Update()
        {
            HandleMouseLook();
            
            if (_playerTransform == false) return;

            Vector3 offSetVect = Vector3.back * _mySettings.OffSet;

            Quaternion angleAxis = Quaternion.AngleAxis(_mySettings.PanAngle, Vector3.right);

            Vector3 rotatedVect = angleAxis * offSetVect;

            _cameraTransform.position = _playerTransform.position + rotatedVect;
        }
        
        private void HandleMouseLook()
        {
            _rotationX -= Input.GetAxis("Mouse Y") * _lookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, -_mySettings.PanAngle, _mySettings.PanAngle);
            _cameraTransform.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeedY, 0);
        }
        
        
    }

    [Serializable]
    public class Settings
    {
        [SerializeField] private float _offSet = 3f;
        [SerializeField] private float _panAngle = 60f;
        public float OffSet => _offSet;
        public float PanAngle => _panAngle;
    }
    
}
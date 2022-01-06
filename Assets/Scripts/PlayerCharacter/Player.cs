using UnityEngine;

/* USAGE
    1 - Inherit from Player when creating a custom Player Class
 */

namespace PlayerCharacter
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Player : MonoBehaviour
    {
        [Header("Player ID Settings")]
        public int preferredID;
        [SerializeField] private int _ID;
        
        public int ID
        {
            get => _ID;
            set => _ID = value;
        }
        
        [Header("Movement Settings")]
        public float moveSpeed = 75f;
        public float airSpeed = 15f;

        private float _hMovement;
        private float _vMovement;
        private float _groundDrag = 6f;
        private float _airDrag = 1f;
        private Vector3 _moveDirection;

        private Transform _playerTransform;
        private Rigidbody _rb;

        [Header("Jump Settings")] 
        public float jumpForce = 8f;
        public float fallMultiplier = 3f;
        public float gravityForce = -9.81f;
        public float gravityScale = 1f;
        
        private float _playerHeight = 2f;
        private bool _isGrounded;
        
        [Header("Camera Settings")]
        [SerializeField] private float xSens = 180f;
        [SerializeField] private float ySens = 180f;

        private Camera _mainCamera;
        private float _mouseX;
        private float _mouseY;
        private float _multiplier = 0.01f;
        private float _xRotation;
        private float _yRotation;
        
        public void Awake()
        {
            if (preferredID < 0)
                preferredID = 0;
            
            InitializeMovementReferences();
            
            InitializeCameraReferences();
        }
        
        // Awake Functions
        // Grab Movement References
        private void InitializeMovementReferences()
        {
            _playerTransform = GetComponent<Transform>();
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.useGravity = false;
        }

        // Grab Camera References
        private void InitializeCameraReferences()
        {
            _mainCamera = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            MovementInput();
            ControlDrag();

            _isGrounded = Physics.Raycast(_playerTransform.position, Vector3.down, _playerHeight / 2 + 0.1f);

            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                Jump();
            }

            MouseInput();

            _mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerTransform.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
        
        private void FixedUpdate()
        {
            MovePlayer();
        }
        
        // Update Functions
        private void MovementInput()
        {
            _hMovement = Input.GetAxisRaw("Horizontal");
            _vMovement = Input.GetAxisRaw("Vertical");

            _moveDirection = _playerTransform.forward * _vMovement + _playerTransform.right * _hMovement;
        }
        
        private void MovePlayer()
        {
            if (_isGrounded)
                _rb.AddForce(_moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
            else
                _rb.AddForce(_moveDirection.normalized * airSpeed, ForceMode.Acceleration);
            
            Vector3 gravity = gravityForce * gravityScale * Vector3.up;
            
            if (_rb.velocity.y < 0)
                _rb.AddForce(gravity * fallMultiplier, ForceMode.Acceleration);
            else
                _rb.AddForce(gravity, ForceMode.Acceleration);
        }
        
        private void ControlDrag()
        {
            _rb.drag = _isGrounded ? _groundDrag : _airDrag;
        }

        private void Jump()
        {
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void MouseInput()
        {
            _mouseX = Input.GetAxisRaw("Mouse X");
            _mouseY = Input.GetAxisRaw("Mouse Y");

            _yRotation += _mouseX * xSens * _multiplier;
            _xRotation -= _mouseY * ySens * _multiplier;

            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        }
        
    }
}

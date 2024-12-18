using Managers;
using PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
       private PlayerControls _playerControls;
       private CharacterController _characterController;
       private Animator _animator;
       private PlayerCombat _playerCombat;
       
       private Vector2 _currentMovementInput;
       private Vector3 _currentMovement;
       
       private bool _isMovementPressed;
       [SerializeField]
       private float _rotationSpeed = 30f;
       
       public float movementSpeed = 5f;
       [SerializeField]
       private bool isShooting;

       private void Awake()
       {
           _playerControls = new PlayerControls();
           _characterController = GetComponent<CharacterController>();
           _animator = GetComponentInChildren<Animator>();
           _playerCombat = GetComponent<PlayerCombat>();
           AnimationManager.Instance.InitializeAnimator(_animator, "IsMoving");
       }

       public void Start()
       {
           
           _playerControls.Player.Movement.started +=  OnMovementInput;
           _playerControls.Player.Movement.canceled += OnMovementInput;
           _playerControls.Player.Movement.performed += OnMovementInput;
       }

       private void OnMovementInput(InputAction.CallbackContext ctx)
       {
           _currentMovementInput = ctx.ReadValue<Vector2>();
           _currentMovement.x = _currentMovementInput.x;
           _currentMovement.z = _currentMovementInput.y;
           _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
       }
       
       private void Update()
       {
           if (_playerCombat != null)
           {
               isShooting = _playerCombat.HasTarget();
           }

           if (isShooting && _isMovementPressed)
           {
               HandleCombatRotation();
           }
           else if (!isShooting && _isMovementPressed)
           {
               HandleMovementRotation();
           }
           HandleAnimation();
           HandleMovement();
       }

       private void HandleMovement()
       {
           Vector3 movement = _currentMovement;
           movement.y = -1f; // Gravity
           _characterController.Move(movement * (movementSpeed * Time.deltaTime));
       }
       
       private void HandleCombatRotation()
       {
           if(_playerCombat == null || !_playerCombat.HasTarget()) return;

           Vector3 direction = _playerCombat.GetTargetDirection();
           Quaternion targetRotation = Quaternion.LookRotation(direction);
           transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
       }

       private void HandleMovementRotation()
       {
           //if (!_isMovementPressed) return;
           
           Vector3 positionToLookAt;
           positionToLookAt.x = _currentMovement.x;
           positionToLookAt.y = 0;
           positionToLookAt.z = _currentMovement.z;
           
           if (positionToLookAt.sqrMagnitude > 0.01f) // Kiểm tra hướng hợp lệ
           {
               Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
               transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
           }
       }

       private void HandleAnimation()
       {
           AnimationManager.Instance.SetBool(_animator, "IsMoving", _isMovementPressed);
       }
       
       
       private void OnEnable()
       {
           _playerControls.Enable();
       }

       private void OnDisable()
       {
           _playerControls.Disable();
       }
    }
}

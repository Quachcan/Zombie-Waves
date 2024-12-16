using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
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

       private float _rotationSpeed = 15f;
        
       public float movementSpeed = 5f;

       private void Awake()
       {
           _playerControls = new PlayerControls();
           _characterController = GetComponent<CharacterController>();
           _animator = GetComponentInChildren<Animator>();
           
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
           HandleRotation();
           HandleAnimation();
           _characterController.Move(_currentMovement * (movementSpeed * Time.deltaTime));
       }

       private void HandleRotation()
       {
           Vector3 positionToLookAt;

           if (_playerCombat != null && _playerCombat.HasTarget())
           {
               positionToLookAt = _playerCombat.GetTargetDirection();
           }
           else if (_isMovementPressed)
           {
               positionToLookAt.x = _currentMovement.x;
               positionToLookAt.y = 0;
               positionToLookAt.z = _currentMovement.z;
           }
           else
           {
               return;
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

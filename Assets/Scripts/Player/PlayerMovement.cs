using System;
using Managers;
using Managers.Input_Manager;
using Managers.Player_Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
       private PlayerControls _playerControls;
       private CharacterController _characterController;
       private Animator _animator;
       
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
           
           positionToLookAt.x = _currentMovement.x;
           positionToLookAt.y = 0;
           positionToLookAt.z = _currentMovement.z;
           
           Quaternion currentRotation = transform.rotation;

           if (_isMovementPressed)
           {
               Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
               transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * _rotationSpeed);
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

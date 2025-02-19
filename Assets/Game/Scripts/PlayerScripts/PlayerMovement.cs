using Game.Scripts.Managers;
using Game.Scripts.PlayerScripts;
using Managers;
using PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
       private PlayerControls playerControls;
       private CharacterController characterController;
       private Animator animator;
       private PlayerCombat playerCombat;
       
       private Vector2 currentMovementInput;
       private Vector3 currentMovement;
       
       private bool isMovementPressed;
       [SerializeField]
       private float rotationSpeed = 30f;
       
       public float baseMovementSpeed = 5f;
       
       [SerializeField]
       private float currentMovementSpeed;
       [SerializeField]
       private bool isShooting;

       private void Awake()
       {
           playerControls = new PlayerControls();
           
           currentMovementSpeed = baseMovementSpeed;
       }

       public void Initialize()
       {
           characterController = GetComponent<CharacterController>();
           animator = GetComponentInChildren<Animator>();
           playerCombat = GetComponent<PlayerCombat>();
           
           Player.Instance.animationManager.InitializeAnimator(animator, "IsMoving");
           
           playerControls.Player.Movement.started +=  OnMovementInput;
           playerControls.Player.Movement.canceled += OnMovementInput;
           playerControls.Player.Movement.performed += OnMovementInput;
       }

       private void OnMovementInput(InputAction.CallbackContext ctx)
       {
           currentMovementInput = ctx.ReadValue<Vector2>();
           currentMovement.x = currentMovementInput.x;
           currentMovement.z = currentMovementInput.y;
           isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
       }
       
       private void Update()
       {
           if (playerCombat != null)
           {
               isShooting = playerCombat.HasTarget();
           }

           if (isShooting && isMovementPressed)
           {
               HandleCombatRotation();
           }
           else if (!isShooting && isMovementPressed)
           {
               HandleMovementRotation();
           }
           HandleAnimation();
           HandleMovement();
       }

       private void HandleMovement()
       {
           Vector3 movement = currentMovement;
           movement.y = -1f; // Gravity
           characterController.Move(movement * (currentMovementSpeed * Time.deltaTime));
       }
       
       private void HandleCombatRotation()
       {
           if(playerCombat == null || !playerCombat.HasTarget()) return;

           Vector3 direction = playerCombat.GetTargetDirection();
           Quaternion targetRotation = Quaternion.LookRotation(direction);
           transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
       }

       private void HandleMovementRotation()
       {
           //if (!_isMovementPressed) return;
           
           Vector3 positionToLookAt;
           positionToLookAt.x = currentMovement.x;
           positionToLookAt.y = 0;
           positionToLookAt.z = currentMovement.z;
           
           if (positionToLookAt.sqrMagnitude > 0.01f) // Kiểm tra hướng hợp lệ
           {
               Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
               transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
           }
       }

       private void HandleAnimation()
       {
           Player.Instance.animationManager.SetBool(animator, "IsMoving", isMovementPressed);
       }

       public void IncreaseMovementSpeed(float amount)
       {
           currentMovementSpeed += amount;
           Debug.Log("Movement speed Increased" + baseMovementSpeed);
       }
       
       private void OnEnable()
       {
           playerControls.Enable();
       }

       private void OnDisable()
       {
           playerControls.Disable();
       }
    }
}

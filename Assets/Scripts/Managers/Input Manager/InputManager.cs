using UnityEngine;

namespace Managers.Input_Manager
{
   public class InputManager : MonoBehaviour
   {
      private PlayerControls _playerControls;
   
      public float horizontalInput;
      public float verticalInput;
      public float moveAmount;
   
      private Vector2 _movementInput;

      private void OnEnable()
      {
         if (_playerControls == null)
         {
            _playerControls = new PlayerControls();

            _playerControls.Player.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _playerControls.Player.Movement.canceled += ctx => _movementInput = Vector2.zero;
         
         }
         _playerControls.Enable();
      }

      private void OnDisable()
      {
         _playerControls.Disable();
      }

      public void HandleAllInputs()
      {
         HandleMovementInput();
      }

      private void HandleMovementInput()
      {
         horizontalInput = _movementInput.x;
         verticalInput = _movementInput.y;
         moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
      }
   }
}

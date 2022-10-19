using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hyeur
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool shift_Input;
        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;

        PlayerController inputActions;
        Vector2 movementInput;
        Vector2 cameraInput;
        

        public void OnEnable() {
            if (inputActions == null) {
                inputActions = new PlayerController();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }
            inputActions.Enable();
        }
                
        private void OnDisable() {
            inputActions.Disable();
        }

        public void TickInput(float dt){
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
            MoveInput(dt);
            HandleRollInput(dt);
        }
        private void MoveInput(float dt){
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        }

        private void HandleRollInput(float dt){

            shift_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (shift_Input) {
                rollInputTimer += dt;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.25f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }
    }
    
}

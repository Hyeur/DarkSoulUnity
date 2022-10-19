using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hyeur
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;

        PlayerLocomotion playerLocomotion;
        Animator anim;
        CameraHandler cameraHandler;
        Vector2 cameraInput;

        [Header("Player Flags")]
        public bool isInteracting;
        public bool isSprinting;
        private void Awake(){
            cameraHandler = CameraHandler.inst;
            

        }
        void Start(){
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        void LateUpdate(){           
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            isSprinting = inputHandler.shift_Input;
        }

        private void Update(){
            
            float dt = Time.fixedDeltaTime;
            isInteracting = anim.GetBool("isInteracting");

            if (cameraHandler != null){
                cameraHandler.FollowTarget(dt);
                cameraHandler.HandleCameraRotation(dt,inputHandler.mouseX, inputHandler.mouseY);
            }

            
        }
        public void FixedUpdate(){
            float dt = Time.deltaTime;
            
            inputHandler.TickInput(dt);
            playerLocomotion.HandleRollingAndSprinting(dt);
            playerLocomotion.HandleMovement(dt);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Hyeur
{

    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;

        [HideInInspector]
        public Transform myTranform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;
        public new Rigidbody rigidbody;
        public GameObject normalCamera;
        
        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;


        

        void Start() {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
            cameraObject = Camera.main.transform;
            myTranform = transform;
            animatorHandler.Initialize();
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;
        public void HandleMovement(float dt){
            if (playerManager.isInteracting)
                return;
            if (inputHandler.rollFlag)
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y =0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
            }
            
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection,normalVector);
            rigidbody.velocity =projectedVelocity;
            
            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            if (animatorHandler.canRotate) {
                HandleRotation(dt);
            }
        }
        #endregion

        public void HandleRollingAndSprinting(float dt){

            if (animatorHandler.anim.GetBool("isInteracting")){
                return;
            }

            if(inputHandler.rollFlag){
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                moveDirection.Normalize();


                if(inputHandler.moveAmount > 0) {
                    animatorHandler.PlayTargetAnimation("StandToRoll_01",true);
                    moveDirection.y = 0;
                    Quaternion rollRotaion= Quaternion.LookRotation(moveDirection);
                    myTranform.rotation = rollRotaion;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("JumpBack_01",true);
                }
            }
        }
        private void HandleRotation(float dt){
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if(targetDir == Vector3.zero)
            {
                targetDir = myTranform.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTranform.rotation, tr, rs * dt);

            myTranform.rotation = targetRotation;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectFear.Input;
using System;
using ProjectFear.Animation;

namespace ProjectFear.Movement
{
    [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        private InputManager input = null;
        private PlayerAnimationController animController = null; 
        private Rigidbody rb = null;

        [SerializeField] private Transform cam = null;
        private GameObject baseCamera = null;
        private Vector3 moveDirection;

        [Header("Movement Variables")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 7.5f;

        private Vector3 normalVector;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<InputManager>();
            animController = GetComponentInChildren<PlayerAnimationController>(); // TODO write a helper function to just Get component in current and children
            if (cam == null)
                cam = Camera.main.transform;
        }

        private void Start()
        {
            animController.Init();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            input.Tick(deltaTime);

            HandleMovement(deltaTime);
            HandleRoll();
        }

        public void SetDrag(float value) => rb.drag = value;
        public void SetVelocity(Vector3 velocity) => rb.velocity = velocity;
        public void HandleMovement(float deltaTime)
        {
            SetBaseMoveDirection();
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = moveSpeed;
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rb.velocity = projectedVelocity;

            animController.UpdateAnimatorValues(input.MoveAmount, 0);

            if (animController.CanRotate)
                HandleRotation(deltaTime);
        }

        public void HandleRoll()
        {
            if (animController.Anim.GetBool("IsInteracting")) // this is very bad, dont rely on the animator for this stuff
                return;
            if (input.GetActionFlagValue(ActionFlagType.Roll))
            {
                if (input.MoveAmount > 0)
                    animController.PlayAnimation("standing_roll", true); //TODO port this out of movement controller
            }
        }

        private void HandleRotation(float deltaTime)
        {
            Vector3 targetDir;
            float moveOverride = input.MoveAmount;

            targetDir = cam.forward * input.Vertical;
            targetDir += cam.right * input.Horziontal;
            targetDir.Normalize();

            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            float currentRotSpeed = rotationSpeed;

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            Quaternion smoothedTargetRotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotSpeed * deltaTime);

            transform.rotation = smoothedTargetRotation;
        }

        private void SetBaseMoveDirection()
        {
            moveDirection = cam.forward * input.Vertical;
            moveDirection += cam.right * input.Horziontal;
        }

    }
}
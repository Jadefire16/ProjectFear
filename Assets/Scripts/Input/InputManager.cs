using ProjectFear.Cameras;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectFear.Input
{
    public class InputManager : MonoBehaviour
    {
        private float horziontal = 0;
        private float vertical = 0;
        private float moveAmount = 0;
        private float mouseX = 0;
        private float mouseY = 0;
        private Vector2 movementInput;
        private Vector2 cameraInput;

        private CameraController camController;
        private StandardControls inputActions = null;
        private StandardControls InputActions
        {
            get
            {
                if(inputActions == null)
                    inputActions = new StandardControls();
                return inputActions;
            }
            set
            {
                inputActions = value;
            }
        }
        public float Horziontal { get => horziontal; }
        public float Vertical { get => vertical; }
        public float MoveAmount { get => moveAmount; }
        public float MouseX { get => mouseX; }
        public float MouseY { get => mouseY; }

        public void Tick(float deltaTime) => MoveInput(deltaTime);

        private void Start()
        {
            if(inputActions == null)
                inputActions = new StandardControls();
            camController = CameraController.Instance;
        }

        private void FixedUpdate()
        {
            var fixedDeltaTime = Time.fixedDeltaTime;

            if(camController != null)
            {
                camController.FollowTarget(fixedDeltaTime);
                camController.ProcessCameraRotation(mouseX, mouseY, fixedDeltaTime);
            }
        }

        private void OnEnable()
        {
            InputActions.PlayerMovementStandard.Movement.performed += InputActions => movementInput = InputActions.ReadValue<Vector2>();
            InputActions.PlayerMovementStandard.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }

        private void MoveInput(float deltaTime)
        {
            horziontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horziontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

    }
}
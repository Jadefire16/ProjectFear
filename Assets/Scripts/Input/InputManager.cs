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
        private bool rollInput;
        private bool isInteracting;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private static readonly Dictionary<ActionFlagType, bool> actionFlags = new Dictionary<ActionFlagType, bool>
        {
            {ActionFlagType.Roll, false },
            {ActionFlagType.LightAttack, false },
            {ActionFlagType.HeavyAttack, false }
        };

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
        public bool RollInput { get => rollInput; set => rollInput = value; }
        public bool IsInteracting { get => isInteracting; set => isInteracting = value; }

        public void Tick(float deltaTime)
        {
            MoveInput(deltaTime);
            HandleFlags(deltaTime);
        }

        public bool GetActionFlagValue(ActionFlagType type)
        {
            if (!actionFlags.ContainsKey(type))
                throw new System.Exception("Invalid Action Type");
            return actionFlags[type];
        }

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

        private void LateUpdate()
        {
            SetActionFlag(ActionFlagType.Roll, false);
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

        private void HandleFlags(float deltaTime)
        {
            rollInput = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            if (rollInput) // if we're rolling keep on rolling
                SetActionFlag(ActionFlagType.Roll, true);
        }

        private void SetActionFlag(ActionFlagType flagType, bool value)
        {
            if (!actionFlags.ContainsKey(flagType))
                return;
            actionFlags[flagType] = value;
        }

    }
}

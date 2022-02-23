using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectFear.Cameras
{
    public class CameraController : MonoBehaviour
    {
        private static CameraController instance;
        public static CameraController Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<CameraController>();
                return instance;
            }
        }
        private void Awake()
        {
#if UNITY_EDITOR
            Debug.Assert(instance == null);
#endif
            instance = this;

            defaultDepth = camTransform.localPosition.z;
            ignoreMask = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        [Header("Transforms")]
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform camTransform;
        [SerializeField] private Transform camPivot;
        [Header("Controls")]
        [SerializeField] private float lookSpeed = 0.1f;
        [SerializeField] private float followSpeed = 0.1f;
        [SerializeField] private float pivotSpeed = 0.03f;
        [Header("Collision")]
        [SerializeField] private float collisionRadius = 0.2f;
        [SerializeField] private float collisionOffset = 0.2f;
        [SerializeField] private float minimumCollisionOffset = 0.2f;
        [Header("Ranges")]
        [SerializeField] private float minimumPivotAngle = -35f;
        [SerializeField] private float maximumPivotAngle = 35f;

        private Vector3 previousCameraVelocity;
        private Vector3 camTransformPos;
        private LayerMask ignoreMask;

        private float targetDepth;
        private float defaultDepth;
        private float lookAngle;
        private float pivotAngle;

        public void FollowTarget(float deltaTime)
        {
            var targetPos = Vector3.SmoothDamp(transform.position, targetTransform.position, ref previousCameraVelocity, deltaTime / followSpeed);
            transform.position = targetPos;
            ProcessCameraCollisions(Time.fixedDeltaTime);
        }

        public void ProcessCameraRotation(float x, float y, float fixedDeltaTime)
        {
            lookAngle += (x * lookSpeed) / fixedDeltaTime;
            pivotAngle -= (y * pivotSpeed) / fixedDeltaTime;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

            var eulerRotation = Vector3.zero;
            eulerRotation.y = lookAngle;
            var targetRotation = Quaternion.Euler(eulerRotation);
            transform.rotation = targetRotation;

            eulerRotation = Vector3.zero;
            eulerRotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(eulerRotation);
            camPivot.localRotation = targetRotation;
        }

        private void ProcessCameraCollisions(float deltaTime)
        {
            targetDepth = defaultDepth;
            Vector3 normalizedDirection = camTransform.position - camPivot.position; // get normalized direction
            normalizedDirection.Normalize();

            if (Physics.SphereCast(camPivot.position, collisionRadius, normalizedDirection, out RaycastHit hit, Mathf.Abs(targetDepth), ignoreMask)) // detect collisions
            {
                float dist = Vector3.Distance(camPivot.position, hit.point);
                targetDepth = -(dist - collisionOffset);
            }

            if(Mathf.Abs(targetDepth) < minimumCollisionOffset) // ensure the camera is a suitable distance away
                targetDepth = -minimumCollisionOffset;

            camTransformPos.z = Mathf.Lerp(camTransform.localPosition.z, targetDepth, deltaTime / 0.2f); // assign value, should probably smooth damp?
            camTransform.localPosition = camTransformPos;
        }
    }
}
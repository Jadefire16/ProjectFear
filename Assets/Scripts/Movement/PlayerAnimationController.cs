using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectFear.Animation
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private float dampTime = 0.1f;
        [SerializeField] private bool canRotate = false;
        private int verticalHash = 0, horizontalHash = 0;

        public bool CanRotate { get => canRotate; }

        public void Init()
        {
            anim = GetComponent<Animator>();
            verticalHash = Animator.StringToHash("Vertical");
            horizontalHash = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float vertical, float horizontal)
        {
            float v = 0;  // TODO find a way to clamp this shit
            if (vertical > 0 && vertical < 0.55f)
                v = 0.5f;
            else if (vertical > 0.55f)
                v = 1;
            else if (vertical < 0 && vertical > -0.55f)
                v = -0.5f;
            else if (vertical < -0.55f)
                v = -1;
            else
                v = 0;

            float h = 0;
            if (horizontal > 0 && horizontal < 0.55f)
                h = 0.5f;
            else if (horizontal > 0.55f)
                h = 1;
            else if (horizontal < 0 && horizontal > -0.55f)
                h = -0.5f;
            else if (horizontal < -0.55f)
                h = -1;
            else
                h = 0;
            anim.SetFloat(verticalHash, v, dampTime, Time.deltaTime);
            anim.SetFloat(horizontalHash, h, dampTime, Time.deltaTime);
        }

        public void AllowRotation() => canRotate = true;
        public void StopRotation() => canRotate = false;
    }
}
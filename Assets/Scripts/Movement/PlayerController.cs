using ProjectFear.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Animator anim;
    private InputManager input;

    private int interactingHash;

    private void Start()
    {
        input = GetComponent<InputManager>();
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        interactingHash = Animator.StringToHash("IsInteracting");
    }

    private void Update()
    {
        input.IsInteracting = anim.GetBool(interactingHash); // TODO again decouple this, this is bad
        input.RollFlag = false;
    }
}

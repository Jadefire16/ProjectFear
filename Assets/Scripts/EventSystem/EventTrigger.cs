using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventTrigger : MonoBehaviour
{
    [SerializeField] protected UnityEvent trigger;


    [SerializeField] protected bool triggerOnStart = false;
    [Header("Delay")]
    [SerializeField, Tooltip("Very Important: Must be active to use Delayed Functions")] protected bool delayTrigger = false;
    [SerializeField] protected float delayTime = 1f;


    protected virtual void Start()
    {
        if (delayTrigger)
            StartCoroutine(TriggerDelay());
        else if (triggerOnStart)
            FireTrigger();
    }
    public virtual void FireTrigger() => trigger?.Invoke();

    protected virtual IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(delayTime);
        FireTrigger();
        Debug.Log("Fired");

    }

}

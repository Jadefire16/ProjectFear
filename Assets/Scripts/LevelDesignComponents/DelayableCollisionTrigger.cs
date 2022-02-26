using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DelayableCollisionTrigger : EventTrigger
{
    [Tooltip("What can interact with this trigger?")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private bool resetTriggerAfterFire = false;

    private Collider col;
    private bool active = true;

    protected override void Start()
    {
        if (!triggerOnStart)
            return;
        else if (delayTrigger)
            StartCoroutine(TriggerDelay());
        else
            FireTrigger();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;
        if(((1 << other.gameObject.layer) & hitMask) != 0)
        {
            if (delayTrigger)
                FireTriggerDelayed();
            else
                FireTrigger();

            if (!resetTriggerAfterFire)
            {
                active = false;
                col.enabled = false;
            }
        }
    }

    public override void FireTrigger() => trigger?.Invoke();
    [ContextMenu("Test Trigger")]
    public void FireTriggerDelayed()
    {
        if (delayTrigger)
            StartCoroutine(TriggerDelay());
        else
            FireTrigger();
    }
    protected override IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(delayTime);
        FireTrigger();
    }
}

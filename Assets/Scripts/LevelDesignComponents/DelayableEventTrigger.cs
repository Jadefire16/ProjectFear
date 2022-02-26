using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayableEventTrigger : EventTrigger
{
    protected override void Start()
    {
        if (!triggerOnStart)
            return;
        else if (delayTrigger)
            StartCoroutine(TriggerDelay());
        else
            FireTrigger();
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

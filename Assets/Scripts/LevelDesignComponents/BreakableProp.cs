using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class BreakableProp : BreakableObject
{
    [SerializeField] private float destroyObjectDelay = 1f;
    [SerializeField] private UnityEvent onBreak;

    private bool active = true;
    private Collider col;
    private MeshRenderer rend;

    private void Start()
    {
        currentHealth = initialHealth;
        col = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
    }

    [ContextMenu("Break Object")]
    public override void BreakObject()
    {
        col.enabled = false;
        rend.enabled = false;
        active = false;
        onBreak?.Invoke();
        StartCoroutine(DestroyDelay());
        Debug.Log("Destroyed");
    }

    public override void TakeDamage(float value)
    {
        if (!active)
            return;
        currentHealth -= value;
        HealthCheck();
    }

    public override void TakeDamage(float value, RaycastHit? hit = null)
    {
        if (!active)
            return;
        TakeDamage(value);
    }

    private void HealthCheck()
    {
        if(currentHealth <= 0)
            BreakObject();
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(destroyObjectDelay);
        Destroy(gameObject);
    }
}

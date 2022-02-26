using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BreakableObject : MonoBehaviour, IDamageable
{
    [SerializeField] protected float initialHealth = 2;

    protected float currentHealth;

    public abstract void BreakObject();
    public abstract void TakeDamage(float value);
    public abstract void TakeDamage(float value, RaycastHit? hit = null);
}

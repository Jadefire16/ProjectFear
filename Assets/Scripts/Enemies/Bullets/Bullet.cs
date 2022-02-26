using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private LayerMask hitMask;
    private Vector3 initialPos;
    private float damageOnImpact;
    private float speed;
    private float maxDistance;

    public void Initialize(Vector3 spawnPos, Vector3 bulletDirection, LayerMask hitMask, float damage, float speed, float maxDist)
    {
        this.hitMask = hitMask;
        this.initialPos = spawnPos;
        transform.position = spawnPos;
        this.damageOnImpact = damage;
        this.speed = speed;
        this.maxDistance = maxDist;
        transform.forward = bulletDirection;
    }

    private void FixedUpdate()
    {
        if (Physics.SphereCast(transform.position, 0.25f, transform.forward, out RaycastHit info, speed * Time.fixedDeltaTime, hitMask, QueryTriggerInteraction.Ignore))
        {
            info.collider.TryGetComponent<IDamageable>(out IDamageable damageable);
            if (damageable != null)
                damageable.TakeDamage(damageOnImpact, info);
            Destroy(gameObject);
            return;
        }
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
        if (Vector3.Distance(transform.position, initialPos) > maxDistance)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward);
    }

}

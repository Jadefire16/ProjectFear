using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleFlower : MonoBehaviour, IDamageable // you can definitely move this to a state machine later and recycle this for the controller
{

    [SerializeField] private float initialHealth = 5;
    [SerializeField] private LayerMask hitMask;

    [Header("Ranged Attack")]
    [SerializeField] private GameObject spitBullet;
    [SerializeField] private float spitAttackRange = 5f;
    [SerializeField] private float spitAttackDamage = 1f;

    [Header("Close Attack")]
    [SerializeField] private float whipAttackRadius = 3f;
    [SerializeField] private float whipDamage = 1f;

    private float currentHealth;

    private void Start()
    {
        currentHealth = initialHealth;
    }

    private void Update()
    {
        
    }

    [ContextMenu("Shoot")]
    private void SpitAttack()
    {
        Bullet bullet = Instantiate(spitBullet, transform.forward, Quaternion.identity).GetComponent<Bullet>();
        bullet.Initialize(transform.position, transform.forward, hitMask, spitAttackDamage, 2f, spitAttackRange);
    }

    private void WhipAttack()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, whipAttackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0,0,(int)(spitAttackRange * 0.5f)), new Vector3(0.25f, 0.25f, spitAttackRange));
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
            KillPlant();
    }

    public void TakeDamage(float value, RaycastHit? hit = null)
    {
        TakeDamage(value); // hit it unused atm here till I get some particle effects or something
    }

    private void KillPlant()
    {
        Destroy(gameObject);
    }
}
//if (!Physics.SphereCast(transform.position, 0.25f, transform.forward, out RaycastHit info, spitAttackRange, hitMask, QueryTriggerInteraction.Ignore))
//    return;
//info.collider.TryGetComponent<IDamageable>(out IDamageable damageable);
//if (damageable == null)
//    return;
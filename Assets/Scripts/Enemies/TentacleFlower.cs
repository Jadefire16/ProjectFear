using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleFlower : MonoBehaviour // you can definitely move this to a state machine later and recycle this for the controller
{

    [SerializeField] private float spitAttackRange = 6;
    [SerializeField] private float whipAttackRadius = 3;


    private void Start()
    {
        
    }


    private void SpitAttack()
    {

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

}

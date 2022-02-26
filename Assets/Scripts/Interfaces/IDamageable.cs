using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float value);
    void TakeDamage(float value, RaycastHit? hit = null);
}

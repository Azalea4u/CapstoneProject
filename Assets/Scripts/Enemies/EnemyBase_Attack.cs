using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase_Attack : MonoBehaviour
{
    public int attackDamage = 1;
    Collider2D attackCollider;
    IDamageable damageable;

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // See if it can be hit
        damageable = collision.GetComponent<IDamageable>();

        if (damageable != null && collision.gameObject.CompareTag("Player"))
        {
            // Hit the Target
            bool gotHit = damageable.TakeDamage(attackDamage);

            if (gotHit)
                Debug.Log(collision.name + " receives " + attackDamage + " damage");
        }
    }
}

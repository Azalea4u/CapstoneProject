using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int MaxHealth { get; set; }
    int Health { get; set; }
    bool IsAlive { get; set; }

    void TakeDamage(int damage);
    void Death();
}
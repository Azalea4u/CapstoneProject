using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    int AttackDamage { get; }
    float AttackCooldown { get; }
    float AttackRange { get; }
    Collider2D AttackPoint { get; }
    bool CanAttack { get; set; }

    void Attack();
    void OnAttackPointTriggered(Collider2D collision);
}
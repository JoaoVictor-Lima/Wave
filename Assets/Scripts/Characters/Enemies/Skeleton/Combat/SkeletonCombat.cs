using Assets.Scripts.Entities.Enemies.Core.Combat;
using UnityEngine;

public class SkeletonCombat : EnemyCombat
{
    [Header("References")]
    [SerializeField] private Animator animator;

    protected override void ExecuteAttack(EnemyAttackData attack, Transform target)
    {
        transform.LookAt(target);

        Debug.Log($"Skeleton attacks {target.name} with {attack.attackName}");

        if (attack.AnimationTriggerName != null)
            animator.SetTrigger(attack.AnimationTriggerName);
    }

    protected override EnemyAttackData SelectAttack(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);

        foreach (var attack in attacks)
        {
            if (distance <= attack.range && IsAttackReady(attack))
            {
                return attack;
            }
        }

        return null;
    }
}

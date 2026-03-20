using Assets.Scripts.Entities.Enemies.Core.Combat;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombat : BaseCombat
{
    [SerializeField] protected List<EnemyAttackData> attacks;

    private Dictionary<EnemyAttackData, float> _cooldowns =
        new Dictionary<EnemyAttackData, float>();

    protected virtual void Awake()
    {
        foreach (var attack in attacks)
            _cooldowns[attack] = 0f;
    }

    protected virtual void Update()
    {
        var keys = new List<EnemyAttackData>(_cooldowns.Keys);
        foreach (var attack in keys)
        {
            if (_cooldowns[attack] > 0f)
                _cooldowns[attack] -= Time.deltaTime;
        }
    }

    public void TryAttack(Transform target)
    {
        if (IsAttacking)
            return;

        var attack = SelectAttack(target);
        if (attack == null)
            return;

        if (_cooldowns[attack] > 0f)
            return;

        IsAttacking = true;
        ExecuteAttack(attack, target);
        _cooldowns[attack] = attack.cooldown;
    }

    protected bool IsAttackReady(EnemyAttackData attack)
    {
        return _cooldowns[attack] <= 0f;
    }

    public bool CanAttackTarget(Transform target)
    {
        return SelectAttack(target) != null;
    }

    protected abstract EnemyAttackData SelectAttack(Transform target);

    protected abstract void ExecuteAttack(EnemyAttackData attack, Transform target);
}

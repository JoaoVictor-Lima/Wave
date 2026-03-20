using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private EnemyCombat _combat;
    private Transform _target;
    private IMovementAgent _movement;

    public EnemyAttackState(EnemyBrain brain) : base(brain) { }

    public override void Enter()
    {
        Debug.Log($"{Brain.name} entrou em Attack");

        _combat = Brain.Combat;
        _movement = Brain.Movement;
        _target = Brain.CurrentTarget;

        _movement.SetSpeedMultiplier(0.1f);
    }

    public override void Update()
    {
        if (_target == null)
        {
            Brain.ChangeState<EnemyIdleState>();
            return;
        }

        if (_combat.IsAttacking)
        {
            return;
        }

        float distance = Vector3.Distance(
            Brain.transform.position,
            _target.position
        );

        if (!Brain.Combat.CanAttackTarget(Brain.CurrentTarget))
        {
            Brain.ChangeState<EnemyChaseState>();
            return;
        }

        _combat.TryAttack(_target);
    }

    public override void Exit()
    {
        _movement.SetSpeedMultiplier(1f);
    }
}

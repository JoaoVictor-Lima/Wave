using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyBrain brain) : base(brain) { }

    public override void Enter()
    {
        Debug.Log($"{Brain.name} entrou em Chase");
    }

    public override void Update()
    {
        if (Brain.CurrentTarget == null)
        {
            Brain.Movement?.Stop();
            Brain.ChangeState<EnemyIdleState>();
            return;
        }

        float distance = Vector3.Distance(
            Brain.transform.position,
            Brain.CurrentTarget.position
        );

        if (Brain.Combat.CanAttackTarget(Brain.CurrentTarget))
        {
            Brain.ChangeState<EnemyAttackState>();
            return;
        }

        Brain.Movement?.MoveTo(Brain.CurrentTarget.position);
    }
}

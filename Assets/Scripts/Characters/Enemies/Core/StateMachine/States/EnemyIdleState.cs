using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyBrain brain) : base(brain) { }

    public override void Enter()
    {
        Debug.Log($"{Brain.name} entrou em Idle");
    }

    public override void Update()
    {
        // Por enquanto, não faz nada
    }
}

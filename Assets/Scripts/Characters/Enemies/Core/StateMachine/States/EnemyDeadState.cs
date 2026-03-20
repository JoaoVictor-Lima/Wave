using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyBrain brain) : base(brain) { }

    public override void Enter()
    {
        Debug.Log($"{Brain.name} entrou em Dead");

        Brain.Movement?.Stop();

        if (Brain.Movement is MonoBehaviour mb)
            mb.enabled = false;

        Brain.Combat.enabled = false;
        Brain.Perception.enabled = false;

        var animator = Brain.GetComponentInChildren<Animator>();
        animator?.SetTrigger("Die");

        var collider = Brain.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
    }

    public override void Update() { }
}

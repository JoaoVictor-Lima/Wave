using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    protected EnemyStateMachine StateMachine;
    public EnemyPerception Perception;
    public EnemyCombat Combat;
    public Health Health;

    public Transform CurrentTarget { get; private set; }
    public IMovementAgent Movement { get; private set; }


    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine();
        Perception = GetComponent<EnemyPerception>();
        Movement = GetComponent<IMovementAgent>();
        Combat = GetComponent<EnemyCombat>();
        Health = GetComponent<Health>();

        RegisterStates();
    }

    protected virtual void Start()
    {
        if (Perception != null)
        {
            Perception.OnTargetDetected += SetTarget;
            Perception.OnTargetLost += ClearTarget;
        }

        if (Health != null)
        {
            Health.OnDeath += OnDeath;
        }

        StateMachine.ChangeState<EnemyIdleState>();
    }

    protected virtual void Update()
    {
        StateMachine.Update();
    }

    private void OnDeath()
    {
        StateMachine.ChangeState<EnemyDeadState>();
    }

    void SetTarget(Transform target)
    {
        CurrentTarget = target;
        ChangeState<EnemyChaseState>();
    }

    void ClearTarget()
    {
        CurrentTarget = null;
        ChangeState<EnemyIdleState>();
    }

    public void ChangeState<T>() where T : EnemyState
    {
        StateMachine.ChangeState<T>();
    }


    protected virtual void RegisterStates()
    {
        StateMachine.AddState(new EnemyIdleState(this));
        StateMachine.AddState(new EnemyChaseState(this));
        StateMachine.AddState(new EnemyDeadState(this));
        StateMachine.AddState(new EnemyAttackState(this));
    }
}

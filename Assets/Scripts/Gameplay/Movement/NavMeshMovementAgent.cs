using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovementAgent : MonoBehaviour, IMovementAgent
{
    private NavMeshAgent _agent;

    [Header("Animation Settings")]

    [Tooltip("Check if the GameObject has animations.")]
    public bool HasAnimator = false;

    [Tooltip("Animator to update. If left null, it will automatically use the Animator on this GameObject.")]
    public Animator Animator;

    private float _speed;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (HasAnimator && Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        _speed = _agent.speed;
    }

    private void Update()
    {
        if (HasAnimator && Animator != null)
        {
            float speed = _agent.velocity.magnitude;
            Animator.SetFloat("Speed", speed);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (!_agent.enabled)
            return;

        _agent.isStopped = false;
        _agent.SetDestination(destination);
    }

    public void Stop()
    {
        _agent.isStopped = true;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        _agent.speed = _speed * multiplier;
    }

    public bool HasReachedDestination()
    {
        return !_agent.pathPending &&
               _agent.remainingDistance <= _agent.stoppingDistance;
    }
}

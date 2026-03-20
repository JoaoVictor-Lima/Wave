using System;
using UnityEngine;

public class SmellSensor : MonoBehaviour, IEnemySensor
{
    [Header("Config")]
    public bool enabledSensor = true;
    public float range = 3f;
    public LayerMask targetMask;

    [Header("Debug")]
    public bool showGizmos = true;

    public event Action<Transform> OnTargetDetected;
    public event Action OnTargetLost;

    private Transform currentTarget;

    public void Tick()
    {
        if (!enabledSensor)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, range, targetMask);

        if (hits.Length > 0)
        {
            Detect(hits[0].transform);
            return;
        }

        Lose();
    }

    void Detect(Transform target)
    {
        if (currentTarget == target)
            return;

        currentTarget = target;
        OnTargetDetected?.Invoke(target);
    }

    void Lose()
    {
        if (currentTarget == null)
            return;

        currentTarget = null;
        OnTargetLost?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

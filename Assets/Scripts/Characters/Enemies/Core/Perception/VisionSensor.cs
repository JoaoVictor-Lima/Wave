using System;
using UnityEngine;

public class VisionSensor : MonoBehaviour, IEnemySensor
{
    [Header("Config")]
    public bool enabledSensor = true;
    public float range = 10f;
    [Range(0, 180)] public float angle = 90f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Thresholds")]
    [Range(0f, 1f)]
    public float visibilityThreshold = 0.85f;

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

        foreach (var hit in hits)
        {
            Vector3 dir = hit.transform.position - transform.position;
            float distance = dir.magnitude;
            dir.Normalize();

            float angleToTarget = Vector3.Angle(transform.forward, dir);

            if (angleToTarget > angle * 0.5f)
                continue;

            float distanceRatio = distance / range;

            if (distanceRatio > visibilityThreshold)
                continue;

            if (!Physics.Raycast(transform.position, dir, distance, obstacleMask))
            {
                Detect(hit.transform);
                return;
            }
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range * visibilityThreshold);

        Vector3 left = Quaternion.Euler(0, -angle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, angle / 2, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + left * range);
        Gizmos.DrawLine(transform.position, transform.position + right * range);
    }
}

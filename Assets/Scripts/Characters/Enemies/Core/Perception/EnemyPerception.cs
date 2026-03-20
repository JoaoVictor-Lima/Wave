using System;
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    public VisionSensor vision;
    public HearingSensor hearing;
    public SmellSensor smell;

    public event Action<Transform> OnTargetDetected;
    public event Action OnTargetLost;

    private void Awake()
    {
        Subscribe(vision);
        Subscribe(hearing);
        Subscribe(smell);
    }

    private void Update()
    {
        vision?.Tick();
        hearing?.Tick();
        smell?.Tick();
    }

    void Subscribe(IEnemySensor sensor)
    {
        if (sensor == null)
            return;

        sensor.OnTargetDetected += t => OnTargetDetected?.Invoke(t);
        sensor.OnTargetLost += () => OnTargetLost?.Invoke();
    }
}

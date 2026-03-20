using System;
using UnityEngine;

public interface IEnemySensor
{
    event Action<Transform> OnTargetDetected;
    event Action OnTargetLost;

    void Tick();
}

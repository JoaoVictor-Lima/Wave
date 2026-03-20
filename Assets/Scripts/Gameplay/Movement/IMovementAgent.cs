using UnityEngine;

public interface IMovementAgent
{
    void MoveTo(Vector3 destination);
    void Stop();
    void SetSpeedMultiplier(float multiplier);
    bool HasReachedDestination();
}

using UnityEngine;

public interface IEnemyAttack
{
    float Range { get; }
    float Cooldown { get; }

    bool CanAttack(Transform target);
    void Execute(Transform target);
}

using UnityEngine;

namespace Assets.Scripts.Entities.Enemies.Core.Combat
{
    [CreateAssetMenu(menuName = "Combat/Enemy Attack")]
    public class EnemyAttackData : ScriptableObject
    {
        public string attackName;
        public float range = 2f;
        public float cooldown = 1f;
        public string AnimationTriggerName;
        //public AttackType type;
    }
}

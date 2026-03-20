using Assets.Scripts.Gameplay.Interact;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies.Core.Combat
{
    public abstract class BaseCombat : MonoBehaviour
    {
        [SerializeField] public DamageDealer damageDealer;
        private float pendingDamage = 20;
        public bool IsAttacking { get; set; }

        public void PrepareAttack(float damage)
        {
            pendingDamage = damage;
        }

        public void EnableDamage()
        {
            //TODO: Implement capabilities for attacks
            var capabilities = new Capability[] { Capability.Woodcutting, Capability.Slashing };
            damageDealer.Arm(pendingDamage, capabilities, gameObject);
        }

        public void DisableDamage()
        {
            damageDealer.Disarm();
        }

        public void OnAttackFinished()
        {
            IsAttacking = false;
        }
    }
}

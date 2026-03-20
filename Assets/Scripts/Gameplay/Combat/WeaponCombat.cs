using UnityEngine;

public class WeaponCombat : MonoBehaviour
{
    [SerializeField] private DamageDealer damageDealer;

    private void Awake()
    {
        if (damageDealer == null)
        {
            damageDealer = GetComponentInChildren<DamageDealer>();
        }
    }
}

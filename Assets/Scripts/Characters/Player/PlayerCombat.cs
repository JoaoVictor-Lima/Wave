using Assets.Scripts.Entities.Enemies.Core.Combat;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : BaseCombat
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.6f;

    private bool canAttack = true;
    [SerializeField] private WeaponCombat currentWeaponCombat;

    private PlayerControls controls;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        controls.Player.Attack.performed -= OnAttack;
        controls.Player.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Attack();
    }

    public void SetWeapon(WeaponCombat weapon)
    {
        if (weapon == null)
        {
            damageDealer = null;
            currentWeaponCombat = null;
            return;
        }

        damageDealer = weapon.GetComponent<DamageDealer>();
        currentWeaponCombat = weapon;
    }

    public void Attack()
    {
        if (!canAttack || currentWeaponCombat == null)
            return;

        canAttack = false;
        PrepareAttack(30);
        animator.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}

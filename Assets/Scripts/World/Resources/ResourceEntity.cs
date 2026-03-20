using Assets.Scripts.Gameplay.Interact;
using Assets.Scripts.Gameplay.Loot;
using UnityEngine;

public class ResourceEntity : Entity, IInteractable
{
    [SerializeField] private LootTable lootTable;
    private Health health;
    private bool isHarvested = false;

    protected override void Awake()
    {
        base.Awake();

        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDeath += OnResourceDestroyed;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath -= OnResourceDestroyed;
        }
    }

    public void Interact(InteractionContext context)
    {
        bool isWoodcutting = false;

        for (int i = 0; i < context.Capabilities.Length; i++)
        {
            if (context.Capabilities[i] == Capability.Woodcutting)
            {
                isWoodcutting = true;
                break;
            }
        }

        if (isWoodcutting)
        {
            health.TakeDamage(context.Power);
            Debug.Log($"Árvore atingida! Vida restante: {health}");
        }
        else
        {
            Debug.Log("Este item não serve para cortar madeira.");
        }
    }

    private void OnResourceDestroyed()
    {
        if (isHarvested) return;
        isHarvested = true;

        var itemsToDrop = LootCalculator.GenerateLoot(lootTable.entries);
        LootSpawner.SpawnPhysicalLoot(itemsToDrop, transform.position);

        Destroy(gameObject);
    }
}

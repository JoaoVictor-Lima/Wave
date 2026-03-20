using Assets.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner
{
    public static void SpawnPhysicalLoot(List<ItemInstance> items, Vector3 position)
    {
        foreach (var instance in items)
        {
            GameObject obj = ItemAssembly.Assemble(instance);
            if (obj == null) continue;

            obj.transform.position = position + Random.insideUnitSphere * 0.3f;

            if (obj.TryGetComponent<Rigidbody>(out var rb))
            {
                Vector3 force = Vector3.up * Random.Range(2f, 4f) + Random.insideUnitSphere * 1.5f;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}

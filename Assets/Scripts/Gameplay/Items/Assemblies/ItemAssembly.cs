using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.InstancedModules;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.World;
using Assets.Scripts.World.Items.Assemblies;
using UnityEngine;

public static class ItemAssembly
{
    public static GameObject Assemble(ItemInstance instance, Transform parent = null, Quaternion? rotation = null)
    {
        var weaponParts = instance.GetInstanceModule<WeaponPartsInstanceModule>();

        if (weaponParts != null)
        {
            return WeaponAssembly.AssembleWeapon(instance, parent, rotation);
        }

        return AssembleSimpleItem(instance, parent, rotation);
    }

    private static GameObject AssembleSimpleItem(ItemInstance instance, Transform parent = null, Quaternion? rotation = null)
    {
        var worldPrefab = instance.ItemData.GetModule<WorldVisualItemModule>()?.WorldPrefab;

        if (worldPrefab == null)
        {
            Debug.LogWarning($"Item '{instance.ItemData.ItemName}' has no world visual.");
            return null;
        }

        GameObject obj = Object.Instantiate(worldPrefab);
        obj.name = instance.ItemData.ItemName;

        var entity = obj.GetComponent<ItemEntity>();

        if (entity != null)
            entity.Initialize(instance);

        return obj;
    }
}
using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.InstancedModules;
using Assets.Scripts.Gameplay.Items.Modules;
using UnityEngine;

namespace Assets.Scripts.World.Items.Assemblies
{
    public static class WeaponAssembly
    {
        public static GameObject AssembleWeapon(ItemInstance weaponInstance, Transform parent = null, Quaternion? rotation = null)
        {
            var worldPrefab = weaponInstance.ItemData
                .GetModule<WorldVisualItemModule>()?.WorldPrefab;

            if (worldPrefab == null)
            {
                Debug.LogWarning($"Item '{weaponInstance.ItemData.ItemName}' cannot be created in the world because it has no WorldVisual module.");
                return null;
            }

            Quaternion spawnRotation = rotation ?? Quaternion.identity;

            GameObject weapon;

            if (parent != null)
            {
                weapon = Object.Instantiate(worldPrefab, parent);
                weapon.transform.localRotation = spawnRotation;
            }
            else
            {
                weapon = Object.Instantiate(worldPrefab, Vector3.zero, spawnRotation);
            }

            weapon.name = weaponInstance.ItemData.ItemName;

            ItemEntity entity = weapon.GetComponent<ItemEntity>();

            if (entity != null)
            {
                entity.Initialize(weaponInstance);
            }
            else
            {
                Debug.LogError("Weapon prefab não possui ItemEntity");
            }

            var weaponParts = weaponInstance
                .GetInstanceModule<WeaponPartsInstanceModule>()?.WeaponParts;

            if (weaponParts == null || weaponParts.Count == 0)
            {
                Debug.LogError("Weapon instance has no WeaponPartsInstanceModule");
                return weapon;
            }

            GameObject weaponRoot = new GameObject("WeaponPartsRoot");
            weaponRoot.transform.SetParent(weapon.transform, false);

            foreach (var part in weaponParts)
            {
                var partWorldPrefab = part
                    .GetModule<WorldVisualItemModule>()?.WorldPrefab;

                if (partWorldPrefab == null)
                {
                    Debug.LogWarning($"Item '{part.ItemName}' cannot be created in the world because it has no WorldVisual module.");
                    continue;
                }

                GameObject partInstance = Object.Instantiate(partWorldPrefab, weaponRoot.transform);
                partInstance.name = part.ItemName;
            }

            return weapon;
        }
    }
}
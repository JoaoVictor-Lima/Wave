using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.InstancedModules;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.Gameplay.Items.Weapon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Items
{
    public class ItemInstanceFactory
    {
        public static ItemInstance Create(ItemData itemData, int quantity = 1, List<ItemData> componentItems = null)
        {
            var instancedModules = new List<InstancedModule>();

            foreach (var itemDataModule in itemData.Modules)
            {
                switch (itemDataModule)
                {
                    case WeaponModule weaponModule:
                        instancedModules = CreateWeaponInstancedModules(itemData, weaponModule, componentItems);
                        break;

                    default:
                        break;
                }
            }

            var instance = new ItemInstance(itemData, quantity, instancedModules);
            return instance;
        }

        private static List<InstancedModule> CreateWeaponInstancedModules(ItemData itemData, WeaponModule weaponModule, List<ItemData> componentItems)
        {
            var instancedModules = new List<InstancedModule>();


            var weaponStatsModule = CreateWeaponStatsInstanceModule();
            instancedModules.Add(weaponStatsModule);


            var weaponPartsModule = CreateWeaponPartsInstanceModule(itemData, weaponModule, componentItems);
            instancedModules.Add(weaponPartsModule);

            return instancedModules;
        }

        private static InstancedModule CreateWeaponPartsInstanceModule(ItemData itemData, WeaponModule weaponModule, List<ItemData> componentItems)
        {
            var partsModule = new WeaponPartsInstanceModule
            {
                WeaponParts = new List<ItemData>()
            };

            if (componentItems != null)
            {
                var weaponParts = componentItems
                    .Where(item => item.Modules.Any(module => module is WeaponPartShapeModule))
                    .ToList();

                if (weaponModule.RequiredParts.Count != weaponParts.Count)
                {
                    Debug.Log($"Some component items provided for {itemData.ItemName} are not valid weapon parts. Expected {weaponModule.RequiredParts.Count}, but got {weaponParts.Count}.");
                }

                foreach (var requiredPart in weaponModule.RequiredParts)
                {

                    var weaponPartsInstanceModule = GetWeaponIntancedPart(requiredPart, weaponParts);

                    if (weaponPartsInstanceModule == null)
                    {
                        Debug.LogError($"Failed to create weapon part instance for {requiredPart.PartType}");
                        continue;
                    }

                    partsModule.WeaponParts.Add(weaponPartsInstanceModule);
                }

                if (weaponParts.Count != weaponModule.RequiredParts.Count)
                {
                    Debug.Log($"Some provided weapon parts for {itemData.ItemName} were not used. Expected {weaponModule.RequiredParts.Count}, but got {weaponParts.Count}. Check the logs for details.");
                }
            }

            return partsModule;
        }

        private static WeaponStatsInstanceModule CreateWeaponStatsInstanceModule()
        {
            return new WeaponStatsInstanceModule
            {
                Density = 10f,
                Balance = 5f,
                Sharpness = 1f
            };
        }

        private static ItemData GetWeaponIntancedPart(WeaponPartRequirement requiredPart, List<ItemData> componentItems)
        {
            foreach (var item in componentItems)
            {
                var weaponPartModule = item.Modules
                    .OfType<WeaponPartShapeModule>()
                    .FirstOrDefault();

                if (weaponPartModule != null && weaponPartModule.PartType == requiredPart.PartType)
                {
                    return item;
                }
            }

            Debug.LogError($"Nenhum componente válido encontrado para a parte {requiredPart.PartType}");
            return null;
        }
    }
}

using ACMF.ModHelper.ModPrefabs.Buildings.Interfaces;
using Harmony;
using System;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Buildings.Patches
{
    [HarmonyPatch(typeof(BuildingController))]
    [HarmonyPatch("GetItemGameObject")]
    [HarmonyPatch(new Type[] { typeof(Enums.ItemType) })]
    public class BuildingControllerGetItemGameObjectPatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ref GameObject __result, Enums.ItemType itemType)
        {
            IBuildingCreator buildingCreator = ActiveBuildingCreators.GetCreatorFromEnum(itemType);
            if (buildingCreator != null)
            {
                __result = buildingCreator.Prefab;
                return false;
            }

            return true;
        }
    }
}

using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Procurments
{
    public static class ProcurmentManager
    {
        public static Dictionary<Enums.ProcureableProductType, ProcurmentTemplate> ProcureableProducts { get; private set; } 
            = new Dictionary<Enums.ProcureableProductType, ProcurmentTemplate>();

        [HarmonyPatch(typeof(ProcurementController))]
        [HarmonyPatch("GenerateProcureable")]
        public class ProcurmentControllerGenerateProcureablePatcher
        {
            [HarmonyPrefix]
            public static bool Prefix(ProcurementController __instance, Enums.ProcureableProductType productType)
            {
                System.Console.WriteLine($"ProcurementController::GenerateProcureable {productType}");
                if (ProcureableProducts.ContainsKey(productType))
                {
                    ProcureableProduct pp = ProcureableProducts[productType].CreateProcureableProduct();
                    __instance.allAvailableProcureableProducts.Add(pp);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ProcurementController))]
        [HarmonyPatch("GetProcureableProductSprite")]
        public class ProcurmentControllerGetProcureableProductSpritePatcher
        {
            [HarmonyPrefix]
            public static bool Prefix(Sprite __result, Enums.ProcureableProductType procureableProductType)
            {
                __result = DataPlaceholder.Instance.procureableProductSprites[1];
                return false;
            }
        }
    }
}

using Harmony;
using System.Collections.Generic;

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
                if (ProcureableProducts.ContainsKey(productType))
                {
                    __instance.allAvailableProcureableProducts.Add(ProcureableProducts[productType].CreateProcureableProduct());
                    return false;
                }

                return true;
            }
        }
    }
}

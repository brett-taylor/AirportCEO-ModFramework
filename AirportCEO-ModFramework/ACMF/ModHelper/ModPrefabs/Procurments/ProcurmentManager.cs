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
                if (ProcureableProducts.ContainsKey(productType))
                {
                    System.Console.WriteLine($"ProcurementController::GenerateProcureable {productType}");
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
                if (ProcureableProducts.ContainsKey(procureableProductType))
                {
                    __result = ProcureableProducts[procureableProductType].Sprite;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ProcurementController))]
        [HarmonyPatch("SpawnProcureable")]
        public class ProcurmentControllerSpawnProcureablePatcher
        {
            [HarmonyPrefix]
            public static bool Prefix(Enums.ProcureableProductType productType)
            {
                if (ProcureableProducts.ContainsKey(productType))
                {
                    ProcurmentTemplate pt = ProcureableProducts[productType];
                    pt.SpawnProcureable();
                    NotificationController.Instance.AttemptSendNotification("A new product has arrived!", CameraController.Instance.GetWorldCenter(),
                        Enums.NotificationType.Other, Enums.MessageSeverity.Unspecified, pt.Title + "NewProduct", pt.Title, Enums.InteractableObjectType.Vehicle, true);
                    return false;
                }
                
                return true;
            }
        }
    }
}

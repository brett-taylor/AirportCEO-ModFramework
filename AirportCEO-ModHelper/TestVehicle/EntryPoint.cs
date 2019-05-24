using ACMH.Utilities.Extensions;
using ACMH.Utilities.Misc;
using ACML.ModLoader;
using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TestVehicle
{
    [ACML.ModLoader.Attributes.ACMLMod(id: "TestVehicleTestBed", name: "Test Vehicle Test Bed", modVersion: "1.0.0", requiredACMLVersion: "0.1.0")]
    public class EntryPoint
    {
        public static HarmonyInstance HarmonyInstance { get; set; }
        public static Mod Mod { get; set; }
        public static Enums.ProcureableProductType ProductTypeEnum { get; set; } = (Enums.ProcureableProductType)123532;

        [ACML.ModLoader.Attributes.ACMLModEntryPoint]
        public static void Entry(Mod mod)
        {
            Mod = mod;
            HarmonyInstance = HarmonyInstance.Create(Mod.ModInfo.ID);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("GetValues")]
    public class EnumGetValuesPatcher
    {
        [HarmonyPostfix]
        private static void Postfix_GetValues(Type enumType, ref Array __result)
        {
            if (enumType.Equals(typeof(Enums.ProcureableProductType)))
            {
                Console.WriteLine($"Enum::GetValues Patch for Enums.ProcureableProductType");
                var listArray = new List<Enums.ProcureableProductType>();
                foreach (Enums.ProcureableProductType productType in __result)
                {
                    listArray.Add(productType);
                }

                listArray.Add(EntryPoint.ProductTypeEnum);

                __result = listArray.ToArray();
            }
        }
    }

    [HarmonyPatch(typeof(ProcurementController))]
    [HarmonyPatch("GetProcureableProductSprite")]
    public class ProcurmentControllerGetProcureableProductSpritePatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(Sprite __result, Enums.ProcureableProductType procureableProductType)
        {
            Console.WriteLine($"ProcurementController::GetProcureableProductSprite Patch for {procureableProductType}");
            if (procureableProductType == EntryPoint.ProductTypeEnum)
            {
                __result = DataPlaceholder.Instance.procureableProductSprites[13];
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(ProcurementController))]
    [HarmonyPatch("GenerateProcureable")]
    public class ProcurmentControllerGenerateProcureablePatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ProcurementController __instance, Enums.ProcureableProductType productType)
        {
            Console.WriteLine($"ProcurementController::GenerateProcureable Patch for {productType}");
            if (productType == EntryPoint.ProductTypeEnum)
            {
                ProcureableProduct procureableProduct = new ProcureableProduct
                {
                    type = EntryPoint.ProductTypeEnum,
                    title = "Cool Test Bed Vehicle",
                    category = Enums.ProcureableProductCategory.Vehicles,
                    subCategory = Enums.ProcureableProductSubCategory.ServiceVehicles,
                    description = "Cool Test Bed that does fuck all",
                    fixedCost = 100f,
                    operatingCost = 5f,
                    deliveryTime = new TimeSpan(0, 10, 0),
                    isQuantifiable = true,
                    isPhysicalProduct = true,
                    prerequisiteForDisplay = new ProcurementController.Prerequisite[0],
                    prerequisite = new PrerequisiteContainer[0]
                };

                __instance.allAvailableProcureableProducts.Add(procureableProduct);
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
            Console.WriteLine($"ProcurementController::SpawnProcureable Patched for {productType}");
            if (productType == EntryPoint.ProductTypeEnum)
            {
                string text = "";

                /*GameObject serviceCar = UnityEngine.Object.Instantiate(TrafficController.Instance.serviceCarPrefab, FolderController.Instance.GetSceneRootTransform());
                ServiceCarController serviceCarController = serviceCar.GetComponent<ServiceCarController>();



                /*if (serviceCarController != null)
                    UnityEngine.Object.Destroy(serviceCarController);

                VehicleController vehicleController = serviceCar.AddComponent<MaintenanceTruckController>();
                vehicleController.Initialize();
                TrafficController.Instance.AddVehicleToSpawnQueue(vehicleController, false);

                NotificationController.Instance.AttemptSendNotification("A new product has arrived!", CameraController.Instance.GetWorldCenter(), 
                    Enums.NotificationType.Other, Enums.MessageSeverity.Unspecified, text + "NewProduct", text, Enums.InteractableObjectType.Vehicle, true);*/

                GameObject gameObject8 = Singleton<TrafficController>.Instance.SpawnVehicleGameObject(Enums.VehicleType.ServiceCar, Enums.VehicleSubType.Unspecified);
                gameObject8.DumpFields();
                ServiceCarController component7 = gameObject8.GetComponent<ServiceCarController>();
                component7.Initialize();
                component7.ServiceVehicleModel.isOwnedByAirport = true;
                Singleton<TrafficController>.Instance.AddVehicleToSpawnQueue(component7, false);

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(VehicleDoorManager))]
    [HarmonyPatch("Awake")]
    public class VehicleDoorManagerAwakePatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(VehicleDoorManager __instance)
        {
            if (__instance.frontDoorPoints == null|| __instance.rearDoorPoints == null)
            {
                __instance.animator = __instance.GetComponent<Animator>();
                __instance.allAccessPoints = new List<Transform>();
                return false;
            }

            return true;
        }
    }
}
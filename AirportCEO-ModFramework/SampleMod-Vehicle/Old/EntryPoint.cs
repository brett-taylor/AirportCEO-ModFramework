using ACMF.ModHelper.EnumPatcher;
using ACMF.ModHelper.ModPrefabs;
using ACMF.ModHelper.Utilities.Misc;
using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SampleModVehicle
{
    [ACMFMod(id: "TestVehicleTestBed", name: "Test Vehicle Test Bed", modVersion: "1.0.0", requiredACMLVersion: "0.1.0")]
    public class EntryPoint
    {
        public static HarmonyInstance HarmonyInstance { get; set; }
        public static Mod Mod { get; set; }
        public static Enums.ProcureableProductType ProductTypeEnum { get; set; }
        public static Enums.VehicleType VehicleType { get; set; }
        public static Enums.VehicleJobTaskType VehicleJobTask { get; set; }

        [ACMFModEntryPoint]
        public static void Entry(Mod mod)
        {
            Mod = mod;
            HarmonyInstance = HarmonyInstance.Create(Mod.ModInfo.ID);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            Assets.Initialise();

            VehicleType = EnumCache<Enums.VehicleType>.Instance.Patch("TestTruck");
            ProductTypeEnum = EnumCache<Enums.ProcureableProductType>.Instance.Patch("TestTruckProcureable");
            VehicleJobTask = EnumCache<Enums.VehicleJobTaskType>.Instance.Patch("TestTruckVehicleJobType");
        }
    }

    [HarmonyPatch(typeof(ProcurementController))]
    [HarmonyPatch("GetProcureableProductSprite")]
    public class ProcurmentControllerGetProcureableProductSpritePatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(Sprite __result, Enums.ProcureableProductType procureableProductType)
        {
            if (procureableProductType == EntryPoint.ProductTypeEnum)
            {
                __result = DataPlaceholder.Instance.procureableProductSprites[13];
                return false;
            }

            return true;
        }
    }

    public class ProcurmentControllerGenerateProcureablePatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ProcurementController __instance, Enums.ProcureableProductType productType)
        {
            if (productType == EntryPoint.ProductTypeEnum)
            {
                ProcureableProduct procureableProduct = new ProcureableProduct
                {
                    type = EntryPoint.ProductTypeEnum,
                    title = "Test Truck",
                    category = Enums.ProcureableProductCategory.Vehicles,
                    subCategory = Enums.ProcureableProductSubCategory.ServiceVehicles,
                    description = "Cool Test Truck that does nothing because the job system is hard to understand",
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
                Console.WriteLine($"!!!!!!!IMPORTANT! TestCar Spawned");

                GameObject testCar = Assets.GetGameObjectForTestTruck();

                TestTruckController scc = testCar.GetComponent<TestTruckController>();
                scc.Initialize();
                scc.ServiceVehicleModel.isOwnedByAirport = true;
                TrafficController.Instance.AddVehicleToSpawnQueue(scc, false);
                testCar.DumpFields();
                SpawnTestCar.TEST.Add(testCar);

                NotificationController.Instance.AttemptSendNotification("A new product has arrived!", CameraController.Instance.GetWorldCenter(),
                    Enums.NotificationType.Other, Enums.MessageSeverity.Unspecified, text + "NewProduct", text, Enums.InteractableObjectType.Vehicle, true);
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
            if (__instance.frontDoorPoints == null || __instance.rearDoorPoints == null)
            {
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerInputController))]
    [HarmonyPatch("Update")]
    public class SpawnTestCar
    {
        public static List<GameObject> TEST = new List<GameObject>();

        [HarmonyPostfix]
        public static void Postfix()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q))
            {
                GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.ServiceCar, Enums.VehicleSubType.Unspecified);
                ServiceVehicleController component3 = gameObject4.GetComponent<ServiceVehicleController>();
                component3.Initialize();
                component3.ServiceVehicleModel.isOwnedByAirport = true;
                Singleton<TrafficController>.Instance.AddVehicleToSpawnQueue(component3, false);
                gameObject4.DumpFields();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
            {
                ProcurmentControllerSpawnProcureablePatcher.Prefix(EntryPoint.ProductTypeEnum);
                if (TEST != null && TEST.Last() != null)
                    TEST.Last()?.DumpFields();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E))
            {
                ACMF.ModHelper.DialogPopup.DialogManager.QueueMessagePanel($"TestCar: {TEST.Last()?.name ?? "Null"}");
                if (TEST != null && TEST.Last() != null)
                    TEST.Last().transform.position = new Vector2(20f, 20f);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                StandModel standModel = BuildingController.Instance.GetArrayOfSpecificStructureType(Enums.StructureType.AircraftStand)[0] as StandModel;
                JobTaskRequestManager.RequestTestJob(standModel);

                ACMF.ModHelper.Utilities.Logger.ShowNotification($"Vehicle Count: {TEST.Count}");
                foreach (GameObject gameObject in TEST)
                    ACMF.ModHelper.Utilities.Logger.ShowNotification($"Job Agent: {gameObject.name} || Job: {gameObject.GetComponent<ServiceVehicleController>().CurrentJobTaskReferenceID ?? "Empty"}");
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
            {
                /*Enums.VehicleType vt = EnumCache<Enums.VehicleType>.Instance.BackingStore.IntToEnum(24);
                ACMF.ModHelper.DialogPopup.DialogManager.QueueMessagePanel($"Test 1: {vt} || {vt.ToString()}");

                Enums.VehicleType vt2 = EnumCache<Enums.VehicleType>.Instance.BackingStore.IntToEnum(25);
                ACMF.ModHelper.DialogPopup.DialogManager.QueueMessagePanel($"Test 2: {vt2} || {vt2.ToString()}");*/
                
                Enums.VehicleType vt = EnumCache<Enums.VehicleType>.Instance.Patch("TestThisEnum" + UnityEngine.Random.Range(50, 200));
                ACMF.ModHelper.DialogPopup.DialogManager.QueueMessagePanel($"Test 1: {vt} || {vt.ToString()}");
            }
        }
    }

    public class JobTaskRequestManager
    {
        public static void RequestTestJob(StandModel cfm)
        {
            JobTaskController.Instance.CreateGenericFlightJobTask(
                TimeController.Instance.GetCurrentContinuousTimeAsTimeSpan(),
                new TimeSpan(1, 0, 0),
                "",
                Enums.TravelDirection.Unspecified,
                EntryPoint.VehicleType,
                EntryPoint.VehicleJobTask,
                new string[] { cfm.referenceID },
                true,
                cfm.serviceVehicleEntryPosition.position,
                cfm.serviceVehicleEntryPosition.eulerAngles,
                true,
                false,
                Enums.TrailerType.Unspecified,
                0
            );
        }
    }

    [HarmonyPatch(typeof(ServiceVehicleController))]
    [HarmonyPatch("GenerateSpecificServiceVehicleJobTaskActionChain")]
    public static class GenerateSpecificServiceVehicleJobTaskActionChainPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(ServiceVehicleController __instance)
        {
            /*ACMF.ModHelper.Utilities.Logger.ShowDialog($"GenerateSpecificServiceVehicleJobTaskActionChain patched {__instance.VehicleType}");
            if (__instance.ServiceVehicleModel.vehicleType == EntryPoint.VehicleType)
            {
                __instance.ServiceVehicleModel.AddToActionList(Enums.ServiceVehicleAction.MoveToTransitStructure);
                __instance.ServiceVehicleModel.AddToActionList(Enums.ServiceVehicleAction.WaitForTransitStructureOccupation);
                __instance.ServiceVehicleModel.AddToActionList(Enums.ServiceVehicleAction.ParkAtTransitStructure);
            }*/
        }
    }

    [HarmonyPatch(typeof(ServiceVehicleController))]
    [HarmonyPatch("AttemptGenerateSpecificVehicleJobTaskActionChain")]
    public static class AttemptGenerateSpecificVehicleJobTaskActionChainPatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ServiceVehicleController __instance, ref bool __result)
        {
            if (__instance.VehicleType == EntryPoint.VehicleType)
            {
                ACMF.ModHelper.DialogPopup.DialogManager.QueueMessagePanel($"AttemptGenerateSpecificVehicleJobTaskActionChain patched {__instance.VehicleType}");
                __result = true;
                return false;
            }

            return true;
        }
    }
}

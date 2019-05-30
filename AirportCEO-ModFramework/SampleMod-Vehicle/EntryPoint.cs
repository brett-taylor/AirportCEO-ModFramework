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
        public static Enums.ProcureableProductType ProductTypeEnum { get; set; } = (Enums.ProcureableProductType)21470001;
        public static Enums.VehicleType VehicleType { get; set; } = (Enums.VehicleType)25;
        public static Enums.VehicleJobTaskType VehicleJobTask = (Enums.VehicleJobTaskType)21470002;

        [ACMFModEntryPoint]
        public static void Entry(Mod mod)
        {
            Mod = mod;
            HarmonyInstance = HarmonyInstance.Create(Mod.ModInfo.ID);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            Assets.Initialise();
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

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("ToString")]
    [HarmonyPatch(new Type[0])]
    public class EnumToStringPatcher
    {
        [HarmonyPrefix]
        private static bool Prefix(Enum __instance, ref string __result)
        {
            if ((__instance is Enums.VehicleType vehicleType && vehicleType == EntryPoint.VehicleType)
                || (__instance is Enums.ProcureableProductType productType && productType == EntryPoint.ProductTypeEnum))
            {
                __result = "Test Truck";
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("IsDefined")]
    [HarmonyPatch(new Type[] { typeof(Type), typeof(object) })]
    public class EnumIsDefinedPatcher
    {
        [HarmonyPrefix]
        private static bool Prefix_IsDefined(Type enumType, object value, ref bool __result)
        {
            if (enumType.Equals(typeof(Enums.VehicleType)) && (Enums.VehicleType)value == EntryPoint.VehicleType)
            {
                __result = true;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("Parse")]
    [HarmonyPatch(new Type[] { typeof(Type), typeof(string), typeof(bool) })]
    public class EnumParsePatcher
    {
        private static bool Prefix_Parse(Type enumType, string value, bool ignoreCase, ref object __result)
        {
            if (enumType.Equals(typeof(Enums.VehicleType)))
            {
                if (ignoreCase && value.Equals("test truck"))
                {
                    __result = EntryPoint.VehicleType;
                    return false;
                }

                if (ignoreCase == false && value.Equals("Test Truck"))
                {
                    __result = EntryPoint.VehicleType;
                    return false;
                }
            }

            if (enumType.Equals(typeof(Enums.ProcureableProductType)))
            {
                if (ignoreCase && value.Equals("test truck"))
                {
                    __result = EntryPoint.ProductTypeEnum;
                    return false;
                }

                if (ignoreCase == false && value.Equals("Test Truck"))
                {
                    __result = EntryPoint.ProductTypeEnum;
                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("GetNames")]
    [HarmonyPatch(new Type[] { typeof(Type) })]
    public class EnumGetNamesPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(Type enumType, ref Array __result)
        {
            if (enumType.Equals(typeof(Enums.VehicleType)))
            {
                var listArray = new List<string>();
                foreach (string vehicleType in __result)
                {
                    listArray.Add(vehicleType);
                }

                listArray.Add(EntryPoint.VehicleType.ToString());

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
                ACMF.ModHelper.Utilities.Logger.ShowDialog($"TestCar: {TEST.Last()?.name ?? "Null"}");
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
                List<TestTruckModel> models = new List<TestTruckModel>();

                VehicleController[] vehicleArray = Singleton<TrafficController>.Instance.GetVehicleArray();
                foreach(VehicleController vc in vehicleArray)
                    if (vc is TestTruckController)
                        models.Add(vc.GetComponent<TestTruckController>().TestTruckModel);

                Serialization.ACMHVehicleWrapper aCMHVehicleWrapper = new Serialization.ACMHVehicleWrapper();
                aCMHVehicleWrapper.VehicleModels.AddRange(models);

                string json = JsonHelper.ToJson(aCMHVehicleWrapper.VehicleModels.ToArray(), true);
                ACMF.ModHelper.Utilities.Logger.ShowDialog($"Count: {aCMHVehicleWrapper.VehicleModels.Count} || JSON: {json}");
                System.Console.WriteLine($" ");
                System.Console.WriteLine($" ");
                System.Console.WriteLine($"JSON: {json}");
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Y))
            {
                TestTruckModel model = TEST.First().GetComponent<TestTruckController>().TestTruckModel;
                string json = JsonUtility.ToJson(model);

                ACMF.ModHelper.Utilities.Logger.ShowDialog($"JSON: {json}");
                System.Console.WriteLine($" ");
                System.Console.WriteLine($" ");
                System.Console.WriteLine($"JSON: {json}");
            }

            /*foreach (GameObject gameObject in TEST)
            {
                string referenceJob = gameObject.GetComponent<ServiceVehicleController>().CurrentJobTaskReferenceID;
                if (string.IsNullOrEmpty(referenceJob) == false)
                {
                    ACMH.Utilities.Logger.ShowNotification($"JOB SET: {gameObject.name} has job {referenceJob}");
                }
            }*/
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
            ACMF.ModHelper.Utilities.Logger.ShowDialog($"GenerateSpecificServiceVehicleJobTaskActionChain patched {__instance.VehicleType}");
            if (__instance.ServiceVehicleModel.vehicleType == EntryPoint.VehicleType)
            {
                __instance.ServiceVehicleModel.AddToActionList(Enums.ServiceVehicleAction.MoveToTransitStructure);
                __instance.ServiceVehicleModel.AddToActionList(Enums.ServiceVehicleAction.WaitForTransitStructureOccupation);
                __instance.ServiceVehicleModel.AddToActionList(Enums.ServiceVehicleAction.ParkAtTransitStructure);
            }
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
                ACMF.ModHelper.Utilities.Logger.ShowDialog($"AttemptGenerateSpecificVehicleJobTaskActionChain patched {__instance.VehicleType}");
                __result = true;
                return false;
            }

            return true;
        }
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}

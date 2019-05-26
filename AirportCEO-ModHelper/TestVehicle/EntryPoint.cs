using ACMH.Utilities.Misc;
using ACML.ModLoader;
using Harmony;
using System;
using System.Linq;
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
        public static Enums.ProcureableProductType ProductTypeEnum { get; set; } = (Enums.ProcureableProductType) 21470001;
        public static Enums.VehicleType VehicleType { get; set; } = (Enums.VehicleType) 25;
        public static Enums.VehicleJobTaskType VehicleJobTask = (Enums.VehicleJobTaskType) 21470002;

        [ACML.ModLoader.Attributes.ACMLModEntryPoint]
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
            if (enumType.Equals(typeof(Enums.VehicleType)) && (Enums.VehicleType) value == EntryPoint.VehicleType)
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

                GameObject testCar = UnityEngine.Object.Instantiate(Assets.TEST_VEHICLE);
                testCar.transform.SetParent(FolderController.Instance.GetSceneRootTransform(), false);

                //////////////////////////////////////////
                VehicleDoorManager vdm = testCar.transform.Find("Doors").gameObject.AddComponent<VehicleDoorManager>();
                vdm.frontDoorPoints = new List<Transform>();
                vdm.rearDoorPoints = new List<Transform>();
                vdm.cargoDoorPoints = new List<Transform>();
                vdm.transformsToHide = new List<Transform>();
                vdm.frontDoorPoints.Add(testCar.transform.Find("Doors/FrontDoor1"));
                vdm.frontDoorPoints.Add(testCar.transform.Find("Doors/FrontDoor2"));
                vdm.rearDoorPoints.Add(testCar.transform.Find("Doors/RearDoor1"));
                vdm.cargoDoorPoints.Add(testCar.transform.Find("Doors/CargoPoint"));
                vdm.allAccessPoints = new List<Transform>();
                vdm.allAccessPoints.AddRange(vdm.frontDoorPoints);
                vdm.allAccessPoints.AddRange(vdm.rearDoorPoints);
                //////////////////////////////////////////

                //////////////////////////////////////////
                VehicleLightManager vlm = testCar.transform.Find("Lights").gameObject.AddComponent<VehicleLightManager>();
                //////////////////////////////////////////

                //////////////////////////////////////////
                BoundaryHandler bh = testCar.transform.Find("Boundary").gameObject.AddComponent<BoundaryHandler>();
                bh.zoneType = (Enums.ZoneType) 4;
                bh.boundaryType = BoundaryHandler.BoundaryType.PersonGrid;
                //////////////////////////////////////////

                //////////////////////////////////////////
                ShadowHandler shadowHandler = testCar.transform.Find("Sprite/Shadow").gameObject.AddComponent<ShadowHandler>();
                shadowHandler.shadowDistance = 0.175f;
                //////////////////////////////////////////

                //////////////////////////////////////////
                VehicleAudioManager vehicleAudio = testCar.transform.Find("Audio").gameObject.AddComponent<VehicleAudioManager>();
                //////////////////////////////////////////

                //////////////////////////////////////////
                TestTruckController scc = testCar.AddComponent<TestTruckController>();
                scc.colorableParts = new SpriteRenderer[] { testCar.transform.Find("Sprite/Chassie").gameObject.GetComponent<SpriteRenderer>() };
                scc.cargoDoors = new Transform[0];
                scc.doorManager = vdm;
                scc.lightManager = vlm;
                scc.audioManager = vehicleAudio;
                scc.exhaust = testCar.GetComponentInChildren<ParticleSystem>();
                scc.shadows = new ShadowHandler[] { shadowHandler };
                scc.boundary = bh;
                scc.thoughtsReferenceList = new List<Thought>(); 
                scc.currentShipment = new Shipment(Vector3.zero, Enums.DeliveryContainerType.Unspecified, Enums.DeliveryContentType.Unspecified, 0, "");
                scc.currentActionDescriptionListReference = new List<Enums.ServiceVehicleAction>();
                //////////////////////////////////////////

                //////////////////////////////////////////
                scc.gameObject.SetActive(false);
                scc.transform.position = Vector3.zero;
                scc.Initialize();
                scc.ServiceVehicleModel.isOwnedByAirport = true;
                TrafficController.Instance.AddVehicleToSpawnQueue(scc, false);
                vlm.ActivateLights();
                vlm.ToggleWarningLights(true);
                testCar.DumpFields();
                SpawnTestCar.TEST.Add(testCar);
                //////////////////////////////////////////

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
            if (__instance.frontDoorPoints == null|| __instance.rearDoorPoints == null)
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
                ACMH.Utilities.Logger.ShowDialog($"TestCar: {TEST.Last()?.name ?? "Null"}");
                if (TEST != null && TEST.Last() != null)
                    TEST.Last().transform.position = new Vector2(20f, 20f);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                StandModel standModel = BuildingController.Instance.GetArrayOfSpecificStructureType(Enums.StructureType.AircraftStand)[0] as StandModel;
                JobTaskRequestManager.RequestTestJob(standModel);

                ACMH.Utilities.Logger.ShowNotification($"Vehicle Count: {TEST.Count}");
                foreach (GameObject gameObject in TEST)
                    ACMH.Utilities.Logger.ShowNotification($"Job Agent: {gameObject.name} || Job: {gameObject.GetComponent<ServiceVehicleController>().CurrentJobTaskReferenceID ?? "Empty"}");
            }

            foreach (GameObject gameObject in TEST)
            {
                string referenceJob = gameObject.GetComponent<ServiceVehicleController>().CurrentJobTaskReferenceID;
                if (string.IsNullOrEmpty(referenceJob) == false)
                {
                    ACMH.Utilities.Logger.ShowNotification($"JOB SET: {gameObject.name} has job {referenceJob}");
                }
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
            ACMH.Utilities.Logger.ShowDialog($"GenerateSpecificServiceVehicleJobTaskActionChain patched {__instance.VehicleType}");
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
                ACMH.Utilities.Logger.ShowDialog($"AttemptGenerateSpecificVehicleJobTaskActionChain patched {__instance.VehicleType}");           
                __result = true;
                return false;
            }

            return true;
        }
    }
}
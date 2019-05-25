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
                    deliveryTime = new TimeSpan(0, 1, 0),
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
                scc.colorableParts = new SpriteRenderer[0];
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
                SpawnBeltLoader.TEST = testCar;
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

    [HarmonyPatch(typeof(VehicleController))]
    [HarmonyPatch("Initialize")]
    public class VehicleControllerInitializePrefixPatcher
    {
        [HarmonyPrefix]
        public static void Prefix(VehicleController __instance)
        {
            Console.WriteLine($"HarmonyPrefix VehicleController::Initialize called on {__instance.gameObject.name}");
        }
    }

    [HarmonyPatch(typeof(VehicleController))]
    [HarmonyPatch("Initialize")]
    public class VehicleControllerInitializePostfixPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(VehicleController __instance)
        {
            Console.WriteLine($"HarmonyPostfix VehicleController::Initialize called on {__instance.gameObject.name}");
        }
    }

    [HarmonyPatch(typeof(VehicleController))]
    [HarmonyPatch("GetAllSpritesInObject")]
    public class VehicleControllerGetAllSpritesInObjectPrefixPatcher
    {
        [HarmonyPrefix]
        public static void Postfix(VehicleController __instance)
        {
            Console.WriteLine($"HarmonyPrefix VehicleController::GetAllSpritesInObject called on {__instance.gameObject.name}");
        }
    }

    [HarmonyPatch(typeof(PlayerInputController))]
    [HarmonyPatch("Update")]
    public static class SpawnBeltLoader
    {
        public static GameObject TEST;

        [HarmonyPostfix]
        public static void Postfix()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Y))
            {
                GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.PushbackTruck, Enums.VehicleSubType.Unspecified);
                PushbackTruckController component3 = gameObject4.GetComponent<PushbackTruckController>();
                component3.Initialize();
                component3.ServiceVehicleModel.isOwnedByAirport = true;
                Singleton<TrafficController>.Instance.AddVehicleToSpawnQueue(component3, false);
                TrafficController.Instance.StartCoroutine(TestTest(component3));
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.U))
            {
                ProcurmentControllerSpawnProcureablePatcher.Prefix(EntryPoint.ProductTypeEnum);
                TrafficController.Instance.StartCoroutine(TestTest(TEST.GetComponent<TestTruckController>()));
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.I))
            {
                ACMH.Utilities.Logger.ShowDialog($"TestCar: {TEST?.name ?? "Null"}");
                if (TEST != null)
                {
                    TEST.transform.position = new Vector2(200f, 200f);
                }
            }
        }

        public static IEnumerator TestTest(ServiceVehicleController controller)
        {
            ACMH.Utilities.Logger.ShowNotification("CALLED");
            yield return new WaitForSecondsRealtime(10f);
            controller.gameObject.DumpFields();
            Console.WriteLine($"[TESTTEST] Object Name: {controller.gameObject.name} Component name {controller.GetType()}");
            Console.WriteLine($"[TESTTEST] currentActionDescriptionListReference.Count: {controller.currentActionDescriptionListReference.Count}");
            foreach(Enums.ServiceVehicleAction action in controller.currentActionDescriptionListReference)
            {
                Console.WriteLine($"[TESTTEST] element: {action}");
            }
            ACMH.Utilities.Logger.ShowNotification("DONE DONE DONE");
            ACMH.Utilities.Logger.ShowDialog("done");
        }
    }
}
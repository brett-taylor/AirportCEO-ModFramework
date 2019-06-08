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
            VehicleJobTask = EnumCache<Enums.VehicleJobTaskType>.Instance.Patch("TestTruckVehicleJobType");
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
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
            {
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

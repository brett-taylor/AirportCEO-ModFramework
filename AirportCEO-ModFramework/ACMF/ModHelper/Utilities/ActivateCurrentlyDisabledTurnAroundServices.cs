using ACMF.ModHelper.Utilities.Misc;
using Harmony;
using UnityEngine;

namespace ACMF.ModHelper.Utilities
{
    [HarmonyPatch(typeof(OperationsOverviewPanelUI))]
    [HarmonyPatch("LoadPanel")]
    public class ActivateCurrentlyDisabledTurnAroundServicesOperationsPanelPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(OperationsOverviewPanelUI __instance)
        {
            foreach (Transform t in __instance.cateringServiceStatusText.transform.parent.parent)
                t.gameObject.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(PlayerInputController))]
    [HarmonyPatch("Update")]
    public class ActivateCurrentlyDisabledTurnAroundServicesBuildingControllerPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(BuildingController __instance)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKeyDown(KeyCode.Z))
            {
                BuildingController.Instance.SpawnStructure(Enums.StructureType.CateringDepot);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKeyDown(KeyCode.X))
            {
                BuildingController.Instance.SpawnStructure(Enums.StructureType.AircraftDeicingStand);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKeyDown(KeyCode.C))
            {
                BuildingController.Instance.SpawnStructure(Enums.StructureType.DeicingFluidDepot);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKeyDown(KeyCode.V))
            {
                BuildingController.Instance.SpawnStructure(Enums.StructureType.AircraftDeicingStand);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKeyDown(KeyCode.B))
            {
                GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.CateringTruck, Enums.VehicleSubType.Unspecified);
                ServiceVehicleController component3 = gameObject4.GetComponent<ServiceVehicleController>();
                component3.Initialize();
                component3.ServiceVehicleModel.isOwnedByAirport = true;
                Singleton<TrafficController>.Instance.AddVehicleToSpawnQueue(component3, false);
                gameObject4.DumpFields();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKeyDown(KeyCode.N))
            {
                GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.AircraftCabinCleaningTruck, Enums.VehicleSubType.Unspecified);
                ServiceVehicleController component3 = gameObject4.GetComponent<ServiceVehicleController>();
                component3.Initialize();
                component3.ServiceVehicleModel.isOwnedByAirport = true;
                Singleton<TrafficController>.Instance.AddVehicleToSpawnQueue(component3, false);
                gameObject4.DumpFields();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKeyDown(KeyCode.M))
            {
                GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.DeicingTruck, Enums.VehicleSubType.Unspecified);
                ServiceVehicleController component3 = gameObject4.GetComponent<ServiceVehicleController>();
                component3.Initialize();
                component3.ServiceVehicleModel.isOwnedByAirport = true;
                Singleton<TrafficController>.Instance.AddVehicleToSpawnQueue(component3, false);
                gameObject4.DumpFields();
            }
        }
    }
}


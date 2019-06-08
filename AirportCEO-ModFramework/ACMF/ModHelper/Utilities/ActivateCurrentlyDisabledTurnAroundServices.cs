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

    [HarmonyPatch(typeof(BuildingController))]
    [HarmonyPatch("Awake")]
    public class ActivateCurrentlyDisabledTurnAroundServicesBuildingControllerPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(BuildingController __instance)
        {
            Utilities.Logger.Print($"TESTTESTEST: {__instance.mediumDeicingPad.activeSelf}");
            Utilities.Logger.Print($"TESTTESTEST 2: {__instance.mediumDeicingPad.transform.parent?.name ?? "null"}");
            Utilities.Logger.Print($"TESTTESTEST 3: {__instance.largeATCTower.transform.parent?.name ?? "null"}");
            __instance.cateringDepot.SetActive(true);
            __instance.wasteDepot.SetActive(true);
            __instance.deicingFluidDepot.SetActive(true);
            __instance.smallDeicingPad.SetActive(true);
            __instance.mediumDeicingPad.SetActive(true);
            __instance.largeStand.SetActive(true);
        }
    }
}

/*
 * BuildingController.Instance.SpawnStructure(Enums.StructureType.CateringDepot);
 * 
 * BuildingController.Instance.SpawnStructure(Enums.StructureType.AircraftDeicingStand);
 * 
 * BuildingController.Instance.SpawnStructure(Enums.StructureType.DeicingFluidDepot);
 * 
 * BuildingController.Instance.SpawnStructure(Enums.StructureType.WasteDepot);
 * 
 * GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.CateringTruck, Enums.VehicleSubType.Unspecified);
 * GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.AircraftCabinCleaningTruck, Enums.VehicleSubType.Unspecified);
 * GameObject gameObject4 = TrafficController.Instance.SpawnVehicleGameObject(Enums.VehicleType.DeicingTruck, Enums.VehicleSubType.Unspecified);
                ServiceVehicleController component3 = gameObject4.GetComponent<ServiceVehicleController>();
                component3.Initialize();
                component3.ServiceVehicleModel.isOwnedByAirport = true;
                Singleton<TrafficController>.Instance.AddVehicleToSpawnQueue(component3, false);
                gameObject4.DumpFields();
*/


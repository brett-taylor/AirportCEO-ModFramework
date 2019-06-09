using System;
using ACMF.ModHelper.ModPrefabs.Procurments;
using UnityEngine;

namespace SampleModVehicle
{
    public class TestTruckProcurment : ACMFProcurmentTemplate
    {
        public static readonly string ProcureableProductTypeEnumNameGlobal = "SampleModVehicleTestTruck";

        public override string ProcureableProductTypeEnumName => ProcureableProductTypeEnumNameGlobal;
        public override string Title => "Test Truck";
        public override Enums.ProcureableProductCategory Category => Enums.ProcureableProductCategory.Vehicles;
        public override Enums.ProcureableProductSubCategory SubCategory => Enums.ProcureableProductSubCategory.ServiceVehicles;
        public override string Description => "Cool Test Truck that does nothing because the job system is hard to understand";
        public override string Requirement => "Some Cool Requiremnts";
        public override PrerequisiteContainer[] Prequisite => new PrerequisiteContainer[0];
        public override ProcurementController.Prerequisite[] PrerequisiteForDisplay => new ProcurementController.Prerequisite[0];
        public override float FixedCost => 100f;
        public override float OperatingCost => 5f;
        public override TimeSpan DeliveryTime => new TimeSpan(0, 10, 0);
        public override bool IsQuantifiable => true;
        public override bool IsPhysicalProduct => true;
        public override Sprite Sprite => DataPlaceholder.Instance.procureableProductSprites[1];

        public override void SpawnProcureable()
        {
            GameObject testCar = TestTruckCreator.Instance.CreateInstance();
            TestTruckController scc = testCar.GetComponent<TestTruckController>();
            scc.Initialize();
            scc.ServiceVehicleModel.isOwnedByAirport = true;
            TrafficController.Instance.AddVehicleToSpawnQueue(scc, false);
        }
    }
}

using System;
using ACMF.ModHelper.ModPrefabs.Procurments;

namespace SampleModVehicle
{
    public class TestTruckProcurment : ProcurmentTemplate
    {
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
    }
}

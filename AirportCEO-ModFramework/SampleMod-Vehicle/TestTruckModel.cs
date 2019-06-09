using System.Collections.Generic;
using ACMF.ModHelper.EnumPatcher;
using ACMF.ModHelper.ModPrefabs.Vehicles;
using UnityEngine;

namespace SampleModVehicle
{
    public class TestTruckModel : ACMFServiceVehicleModel
    {
        public static readonly string VehicleTypeEnumNameGlobal = "SampleModVehicleTestTruck";
        public static Enums.VehicleType TestTruckVehicleType { get; private set; }

        public override string VehicleTypeEnumName => VehicleTypeEnumNameGlobal;
        public override Enums.VehicleType VehicleTypeEnum => TestTruckVehicleType;
        public override string VehicleName => "Test Truck " + Utils.RandomRangeI(0f, 999f);
        public override Vector2[] VehicleHitboxes => new Vector2[] { new Vector2(6f, 2f) };
        public override Enums.FuelType FuelType => Enums.FuelType.Gasoline;
        public override float TopSpeed => 15f;
        public override float OperatingCost => 100f;
        public override bool RandomizeColor => true;
        public override float MaxQuantityLoadedGoods => 4f;
        public override List<Enums.DeliveryContainerType> AllowedDeliveryContainers => new List<Enums.DeliveryContainerType> { Enums.DeliveryContainerType.Seat };
        public override float AcceptedTargetDistance => GridController.nodeDiameter;
        public override float AcceptedTargetDistanceTurn => 0.5f;
        public override int NodesToOccupy => 2;
        public override bool CanBeAssignedToStaticJobTaskObject => true;
        public override float ConditionReductionRate => 0.05f;

        public override void PatchedVehicleTypeEnum(Enums.VehicleType vehicleType)
        {
            TestTruckVehicleType = vehicleType;
        }

        public override void InitializeServiceVehicle()
        {
        }

        public override void ResetServiceVehicle()
        {
        }
    }
}

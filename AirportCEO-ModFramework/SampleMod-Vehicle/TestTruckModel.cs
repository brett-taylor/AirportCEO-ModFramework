using System;
using UnityEngine;

namespace SampleModVehicle
{
    [Serializable]
    public class TestTruckModel : ServiceVehicleModel
    {
        public int randomNumberOne;

        public new void Initialize()
        {
            base.Initialize();
            vehicleName = "Test Car " + Utils.RandomRangeI(0f, 999f);
            vehicleHitboxes = new Vector2[] { new Vector2(6f, 2f) };
            vehicleType = EntryPoint.VehicleType;
            fuelType = Enums.FuelType.Gasoline;
            topSpeed = 20f;
            operatingCost = 100f;
            randomizeColor = false;
            maxDeliveryQuantityCapacity = 0f;
            CurrentQuantityDelivered = 0f;
            allowedDeliveryContainer = Enums.DeliveryContainerType.Seat;
            acceptedTargetDistance = GridController.nodeDiameter;
            acceptedTargetDistanceTurn = 0.5f;
            nodesToOccupy = 2;
            occupyBackwards = false;
            canBeAssignedToStaticJobTaskObject = true;

            randomNumberOne = 5;
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}

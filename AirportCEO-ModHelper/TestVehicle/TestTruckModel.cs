using UnityEngine;

namespace TestVehicle
{
    public class TestTruckModel : ServiceVehicleModel
    {
        public new void Initialize()
        {
            base.Initialize();
            vehicleName = "Sausy Test Car " + Utils.RandomRangeI(0f, 999f);
            vehicleHitboxes = new Vector2[] { new Vector2(4f, 2f) };
            vehicleType = Enums.VehicleType.ServiceCar;
            fuelType = Enums.FuelType.Gasoline;
            topSpeed = 50f;
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
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}

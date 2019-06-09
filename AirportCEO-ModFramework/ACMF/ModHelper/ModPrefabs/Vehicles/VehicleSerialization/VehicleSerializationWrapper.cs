using OdinSerializer;
using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Vehicles.VehicleSerialization
{
    internal class VehicleSerializationWrapper
    {
        [OdinSerialize]
        private readonly List<Tuple<VehicleModel, Type>> VehicleModels = new List<Tuple<VehicleModel, Type>>();

        internal void Add(VehicleController vehicleController)
        {
            VehicleModels.Add(new Tuple<VehicleModel, Type>(vehicleController.model, vehicleController.GetType()));
        }

        internal IEnumerable<Tuple<VehicleModel, Type>> GetVehicles()
        {
            return VehicleModels;
        }
    }
}

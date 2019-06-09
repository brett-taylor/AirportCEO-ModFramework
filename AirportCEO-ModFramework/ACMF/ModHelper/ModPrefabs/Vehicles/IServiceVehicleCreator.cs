using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Vehicles
{
    internal interface IServiceVehicleCreator
    {
        GameObject CreateNewInstance();
        GameObject CreateForDeserialization(VehicleModel vehicleModel);
    }
}

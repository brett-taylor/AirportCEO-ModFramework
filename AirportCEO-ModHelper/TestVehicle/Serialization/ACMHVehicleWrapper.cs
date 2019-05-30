using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestVehicle.Serialization
{
    [Serializable]
    public class ACMHVehicleWrapper
    {
        [SerializeField]
        public List<string> VehicleTypes;

        [SerializeField]
        public List<VehicleModel> VehicleModels;

        public ACMHVehicleWrapper()
        {
            VehicleTypes = new List<string>();
            VehicleModels = new List<VehicleModel>();
        }

        public void AddNewObject(Type typeInstance, object instance)
        {
            VehicleTypes.Add(instance.GetType().FullName);
            VehicleModels.Add((VehicleModel) instance);
        }

        public static ACMHVehicleWrapper CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ACMHVehicleWrapper>(jsonString);
        }
    }
}

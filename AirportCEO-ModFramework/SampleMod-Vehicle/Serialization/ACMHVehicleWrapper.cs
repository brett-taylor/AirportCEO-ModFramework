using System;
using System.Collections.Generic;
using UnityEngine;

namespace SampleModVehicle.Serialization
{
    [Serializable]
    public class ACMHVehicleWrapper
    {
        public List<TestTruckModel> VehicleModels = new List<TestTruckModel>();

        public void AddNewObject(TestTruckModel testTruckModel) => VehicleModels.Add(testTruckModel);
        public string SaveToJSON() => JsonUtility.ToJson(this);
        public static ACMHVehicleWrapper CreateFromJSON(string jsonString) => JsonUtility.FromJson<ACMHVehicleWrapper>(jsonString);
    }
}

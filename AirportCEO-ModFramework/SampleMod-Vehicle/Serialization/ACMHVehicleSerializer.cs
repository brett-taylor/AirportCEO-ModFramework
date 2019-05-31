using UnityEngine;

namespace SampleModVehicle.Serialization
{
    [Harmony.HarmonyPatch(typeof(Serializers.VehicleSerializer))]
    [Harmony.HarmonyPatch("SerializeVehicles")]
    public static class VehicleSerializerSerializePatcher
    {
        [Harmony.HarmonyPostfix]
        public static void Postfix(bool debugPrints, string savePath)
        {
            ACMHVehicleSerializer.SerializeVehicles(savePath);
        }
    }

    [Harmony.HarmonyPatch(typeof(Serializers.VehicleSerializer))]
    [Harmony.HarmonyPatch("DeserializeVehicles")]
    public static class VehicleDeserializerSerializePatcher
    {
        [Harmony.HarmonyPostfix]
        public static void Postfix(bool debugPrints, string savePath)
        {
            ACMHVehicleSerializer.DeserializeVehicles(savePath);
        }
    }

    public static class ACMHVehicleSerializer
    {
        public static void SerializeVehicles(string savePath)
        {
            VehicleController[] vehicleArray = Singleton<TrafficController>.Instance.GetVehicleArray();
            TrailerController[] trailersArray = Singleton<TrafficController>.Instance.GetTrailersArray();
            ACMHVehicleWrapper vehicleWrapper = new ACMHVehicleWrapper();

            foreach (VehicleController vehicleController in vehicleArray)
            {
                if (!(vehicleController == null) && vehicleController.GetModel<VehicleModel>() != null)
                {
                    if (vehicleController is TestTruckController)
                    {
                        vehicleController.SetVehicleForSerialization();
                        vehicleWrapper.VehicleModels.Add(vehicleController.GetComponent<TestTruckController>().TestTruckModel);
                    }
                }
            }

            byte[] bytes = OdinSerializer.SerializationUtility.SerializeValue(vehicleWrapper, OdinSerializer.DataFormat.JSON);
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            if (!Utils.WriteFileAsJson(json, savePath + "/ModdedVehicleData.json"))
            {
                ACMF.ModHelper.Utilities.Logger.ShowDialog("ERROR! when writing save file to: " + savePath + "/ModdedVehicleData.json");
            }
        }

        public static void DeserializeVehicles(string savePath)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Utils.ReadFileToJson(savePath + "/ModdedVehicleData.json"));
            ACMHVehicleWrapper vehicleWrapper = OdinSerializer.SerializationUtility.DeserializeValue<ACMHVehicleWrapper>(bytes, OdinSerializer.DataFormat.JSON);
            for (int i = 0; i < vehicleWrapper.VehicleModels.Count; i++)
            {
                if (vehicleWrapper.VehicleModels[i] is TestTruckModel)
                {
                    GameObject gameObject = Assets.GetGameObjectForTestTruck();
                    TestTruckController component = gameObject.GetComponent<TestTruckController>();
                    component.Initialize();
                    TestTruckModel vm = vehicleWrapper.VehicleModels[i] as TestTruckModel;
                    component.RestoreVehicleFromSerialization(vm);
                    TrafficController.Instance.AddVehicleToList(component);
                    component.Launch();
                }
            }
        }
    }
}

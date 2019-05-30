
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
                        vehicleWrapper.AddNewObject(vehicleController.GetComponent<TestTruckController>().TestTruckModel);
                    }
                }
            }

            
            if (!Utils.WriteFileAsJson(JsonUtility.ToJson(vehicleWrapper, true), savePath + "/ModdedVehicleData.json"))
            {
                DialogPanel.Instance.ShowMessagePanel("Error when writing save file to: " + savePath + "/ModdedVehicleData.json", false);
            }
        }

        public static void DeserializeVehicles(string savePath)
        {
            /*ACMHVehicleWrapper vehicleWrapper = JsonUtility.FromJson<ACMHVehicleWrapper>(Utils.ReadFileToJson(savePath + "/ModdedVehicleData.json"));
            for (int i = 0; i < vehicleWrapper.VehicleTypes.Count; i++)
            {
                if (vehicleWrapper.VehicleTypes[i].Equals(typeof(TestTruckModel).FullName) == true)
                {
                    GameObject gameObject = Assets.GetGameObjectForTestTruck();
                    TestTruckController component = gameObject.GetComponent<TestTruckController>();
                    component.Initialize();
                    TestTruckModel vm = vehicleWrapper.VehicleModels[i] as TestTruckModel;
                    System.Console.WriteLine($"222 vehicleModel {vm?.vehicleName ?? "null"}");
                    component.RestoreVehicleFromSerialization(vm);
                    TrafficController.Instance.AddVehicleToList(component);
                    component.Launch();
                }
            }*/
        }
    }

    [Harmony.HarmonyPatch(typeof(VehicleController))]
    [Harmony.HarmonyPatch("RestoreVehicleFromSerialization")]
    public class RestoreVehicleFromSerializationPatcher
    {
        [Harmony.HarmonyPrefix]
        public static void Prefix(VehicleController __instance, VehicleModel vehicleModel)
        {
            if (__instance is TestTruckController)
                System.Console.WriteLine($"111 vehicleModel {vehicleModel?.vehicleName ?? "null"}");
        }
    }
}

using System;
using System.IO;

namespace ACMF.ModHelper.ModPrefabs.Vehicles.VehicleSerialization
{
    public static class VehicleSerializer
    {
        private static readonly string MODDED_VEHICLES_SAVE_FILE_NAME = "/ModdedVehicleData.json";

        public static void SerializeVehicles(string savePath)
        {
            VehicleController[] vehicleArray = Singleton<TrafficController>.Instance.GetVehicleArray();
            TrailerController[] trailersArray = Singleton<TrafficController>.Instance.GetTrailersArray();
            VehicleSerializationWrapper vehicleSerializationWrapper = new VehicleSerializationWrapper();

            foreach (VehicleController vehicleController in vehicleArray)
            {
                if (ServiceVehicleCreator.HasCreatorFromType(vehicleController.GetType()) == false)
                    continue;

                Utilities.Logger.Print($"Modded Vehicles Serializing {vehicleController}...");
                vehicleController.SetVehicleForSerialization();
                vehicleSerializationWrapper.Add(vehicleController);
            }

            bool result = Utilities.JsonSerialization.Serialize(vehicleSerializationWrapper, savePath + MODDED_VEHICLES_SAVE_FILE_NAME);
            if (result == false)
                DialogPopup.DialogManager.QueueMessagePanel($"A error occured while trying to serialize modded vehicles. Please check the output log to see why.");

            Utilities.Logger.Print($"Modded Vehicles Serialization successful.");
        }

        public static void DeserializeVehicles(string savePath)
        {
            if (File.Exists(savePath + MODDED_VEHICLES_SAVE_FILE_NAME) == false)
                return;

            bool result = Utilities.JsonSerialization.Deserialize(out VehicleSerializationWrapper vehicleSerializationWrapper, savePath + MODDED_VEHICLES_SAVE_FILE_NAME);
            if (result == false)
                DialogPopup.DialogManager.QueueMessagePanel($"A error occured while trying to deserialize modded vehicles. Please check the output log to see why.");

            foreach (Tuple<VehicleModel, Type> tuple in vehicleSerializationWrapper.GetVehicles())
            {
                if (ServiceVehicleCreator.HasCreatorFromType(tuple.Item2) == false)
                    continue;

                Utilities.Logger.Print($"Modded Vehicles Deserializing {tuple}...");
                ServiceVehicleCreator.GetCreatorFromType(tuple.Item2).CreateForDeserialization(tuple.Item1);
            }

            Utilities.Logger.Print($"Modded Vehicles Deserialization successful.");
        }
    }
}

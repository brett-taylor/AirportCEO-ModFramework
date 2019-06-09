using Harmony;

namespace ACMF.ModHelper.ModPrefabs.Vehicles.VehicleSerialization
{
    public static class VehicleSerializationPatcher
    {
        [HarmonyPatch(typeof(Serializers.VehicleSerializer))]
        [HarmonyPatch("SerializeVehicles")]
        public static class VehicleSerializerSerializePatcher
        {
            [HarmonyPostfix]
            public static void Postfix(bool debugPrints, string savePath)
            {
                VehicleSerializer.SerializeVehicles(savePath);
            }
        }

        [HarmonyPatch(typeof(Serializers.VehicleSerializer))]
        [HarmonyPatch("DeserializeVehicles")]
        public static class VehicleDeserializerSerializePatcher
        {
            [HarmonyPostfix]
            public static void Postfix(bool debugPrints, string savePath)
            {
                VehicleSerializer.DeserializeVehicles(savePath);
            }
        }
    }
}

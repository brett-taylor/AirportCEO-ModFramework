using ACMF.ModHelper.Utilities.Misc;
using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System.Reflection;
using UnityEngine;

namespace SampleModPlane
{
    [ACMFMod(id: "TestPlaneTestBed", name: "Test Plane Test Bed", modVersion: "1.0.0", requiredACMLVersion: "0.1.0")]
    public class EntryPoint
    {
        public static HarmonyInstance HarmonyInstance { get; set; }
        public static Mod Mod { get; set; }

        [ACMFModEntryPoint]
        public static void Entry(Mod mod)
        {
            Mod = mod;
            HarmonyInstance = HarmonyInstance.Create(Mod.ModInfo.ID);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(AirTrafficController))]
    [HarmonyPatch("Awake")]
    public class ATCDumper
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            GameObject[] aircraftPrefabs = Singleton<AirTrafficController>.instance.aircraftPrefabs;
            ACMF.ModHelper.Utilities.Logger.ShowDialog("" + aircraftPrefabs.Length);
            for (int i = 0; i < aircraftPrefabs.Length; i++)
            {
                UnityObjectDumpFields.DumpFields(aircraftPrefabs[i]);
            }
        }
    }
}

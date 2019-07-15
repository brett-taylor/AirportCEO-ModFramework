using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System.Reflection;
using UnityEngine;

namespace SampleModNewFloors
{
    [ACMFMod(id: "ACMF.SampleMod.NewFloors", name: "Sample-Mod-New-Floors", modVersion: "1.0.0", requiredACMLVersion: "0.0.0")]
    public class EntryPoint
    {
        public static HarmonyInstance HarmonyInstance { get; private set; }
        public static Mod Mod { get; private set; }

        [ACMFModEntryPoint]
        public static void Entry(Mod mod)
        {
            Mod = mod;
            HarmonyInstance = HarmonyInstance.Create(Mod.ModInfo.ID);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}

using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SampleJobsMod
{
    [ACMFMod(id: "Jobs.Mod", name: "Jobs Mod", modVersion: "1.0.0", requiredACMLVersion: "0.1.0")]
    public class EntryPoint
    {
        public static Dictionary<Type, List<Enums.ServiceVehicleAction>> reassignedActions;
        public static HarmonyInstance HarmonyInstance { get; private set; }
        public static Mod Mod { get; private set; }

        [ACMFModEntryPoint]
        public static void Entry(Mod mod)
        {
            Mod = mod;
            HarmonyInstance = HarmonyInstance.Create(Mod.ModInfo.ID);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            reassignedActions = new Dictionary<Type, List<Enums.ServiceVehicleAction>>();
            //ACMF.ModHelper.EnumPatcher.EnumCache<Enums.ServiceVehicleActivity>.Instance.Patch("RunAround");
        }
    }
}

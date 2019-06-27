using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System.Reflection;
using UnityEngine;
using ACMF.ModHelper.ModPrefabs.Buildings;

namespace SampleModTerminaltem
{
    [ACMFMod(id: "ACMF.SampleMod.TerminalItem", name: "Sample-Mod-Terminal-Item", modVersion: "1.0.0", requiredACMLVersion: "0.0.0")]
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

    [HarmonyPatch(typeof(PlayerInputController))]
    [HarmonyPatch("Update")]
    public class PlayerInputControllerUpdatePatcher
    {
        public static void Postfix()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                BuildingController.Instance.SpawnItem(ActiveBuildingCreators.GetCreator<LargeDesk>().ItemTypeEnum);
            }
        }
    }
}

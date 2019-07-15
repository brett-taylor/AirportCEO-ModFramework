using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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

    [HarmonyPatch(typeof(PlayerInputController))]
    [HarmonyPatch("Update")]
    public class PlayerInputControllerUpdatePatcher
    {
        public static void Postfix()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                foreach(var t in DataPlaceholderMaterials.Instance.floorBundles)
                {
                    System.Console.WriteLine($"{t}");
                    System.Console.WriteLine($"1: {t.floorSprite.name}");
                    System.Console.WriteLine($"2: {t.floorSmall.name}");
                    System.Console.WriteLine($"3: {t.floorLarge.name}");
                }
            }
        }
    }
}

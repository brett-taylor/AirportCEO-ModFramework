using Harmony;
using UnityEngine;
using UnityEngine.UI;
using ACMF.ModLoader;

namespace ACMF.ModHelper.MainMenu
{
    [HarmonyPatch(typeof(NativeModsPanel))]
    [HarmonyPatch("UpdateModLists")]
    public class ModMenuPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(NativeModsPanel __instance)
        {
            __instance.transform.Find("SteamMods/SteamHeaderText").GetComponent<Text>().text = "MODS";
            foreach (Mod mod in ModLoader.ModLoader.ModsFound.Values)
            {
                GameObject modView = Object.Instantiate(__instance.workshopContainer, __instance.workshopModsPanel);
                ModMenuCustomMod modMenuCustomMod = modView.AddComponent<ModMenuCustomMod>();
                modMenuCustomMod.SetMod(mod);
            }
        }
    }
}

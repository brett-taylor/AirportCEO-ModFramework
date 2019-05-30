using Harmony;
using UnityEngine;

namespace ACMF.ModHelper.MainMenu
{
    [HarmonyPatch(typeof(MainMenuWorldController))]
    [HarmonyPatch("Start")]
    public class SkipMainMenu
    {
        private static readonly bool SKIP_MAIN_MENU = false;
        private static readonly string SAVE = @"C:\Users\Brett\AppData\Roaming/Apoapsis Studios/Airport CEO\Saves/biggin hill";

        [HarmonyPostfix]
        public static void Postfix(MainMenuWorldController __instance)
        {
            if (SKIP_MAIN_MENU)
            {
                GameObject go = GameObject.Find("LoadedFromGameWorld");
                if (go == null)
                    __instance.StartCoroutine(__instance.LaunchAirportCoroutine(Enums.GameLoadSetting.ContinueGame, SAVE));
            }
        }
    }
}

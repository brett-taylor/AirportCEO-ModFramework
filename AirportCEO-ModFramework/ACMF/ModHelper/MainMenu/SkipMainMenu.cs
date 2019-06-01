using Harmony;
using UnityEngine;

namespace ACMF.ModHelper.MainMenu
{
    [HarmonyPatch(typeof(MainMenuWorldController))]
    [HarmonyPatch("Start")]
    public class SkipMainMenu
    {
        [HarmonyPostfix]
        public static void Postfix(MainMenuWorldController __instance)
        {
            if (ACMF.Config.ENABLE_INSTANT_LOAD_INTO_SAVE_GAME)
            {
                GameObject go = GameObject.Find("LoadedFromGameWorld");
                if (go == null)
                    __instance.StartCoroutine(__instance.LaunchAirportCoroutine(Enums.GameLoadSetting.ContinueGame, ACMF.Config.INSTANT_LOAD_INTO_SAVE_GAME_FILE));
            }
        }
    }
}

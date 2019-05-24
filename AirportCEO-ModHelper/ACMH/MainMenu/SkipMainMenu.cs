using Harmony;

namespace ACMH.MainMenu
{
    [HarmonyPatch(typeof(MainMenuWorldController))]
    [HarmonyPatch("Start")]
    public class SkipMainMenu
    {
        private static readonly bool SKIP_MAIN_MENU = true;
        private static readonly string SAVE = @"C:\Users\Brett\AppData\Roaming/Apoapsis Studios/Airport CEO\Saves/biggin hill";

        [HarmonyPostfix]
        public static void Postfix(MainMenuWorldController __instance)
        {
            if (SKIP_MAIN_MENU)
            {
                __instance.StartCoroutine(__instance.LaunchAirportCoroutine(Enums.GameLoadSetting.ContinueGame, SAVE));
            }
        }
    }
}
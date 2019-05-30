using Harmony;
using TMPro;
using UnityEngine;

namespace ACMF.ModHelper.MainMenu
{
    [HarmonyPatch(typeof(MainMenuWorldController))]
    [HarmonyPatch("ShowHideGameMenuPanels")]
    public class ShowVersionOnMenuPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(MainMenuWorldController __instance)
        {
            GameObject versionText = Object.Instantiate(Utilities.Assets.MAIN_MENU_VERSION_TEXT);
            versionText.transform.Find("Container/ACML").GetComponent<TextMeshProUGUI>().text = $"Airport CEO Mod Loader: v{ACMF.Version}";
        }
    }
}

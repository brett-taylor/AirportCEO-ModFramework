using ACMF.ModHelper.AssetBundles.Impl;
using Harmony;
using TMPro;
using UnityEngine;

namespace ACMF.ModHelper.MainMenu
{
    [HarmonyPatch(typeof(MainMenuWorldController))]
    [HarmonyPatch("ShowHideGameMenuPanels")]
    public class MainMenuShowHideGameMenuPanelsPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(MainMenuWorldController __instance)
        {
            GameObject versionText = Object.Instantiate(ACMFAssets.Instance.AttemptLoadGameObject("ACMF-Info"));
            versionText.transform.Find("Container/ACMF").GetComponent<TextMeshProUGUI>().text = $"Airport CEO Mod Framework: v{ACMF.Version}";

            DialogPopup.DialogManager.ShowNextIfNoPopupCurrently();
        }
    }
}

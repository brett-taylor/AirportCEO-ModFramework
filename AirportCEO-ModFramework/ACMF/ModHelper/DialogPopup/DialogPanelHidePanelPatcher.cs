using Harmony;

namespace ACMF.ModHelper.DialogPopup
{
    [HarmonyPatch(typeof(DialogPanel))]
    [HarmonyPatch("HidePanel")]
    public class DialogPanelHidePanelPatcher
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            DialogManager.ShowNext();
        }
    }
}

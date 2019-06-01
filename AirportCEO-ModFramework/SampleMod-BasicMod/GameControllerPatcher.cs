using Harmony;
using UnityEngine;

namespace SampleModBasicMod
{
    [HarmonyPatch(typeof(GameController))]
    [HarmonyPatch("Update")]
    public class GameControllerPatcher
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (EntryPoint.Config.ALLOW_GAME_CONTROLLER_PATCH && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
            {
                ACMF.ModHelper.Utilities.Logger.ShowDialog("Working Postfix Onto GameController::Update");
                ACMF.ModHelper.Utilities.Logger.ShowNotification("Working Postfix Onto GameController::Update");
            }
        }
    }
}

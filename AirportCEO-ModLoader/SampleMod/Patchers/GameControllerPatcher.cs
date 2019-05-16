﻿using UnityEngine;
using Harmony;

namespace SampleMod.Patchers
{
    [HarmonyPatch(typeof(GameController))]
    [HarmonyPatch("Update")]
    public class GameControllerPatcher
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
            {
                DialogPanel.Instance.ShowMessagePanel("Working Postfix Onto GameController::Update");
                NotificationController.Instance.SendUINotification("Working Postfix Onto GameController::Update", Enums.NotificationType.Other, Enums.MessageSeverity.High, "", true, true);
            }           
        }
    }
}

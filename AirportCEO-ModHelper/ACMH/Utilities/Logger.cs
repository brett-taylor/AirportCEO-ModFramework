using System;

namespace ACMH.Utilities
{
    public class Logger
    {
        public static void Print(string message, bool alertInGame = false)
        {
            Console.WriteLine($"[AirportCEOModHelper] {message}");

            if (alertInGame)
                ShowNotification(message);
        }

        public static void Error(string message, bool alertInGame = false)
        {
            Print($"[ERROR] {message}", alertInGame);
        }

        public static void ShowDialog(string message)
        {
            if (DialogPanel.Instance != null)
                DialogPanel.Instance.ShowMessagePanel(message);
        }

        public static void ShowNotification(string message)
        {
            if (NotificationController.Instance != null)
            {
                NotificationController.Instance.SendUINotification(message, Enums.NotificationType.Other, Enums.MessageSeverity.Unspecified, "", true, true);
            }
        }
    }
}
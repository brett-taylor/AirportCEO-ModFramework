using System;

namespace ACMF.ModHelper.Utilities
{
    public class Logger
    {
        internal static void Print(string message, bool alertInGame = false)
        {
            Console.WriteLine($"[AirportCEOModHelper] {message}");

            if (alertInGame)
                ShowNotification(message);
        }

        internal static void Error(string message, bool alertInGame = false) => Print($"[ERROR] {message}", alertInGame);

        public static void ShowDialog(string message)
        {
            if (DialogPanel.Instance != null)
                DialogPanel.Instance.ShowMessagePanel(message);

            Console.WriteLine($"[Logger::ShowDialog] {message}");
        }

        public static void ShowNotification(string message)
        {
            if (NotificationController.Instance != null)
                NotificationController.Instance.SendUINotification(message, Enums.NotificationType.Other, Enums.MessageSeverity.Unspecified, "", true, true);

            Console.WriteLine($"[Logger::ShowNotification] {message}");
        }
    }
}

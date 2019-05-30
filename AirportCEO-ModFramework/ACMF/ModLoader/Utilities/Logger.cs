using System;

namespace ACMF.ModLoader.Utilities
{
    public static class Logger
    {
        public static void Print(string message) => Console.WriteLine($"[AirportCEOModFramework] {message}");
        public static void Error(string message, bool alertInGame = false) => Print($"[ERROR] {message}");
    }
}

using System;

namespace ACML.ModLoader.Utilities
{
    public static class Logger
    {
        internal static void Print(string message)
        {
            Console.WriteLine($"[AirportCEOModLoader] {message}");
        }

        internal static void Error(string message)
        {
            Print($"[ERROR] {message}");
        }
    }
}

using System;

namespace ACML.ModLoader.Utilities
{
    public static class Logger
    {
        public static void Print(string message)
        {
            Console.WriteLine($"[AirportCEOModLoader] {message}");
        }

        public static void Error(string message)
        {
            Print($"[ERROR] {message}");
        }
    }
}

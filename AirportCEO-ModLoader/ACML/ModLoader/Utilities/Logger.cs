using System;

namespace ACML.ModLoader.Utilities
{
    public static class Logger
    {
        private static readonly string Name = "AirportCEOModLoader";

        public static void Print(string message)
        {
            System.Console.WriteLine($"[AirportCEOModLoader] {message}");
        }

        public static void Error(string message)
        {
            Print($"[ERROR] {message}");
        }
    }
}

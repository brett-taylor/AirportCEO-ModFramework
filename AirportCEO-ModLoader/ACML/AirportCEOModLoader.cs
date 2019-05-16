using ACML.ModLoader;
using ACML.ModLoader.Utilities;
using System.Reflection;

namespace ACML
{
    public class AirportCEOModLoader
    {
        public static readonly ModVersion ModLoaderVersion = new ModVersion(1, 0, 0);

        public static void Entry()
        {
            Logger.Print($"Started AirportCEOModLoader version {Assembly.GetExecutingAssembly().GetName().Version}");
            Logger.Print("Initialising ModLoader...");
            ModLoader.ModLoader.Initialise();

            Logger.Print($"1: {new ModVersion(25, 25, 25) == new ModVersion(25, 25, 25) == true}");
            Logger.Print($"2: {new ModVersion(25, 25, 25) != new ModVersion(1, 0, 0) == true}");
            Logger.Print($"3: {new ModVersion(1, 0, 1) >= new ModVersion(1, 0, 0) == true}");
            Logger.Print($"4: {new ModVersion(0, 0, 9) <= new ModVersion(1, 0, 0) == true}");
            Logger.Print($"5: {new ModVersion(1, 0, 0) > new ModVersion(0, 0, 9) == true}");
            Logger.Print($"6: {new ModVersion(1, 0, 0) < new ModVersion(1, 0, 1) == true}");
        }
    }
}

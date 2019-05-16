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
        }
    }
}

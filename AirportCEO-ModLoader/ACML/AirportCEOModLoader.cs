using ACML.ModLoader.Utilities;
using System.Reflection;

namespace ACML
{
    public class AirportCEOModLoader
    {
        public static void Entry()
        {
            Logger.Print($"Started AirportCEOModLoader version {Assembly.GetExecutingAssembly().GetName().Version}");
            Logger.Print("Initialising ModHelper...");
            ModHelper.ModHelper.Initialise();
            Logger.Print("Initialising ModLoader...");
            ModLoader.ModLoader.Initialise();
        }
    }
}

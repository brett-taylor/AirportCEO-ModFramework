using ACMF.ModLoader;
using ACMF.ModLoader.Utilities;
using Harmony;
using System.IO;
using System.Reflection;

namespace ACMF
{
    public class ACMF
    {
        public static readonly ModVersion Version = new ModVersion(0, 1, 0);
        public static readonly string HarmonyDLLFileName = "0Harmony.dll";
        public static readonly string ACMFFolderLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static bool Initialised = false;

        public static void Entry()
        {
            if (Initialised)
                return;

            Initialised = true;
            Logger.Print($"ACMF Directory: {ACMFFolderLocation}");
            string harmonyDirectory = Path.Combine(ACMFFolderLocation, HarmonyDLLFileName);
            Logger.Print($"Loading Harmony from {harmonyDirectory}");
            Assembly assembly = Assembly.LoadFrom(harmonyDirectory);

            Logger.Print($"Loading ModHelper...");
            ModHelper.Utilities.Assets.Initialise();
            HarmonyInstance harmonyInstance = HarmonyInstance.Create("AirportCEOModFramework");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Print($"Initialising ModLoader");
            ModLoader.ModLoader.Initialise();
        }
    }
}

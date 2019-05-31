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
        public static readonly string ACMFFolderLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static bool Initialised = false;

        public static void Entry()
        {
            if (Initialised)
                return;

            Initialised = true;
            Logger.Print($"ACMF Directory: {ACMFFolderLocation}");
            Logger.Print($"Loading all DLLS in {ACMFFolderLocation}");

            string[] dllsToLoad = Directory.GetFiles(ACMFFolderLocation, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string dll in dllsToLoad)
            {
                if (Path.GetFileName(dll).Equals("ACMF.dll"))
                    continue;

                Logger.Print($"Loading DLL: {Path.GetFileName(dll)}");
                Assembly.LoadFrom(dll);
            }

            Logger.Print($"Loading ModHelper...");
            ModHelper.Utilities.Assets.Initialise();
            HarmonyInstance harmonyInstance = HarmonyInstance.Create("AirportCEOModFramework");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Print($"Initialising ModLoader");
            ModLoader.ModLoader.Initialise();
        }
    }
}

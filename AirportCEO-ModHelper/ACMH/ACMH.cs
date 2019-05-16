using ACML.ModLoader;
using ACML.ModLoader.Attributes;
using Harmony;
using System.Reflection;

namespace ACMH
{
    [ACMLMod(id: "AirportCEOModHelper", name: "ACMH - Mod Helper", modVersion: "0.1.0", requiredACMLVersion: "0.1.0")]
    public class ACMH
    {
        public static HarmonyInstance HarmonyInstance { get; private set; }
        public static Mod Mod { get; private set; }

        [ACMLModEntryPoint]
        public static void EntryPoint(Mod mod)
        {
            Mod = mod;
            Utilities.Assets.Initialise();
            HarmonyInstance = HarmonyInstance.Create("AirportCEOModHelper");
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}

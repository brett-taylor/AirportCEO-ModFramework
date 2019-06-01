using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System.Reflection;

namespace SampleModBasicMod
{
    [ACMFMod(id: "Hanks.Tom.SampleMod", name: "Sample Mod", modVersion: "12.3.5", requiredACMLVersion: "0.1.0")]
    public class EntryPoint
    {
        public static Config Config { get; private set; } = null;

        [ACMFModEntryPoint]
        public static void Entry(Mod mod)
        {
            System.Console.WriteLine($"[{mod.ModInfo.ID}] {mod.ModInfo.Name} executing.");
            Config = ACMF.ModHelper.Config.ACMFConfigManager.LoadConfig<Config>(mod);
            HarmonyInstance harmony = HarmonyInstance.Create(mod.ModInfo.ID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}

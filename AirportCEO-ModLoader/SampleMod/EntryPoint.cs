using ACML.ModLoader;
using ACML.ModLoader.Attributes;
using Harmony;
using System.Reflection;

namespace SampleMod
{
    [ACMLMod(id: "Hanks.Tom.SampleMod", name: "Sample Mod", modVersion: "12.3.5", requiredACMLVersion: "1.0.0")]
    public class EntryPoint
    {
        [ACMLModEntryPoint]
        public static void Entry()
        {
            System.Console.WriteLine("[SampleMod] Sample Mod executing.");
            var harmony = HarmonyInstance.Create("com.github.harmony.rimworld.mod.example");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}

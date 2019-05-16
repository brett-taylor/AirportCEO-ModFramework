using ACML.ModLoader.Attributes;
using System.Reflection;

namespace ACML.ModLoader
{
    public class Mod
    {
        public ACMLMod ModInfo { get; private set; }
        public Assembly Assembly { get; private set; }
        public MethodInfo EntryPoint { get; private set; }
        public ModVersion ModVersion { get; private set; }
        public ModVersion RequiredACMLVersion { get; private set; }
        public ModLoadFailure ModLoadFailure { get; private set; } = ModLoadFailure.UNKNOWN;

        public Mod(ACMLMod modInfo, Assembly assembly, MethodInfo entryPoint)
        {
            ModInfo = modInfo;
            Assembly = assembly;
            EntryPoint = entryPoint;
            ModVersion = ModVersion.Parse(modInfo.ModVersion);
            RequiredACMLVersion = ModVersion.Parse(modInfo.RequiredACMLVersion);
        }

        public void CalculateIfShouldLoad()
        {
            if (RequiredACMLVersion > AirportCEOModLoader.ModLoaderVersion)
            {
                ModLoadFailure = ModLoadFailure.REQUIRES_NEWER_VERSION_OF_ACML;
                return;
            }

            ModLoadFailure = ModLoadFailure.SUCCESS;
        }

        public bool ShouldLoad()
        {
            return ModLoadFailure == ModLoadFailure.SUCCESS;
        }
    }
}

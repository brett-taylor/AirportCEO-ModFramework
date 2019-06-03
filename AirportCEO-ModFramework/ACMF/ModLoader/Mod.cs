using System.Collections.Generic;
using ACMF.ModLoader.Attributes;
using System.Reflection;

namespace ACMF.ModLoader
{
    public class Mod
    {
        public ACMFMod ModInfo { get; private set; }
        public Assembly Assembly { get; private set; }
        public MethodInfo EntryPoint { get; private set; }
        public ModVersion ModVersion { get; private set; }
        public ModVersion RequiredACMFVersion { get; private set; }
        public ModLoadFailure ModLoadFailure { get; set; }

        public Mod(ACMFMod modInfo, Assembly assembly, MethodInfo entryPoint)
        {
            ModInfo = modInfo;
            Assembly = assembly;
            EntryPoint = entryPoint;
            ModVersion = ModVersion.Parse(modInfo.ModVersion);
            RequiredACMFVersion = ModVersion.Parse(modInfo.RequiredACMFVersion);
            ModLoadFailure = ModLoadFailure.UNKNOWN;
        }

        public bool IsOnCorrectVersionOFACMF()
        {
            return ACMF.Version >= RequiredACMFVersion;
        }

        public List<string> GetMissingDependencies()
        {
            List<string> missingDependencies = new List<string>();
            foreach (string requiredMod in ModInfo.RequiredMods)
                if (ModLoader.ModsLoaded.ContainsKey(requiredMod) == false)
                    missingDependencies.Add(requiredMod);

            return missingDependencies;
        }

        public string MissingDependencies()
        {
            return string.Join(", ", GetMissingDependencies());
        }
    }
}

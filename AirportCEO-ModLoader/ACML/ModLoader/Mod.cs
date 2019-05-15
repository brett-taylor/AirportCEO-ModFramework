using ACML.ModLoader.Attributes;
using System.Reflection;

namespace ACML.ModLoader
{
    public class Mod
    {
        public ACMLMod ModInfo { get; private set; }
        public Assembly Assembly { get; private set; }
        public MethodInfo EntryPoint { get; private set; }

        public Mod(ACMLMod modInfo, Assembly assembly, MethodInfo entryPoint)
        {
            ModInfo = modInfo;
            Assembly = assembly;
            EntryPoint = entryPoint;
        }
    }
}

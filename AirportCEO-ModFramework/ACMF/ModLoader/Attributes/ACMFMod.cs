using System;

namespace ACMF.ModLoader.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ACMFMod : Attribute
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string ModVersion { get; private set; }
        public string RequiredACMLVersion { get; private set; }

        public ACMFMod(string id, string name, string modVersion, string requiredACMLVersion)
        {
            ID = id;
            Name = name;
            ModVersion = modVersion;
            RequiredACMLVersion = requiredACMLVersion;
        }
    }
}

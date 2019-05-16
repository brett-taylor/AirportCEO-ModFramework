using System;

namespace ACML.ModLoader.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ACMLMod : Attribute
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string ModVersion { get; private set; }
        public string RequiredACMLVersion { get; private set; }

        public ACMLMod(string id, string name, string modVersion, string requiredACMLVersion)
        {
            ID = id;
            Name = name;
            ModVersion = modVersion;
            RequiredACMLVersion = requiredACMLVersion;
        }
    }
}

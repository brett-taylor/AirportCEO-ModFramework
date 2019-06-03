using System;
using System.Collections.Generic;

namespace ACMF.ModLoader.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ACMFMod : Attribute
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string ModVersion { get; private set; }
        public string RequiredACMFVersion { get; private set; }
        public List<string> RequiredMods { get; private set; }

        public ACMFMod(string id, string name, string modVersion = "1.0.0", string requiredACMLVersion = "0.0.0", string[] requiredMods = null)
        {
            ID = id;
            Name = name;
            ModVersion = modVersion;
            RequiredACMFVersion = requiredACMLVersion;

            RequiredMods = new List<string>();
            if (requiredMods != null)
                RequiredMods.AddRange(requiredMods);
        }
    }
}

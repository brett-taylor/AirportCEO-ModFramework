﻿using ACML.ModLoader;
using ACML.ModLoader.Attributes;
using Harmony;
using System.Reflection;

namespace SampleMod
{
    [ACMLMod(id: "Hanks.Tom.SampleMod", name: "Sample Mod", modVersion: "12.3.5", requiredACMLVersion: "0.1.0")]
    public class EntryPoint
    {
        [ACMLModEntryPoint]
        public static void Entry(Mod mod)
        {
            System.Console.WriteLine("[SampleMod] Sample Mod executing.");
            var harmony = HarmonyInstance.Create("Hanks.Tom.SampleMod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}

﻿using ACML.ModLoader.Attributes;

namespace SampleMod
{
    [ACMLMod(id: "Hanks.Tom.SampleMod", name: "Sample Mod", modVersion: "0.1")]
    public class EntryPoint
    {
        [ACMLModEntryPoint]
        public static void Entry()
        {
            // do stuff to mod.
        }
    }
}
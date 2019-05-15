using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using ACML.ModLoader.Attributes;

namespace ACML.ModLoader
{
    public static class ModLoader
    {
        public static List<Mod> ModsFound { get; private set; }
        public static List<Mod> ModsLoaded { get; private set; }

        public static void Initialise()
        {
            ModsFound = new List<Mod>();
            ModsLoaded = new List<Mod>();

            string modPath = GetModPath();
            if (Directory.Exists(modPath) == false)
            {
                Utilities.Logger.Print($"Failed to located ModPath at: {GetModPath()}");
                return;
            }

            Utilities.Logger.Print($"Located ModPath: {GetModPath()}");
            FileInfo[] allDlls = new DirectoryInfo(modPath).GetFiles("*.dll", SearchOption.AllDirectories);
            foreach(FileInfo dll in allDlls)
                AddToFoundModsIfACMLMod(dll.ToString());

            foreach (Mod mod in ModsFound)
                LoadMod(mod);
        }

        public static string GetModPath()
        {
            return SystemInfo.operatingSystem.ToLower().Contains("windows") ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Apoapsis Studios/Airport CEO/Mods" :
                SystemInfo.operatingSystem.ToLower().Contains("mac") ? Application.persistentDataPath + "/Mods" : "";
        }

        private static void AddToFoundModsIfACMLMod(string dllLocation)
        {
            Utilities.Logger.Print($"Attempting to load dll: {dllLocation}");
            Assembly assembly = Assembly.LoadFrom(dllLocation);

            Type entryPointClass = assembly.ManifestModule.GetTypes().First((x) => x.GetCustomAttributes(typeof(ACMLMod), true).Length > 0);
            if (entryPointClass == null)
                return;

            MethodInfo entryPointMethod = entryPointClass.GetMethods().First((x) => x.GetCustomAttributes(typeof(ACMLModEntryPoint), true).Length > 0);
            if (entryPointMethod == null)
                return;

            ACMLMod acmlMod = (ACMLMod) entryPointClass.GetCustomAttributes(typeof(ACMLMod), true).FirstOrDefault();
            ModsFound.Add(new Mod(acmlMod, assembly, entryPointMethod));
            Utilities.Logger.Print($"Found Mod: {acmlMod.Name}");
        }

        private static void LoadMod(Mod mod)
        {
            Utilities.Logger.Print($"Executing entry point of mod: {mod.ModInfo.Name}");
            try
            {
                mod.EntryPoint.Invoke(null, null);
                Utilities.Logger.Print($"Sucessfully executed entry point of mod: {mod.ModInfo.Name}");
            }
            catch (Exception e)
            {
                Utilities.Logger.Print($"Failed to execute entry point of mod: {mod.ModInfo.Name}");
                Utilities.Logger.Print(e.ToString());
            }
        }
    }
}
